using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XWPF.UserModel;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.OpenXmlFormats.Vml;
using NPOI.OpenXmlFormats.Vml.Wordprocessing;
using System.Xml;
using NPOI.OpenXmlFormats.Dml;
using NPOI.OpenXmlFormats.Dml.WordProcessing;
using NPOI.OpenXmlFormats.Dml.Picture;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
#if NETFRAMEWORK || NET6_0_OR_GREATER
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Drawing;
#endif

namespace ModuleOffice.Tool
{

    public static class WordTool
    {

        // 读取Word文档内容为字符串
        public static XWPFDocument ReadWord(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                return doc;
            }
        }


        // 读取Word文档内容为字符串
        public static string ReadWordText(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                var text = new System.Text.StringBuilder();
                foreach (var para in doc.Paragraphs)
                {
                    text.AppendLine(para.ParagraphText);
                }
                return text.ToString();
            }
        }

        // 保存Word文档
        public static void SaveWord(XWPFDocument doc, string filePath)
        {
            using (FileStream stream = File.Create(filePath))
            {
                doc.Write(stream);
            }
        }
       
        // 替换Word文档中的${}变量，支持多级
        public static List<string>  ReplaceWordVariables_old(XWPFDocument doc, JObject json)
        {
            var regex = new Regex(@"\$\{([^\}]+)\}");
            var replacements = new Dictionary<string, string>();
            var foundVariables = new HashSet<string>();
            var missingVariables = new List<string>();

            // 1. 收集所有${...}变量并准备替换值
            void CollectVariables(IEnumerable<XWPFParagraph> paragraphs)
            {
                foreach (var para in paragraphs)
                {
                    var matches = regex.Matches(para.ParagraphText);
                    foreach (Match match in matches)
                    {
                        string varName = match.Groups[1].Value;
                        string fullVar = match.Value;

                        if (!foundVariables.Contains(fullVar))
                        {
                            foundVariables.Add(fullVar);

                            // 尝试从JSON获取值
                            string value = GetJsonValue(json, varName);
                            if (value != null) // 只在JSON中找到时才替换
                            {
                                replacements[fullVar] = value;
                            }
                            else
                            {
                                missingVariables.Add(fullVar);
                            }
                        }
                    }
                }
            }

            // 收集段落中的变量
            CollectVariables(doc.Paragraphs);

            // 收集表格中的变量
            foreach (var table in doc.Tables)
            {
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.GetTableCells())
                    {
                        CollectVariables(cell.Paragraphs);
                    }
                }
            }

            // 2. 只替换那些在JSON中找到值的变量
            var unprocessedVariables = ReplaceText(doc, replacements);

           
            // 3. 返回结果
            return unprocessedVariables;
        }

        

        // 支持多级key的JObject取值，找不到返回null
        private static string GetJsonValue(JObject json, string path)
        {
            try
            {
                var parts = path.Split('.');
                JToken token = json;
                foreach (var part in parts)
                {
                    if (token == null || !token.HasValues) return null;
                    token = token[part];
                }
                return token?.ToString();
            }
            catch
            {
                return null;
            }
        }



        // 查找并替换所有文本（只替换已知的变量）
        public static List<string> ReplaceText(XWPFDocument doc, Dictionary<string, string> replacements)
        {
            var unprocessedVariables = new List<string>();

            // 递归处理 shape 文本框内容（兼容NPOI 2.x）
            void ProcessShapeTextBoxRaw(NPOI.OpenXmlFormats.Vml.CT_Shape shape)
            {
                var xml = shape.ToString();
                if (string.IsNullOrEmpty(xml)) return;

                var docXml = new XmlDocument();
                docXml.LoadXml(xml);

                var nsmgr = new XmlNamespaceManager(docXml.NameTable);
                nsmgr.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

                var txbxContents = docXml.SelectNodes("//w:txbxContent", nsmgr);
                if (txbxContents == null) return;

                foreach (XmlNode txbxContent in txbxContents)
                {
                    foreach (XmlNode paraNode in txbxContent.SelectNodes("w:p", nsmgr))
                    {
                        var ctP = NPOI.OpenXmlFormats.Wordprocessing.CT_P.Parse(paraNode, nsmgr);
                        var xwpfPara = new XWPFParagraph(ctP, doc);
                        ProcessParagraphWithTextBox(xwpfPara);
                    }
                }
            }

            // 递归处理 DrawingML 文本框内容
            void ProcessDrawingTextBox(XWPFRun run)
            {
                var ctr = run.GetCTR();
                if (ctr == null) return;
                var drawingList = ctr.GetDrawingList();
                if (drawingList == null) return;
                foreach (var drawing in drawingList)
                {
                    // 处理所有 inline
                    if (drawing.inline != null)
                    {
                        foreach (var inline in drawing.inline)
                        {
                            if (inline != null && inline.graphic != null)
                                ProcessDrawingGraphicData(inline.graphic);
                        }
                    }
                    // 处理所有 anchor
                    if (drawing.anchor != null)
                    {
                        foreach (var anchor in drawing.anchor)
                        {
                            if (anchor != null && anchor.graphic != null)
                                ProcessDrawingGraphicData(anchor.graphic);
                        }
                    }
                }
            }

            void ProcessDrawingGraphicData(CT_GraphicalObject graphic)
            {
                if (graphic == null || graphic.graphicData == null) return;
                var xml = graphic.graphicData.ToString();
                if (string.IsNullOrEmpty(xml)) return;
                var docXml = new System.Xml.XmlDocument();
                docXml.LoadXml(xml);
                var nsmgr = new System.Xml.XmlNamespaceManager(docXml.NameTable);
                nsmgr.AddNamespace("a", "http://schemas.openxmlformats.org/drawingml/2006/main");
                // 查找所有 a:txBody 节点
                var txBodyNodes = docXml.SelectNodes("//a:txBody", nsmgr);
                if (txBodyNodes == null) return;
                foreach (System.Xml.XmlNode txBody in txBodyNodes)
                {
                    foreach (System.Xml.XmlNode paraNode in txBody.SelectNodes("a:p", nsmgr))
                    {
                        foreach (System.Xml.XmlNode rNode in paraNode.SelectNodes("a:r", nsmgr))
                        {
                            foreach (System.Xml.XmlNode tNode in rNode.SelectNodes("a:t", nsmgr))
                            {
                                if (tNode.InnerText != null)
                                {
                                    string newText = tNode.InnerText;
                                    bool changed = false;
                                    foreach (var replacement in replacements)
                                    {
                                        if (newText.Contains(replacement.Key))
                                        {
                                            newText = newText.Replace(replacement.Key, replacement.Value);
                                            changed = true;
                                        }
                                    }
                                    if (changed)
                                    {
                                        tNode.InnerText = newText;
                                    }
                                }
                            }
                        }
                    }
                }
                // 替换后的XML写回graphicData（如需保存到文档，可扩展此处）
                // graphic.graphicData = ...
            }

            // 递归处理段落，包括文本框内容
            void ProcessParagraphWithTextBox(XWPFParagraph para)
            {
                ProcessParagraph(para);
                foreach (var run in para.Runs)
                {
                    // 递归处理DrawingML文本框
                    ProcessDrawingTextBox(run);
                    var ctr = run.GetCTR();
                    if (ctr == null || ctr.Items == null) continue;
                    foreach (var item in ctr.Items)
                    {
                        if (item is NPOI.OpenXmlFormats.Wordprocessing.CT_Picture pict && pict.Items != null)
                        {
                            foreach (var pictItem in pict.Items)
                            {
                                if (pictItem is NPOI.OpenXmlFormats.Vml.CT_Shape shape)
                                {
                                    ProcessShapeTextBoxRaw(shape);
                                }
                            }
                        }
                        if (item is NPOI.OpenXmlFormats.Wordprocessing.CT_Object obj && obj.Items != null)
                        {
                            foreach (var objItem in obj.Items)
                            {
                                if (objItem is NPOI.OpenXmlFormats.Wordprocessing.CT_Picture pict2 && pict2.Items != null)
                                {
                                    foreach (var pictItem in pict2.Items)
                                    {
                                        if (pictItem is NPOI.OpenXmlFormats.Vml.CT_Shape shape)
                                        {
                                            ProcessShapeTextBoxRaw(shape);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // 修改原有ProcessParagraph为私有静态方法，供递归调用
            void ProcessParagraph(XWPFParagraph para)
            {
                string fullText = para.ParagraphText;
                bool hasReplacements = replacements.Keys.Any(k => fullText.Contains(k));

                if (!hasReplacements) return;

                // 合并所有run的文本
                StringBuilder combinedText = new StringBuilder();
                IList<XWPFRun> runs = para.Runs;
                foreach (var run in runs)
                {
                    combinedText.Append(run.Text);
                }

                string newText = combinedText.ToString();
                bool textChanged = false;

                // 只替换replacements字典中存在的键
                foreach (var replacement in replacements)
                {
                    if (newText.Contains(replacement.Key))
                    {
                        newText = newText.Replace(replacement.Key, replacement.Value);
                        textChanged = true;
                    }
                }

                if (textChanged)
                {
                    // 清空所有run
                    foreach (var run in runs)
                    {
                        run.SetText("", 0);
                    }

                    // 在第一个run中设置新文本
                    if (runs.Count > 0)
                    {
                        runs[0].SetText(newText, 0);
                    }
                    else
                    {
                        para.CreateRun().SetText(newText);
                    }
                }
            }

            // 处理普通段落
            foreach (var para in doc.Paragraphs)
            {
                ProcessParagraphWithTextBox(para);
            }

            // 处理表格中的段落
            foreach (var table in doc.Tables)
            {
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.GetTableCells())
                    {
                        foreach (var para in cell.Paragraphs)
                        {
                            ProcessParagraphWithTextBox(para);
                        }
                    }
                }
            }

            return unprocessedVariables;
        }



        /// <summary>
        /// 使用Open XML SDK递归替换Word文档所有${变量}（包括文本框、正文、表格等），支持JObject和多级key
        /// </summary>
        public static List<string> ReplaceVariablesWithOpenXml(string filePath, JObject json)
        {
            var regex = new Regex(@"\$\{([^\}]+)\}");
            var replacements = new Dictionary<string, string>();
            var missingVariables = new List<string>();

            // 1. 收集所有变量（正文+表格+DrawingML+VML）
            using (var doc = WordprocessingDocument.Open(filePath, true))
            {
                // 收集正文和表格
                foreach (var text in doc.MainDocumentPart.Document.Descendants<Text>())
                {
                    foreach (Match match in regex.Matches(text.Text))
                    {
                        string varName = match.Groups[1].Value;
                        string fullVar = match.Value;
                        if (!replacements.ContainsKey(fullVar))
                        {
                            string value = GetJsonValue(json, varName);
                            if (value != null)
                                replacements[fullVar] = value;
                            else
                                missingVariables.Add(fullVar);
                        }
                    }
                }
                // 收集DrawingML
                foreach (var t in doc.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Drawing.Text>())
                {
                    foreach (Match match in regex.Matches(t.Text))
                    {
                        string varName = match.Groups[1].Value;
                        string fullVar = match.Value;
                        if (!replacements.ContainsKey(fullVar))
                        {
                            string value = GetJsonValue(json, varName);
                            if (value != null)
                                replacements[fullVar] = value;
                            else
                                missingVariables.Add(fullVar);
                        }
                    }
                }
                // 收集VML
                foreach (var sdt in doc.MainDocumentPart.Document.Descendants<SdtElement>())
                {
                    foreach (var text in sdt.Descendants<Text>())
                    {
                        foreach (Match match in regex.Matches(text.Text))
                        {
                            string varName = match.Groups[1].Value;
                            string fullVar = match.Value;
                            if (!replacements.ContainsKey(fullVar))
                            {
                                string value = GetJsonValue(json, varName);
                                if (value != null)
                                    replacements[fullVar] = value;
                                else
                                    missingVariables.Add(fullVar);
                            }
                        }
                    }
                }

                // 2. 替换正文、表格、页眉页脚
                ReplaceInBody(doc.MainDocumentPart.Document.Body, replacements);
                // 3. 替换所有 DrawingML 文本框
                foreach (var drawing in doc.MainDocumentPart.Document.Descendants<Drawing>())
                {
                    ReplaceInDrawing(drawing, replacements);
                }
                // 4. 替换所有 VML 文本框（如有）
                foreach (var sdt in doc.MainDocumentPart.Document.Descendants<SdtElement>())
                {
                    ReplaceInSdt(sdt, replacements);
                }
                doc.MainDocumentPart.Document.Save();
            }
            return missingVariables;
        }

        private static void ReplaceInBody(OpenXmlElement element, Dictionary<string, string> replacements)
        {
            foreach (var text in element.Descendants<Text>())
            {
                foreach (var kv in replacements)
                {
                    if (text.Text.Contains(kv.Key))
                    {
                        text.Text = text.Text.Replace(kv.Key, kv.Value);
                    }
                }
            }
        }

        private static void ReplaceInDrawing(Drawing drawing, Dictionary<string, string> replacements)
        {
            foreach (var t in drawing.Descendants<DocumentFormat.OpenXml.Drawing.Text>())
            {
                foreach (var kv in replacements)
                {
                    if (t.Text.Contains(kv.Key))
                    {
                        t.Text = t.Text.Replace(kv.Key, kv.Value);
                    }
                }
            }
        }

        private static void ReplaceInSdt(SdtElement sdt, Dictionary<string, string> replacements)
        {
            foreach (var text in sdt.Descendants<Text>())
            {
                foreach (var kv in replacements)
                {
                    if (text.Text.Contains(kv.Key))
                    {
                        text.Text = text.Text.Replace(kv.Key, kv.Value);
                    }
                }
            }
        }
        /// <summary>
        /// 替换word里面表格中的变量$[变量值]
        /// 循环数组，数组里面内容都是JObject，替换表格中的变量，只有某一行写入了变量，第二行没有变量，也要写入值，如果行数不够，那么要新增行，样式要采用变量行的样式
        /// </summary>         
        /// <param name="word"></param>
        /// <param name="array"></param>
        /// <returns>返回没有替换掉的变量</returns>
        public static List<string> ReplaceWordVariables(XWPFDocument word, JArray array)
        {
            if (array == null || array.Count == 0)
                return new List<string>();

            var missingVariables = new List<string>();
            var regex = new Regex(@"\$\[([^\]]+)\]"); // 匹配 $[变量名]
            foreach (var table in word.Tables)
            {
                for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
                {
                    var row = table.Rows[rowIndex];
                    // 检查本行是否有 $[变量]，作为模板行
                    bool hasVar = row.GetTableCells().Any(cell =>
                        cell.Paragraphs.Any(p => regex.IsMatch(p.ParagraphText)));
                    if (!hasVar) continue;

                    int dataCount = array.Count;
                    // 先批量复制模板行（保留原模板行，后面依次填充）
                    List<XWPFTableRow> dataRows = new List<XWPFTableRow>();
                    dataRows.Add(row); // 第一行用模板行
                    for (int i = 1; i < dataCount; i++)
                    {
                        var newRow = table.InsertNewTableRow(rowIndex + i);
                        var templateCells = row.GetTableCells();
                        foreach (var cell in templateCells)
                        {
                            var newCell = newRow.AddNewTableCell();
                            // 复制单元格内容
                            newCell.Paragraphs.Clear();
                            foreach (var para in cell.Paragraphs)
                            {
                                var newPara = newCell.AddParagraph();
                                newPara.Alignment = para.Alignment;
                                newPara.VerticalAlignment = para.VerticalAlignment;
                                foreach (var run in para.Runs)
                                {
                                    var newRun = newPara.CreateRun();
                                    newRun.SetText(run.Text);
                                    newRun.IsBold = run.IsBold;
                                    newRun.IsItalic = run.IsItalic;
                                    newRun.FontSize = run.FontSize;
                                    newRun.FontFamily = run.FontFamily;
                                    newRun.SetColor(run.GetColor());
                                }
                            }
                            // 复制单元格样式
                            newCell.SetColor(cell.GetColor());
                            newCell.SetVerticalAlignment((XWPFTableCell.XWPFVertAlign)cell.GetVerticalAlignment());
                        }
                        dataRows.Add(newRow);
                    }
                    // 再逐行替换变量
                    for (int i = 0; i < dataCount; i++)
                    {
                        JObject obj = array[i] as JObject;
                        var targetRow = dataRows[i];
                        foreach (var cell in targetRow.GetTableCells())
                        {
                            foreach (var para in cell.Paragraphs)
                            {
                                string text = para.ParagraphText;
                                foreach (Match match in regex.Matches(text))
                                {
                                    string varName = match.Groups[1].Value;
                                    string fullVar = match.Value;
                                    string value = obj[varName]?.ToString();
                                    if (value != null)
                                    {
                                        para.ReplaceText(fullVar, value);
                                    }
                                    else
                                    {
                                        missingVariables.Add(fullVar);
                                    }
                                }
                            }
                        }
                    }
                    // 只处理第一个模板行
                    break;
                }
            }
            return missingVariables;
        }
















        public static List<string> ReplaceWordVariables(XWPFDocument doc, JObject json)
        {
            var regex = new Regex(@"\$\{([^\}]+)\}");
            var replacements = new Dictionary<string, string>();
            var missingVariables = new List<string>();

            // 1. 收集所有变量并准备替换值
            void CollectVariables(IEnumerable<XWPFParagraph> paragraphs)
            {
                foreach (var para in paragraphs)
                {
                    var matches = regex.Matches(para.ParagraphText);
                    foreach (Match match in matches)
                    {
                        string varName = match.Groups[1].Value;
                        string fullVar = match.Value;

                        if (!replacements.ContainsKey(fullVar))
                        {
                            string value = GetJsonValue(json, varName);
                            if (value != null)
                                replacements[fullVar] = value;
                            else
                                missingVariables.Add(fullVar);
                        }
                    }
                }
            }

            // 收集段落中的变量
            CollectVariables(doc.Paragraphs);

            // 收集表格中的变量
            foreach (var table in doc.Tables)
            {
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.GetTableCells())
                    {
                        CollectVariables(cell.Paragraphs);
                    }
                }
            }

            // 2. 替换变量并处理下划线
            var unprocessedVariables = ReplaceVariablesWithUnderline(doc, replacements);

            return unprocessedVariables;
        }

        private static List<string> ReplaceVariablesWithUnderline(XWPFDocument doc, Dictionary<string, string> replacements)
        {
            var unprocessedVariables = new List<string>();
            var regex = new Regex(@"\$\{([^\}]+)\}");

            // 处理段落
            void ProcessParagraph(XWPFParagraph para)
            {
                // 查找所有变量匹配
                var matches = regex.Matches(para.ParagraphText);
                if (matches.Count == 0) return;

                // 按变量在文本中的位置排序
                var sortedMatches = matches.Cast<Match>()
                    .OrderBy(m => m.Index)
                    .ToList();

                // 收集所有run的信息
                var runInfos = para.Runs.Select(r => new RunInfo
                {
                    Run = r,
                    Text = r.Text,
                    UnderlineType = r.Underline,
                    StartPos = 0, // 将在后面计算
                    EndPos = 0    // 将在后面计算
                }).ToList();

                // 计算每个run的全局位置范围
                int globalPos = 0;
                foreach (var runInfo in runInfos)
                {
                    runInfo.StartPos = globalPos;
                    runInfo.EndPos = globalPos + runInfo.Text.Length - 1;
                    globalPos += runInfo.Text.Length;
                }

                // 处理每个匹配的变量
                foreach (var match in sortedMatches)
                {
                    string fullVar = match.Value;
                    if (!replacements.ContainsKey(fullVar))
                    {
                        unprocessedVariables.Add(fullVar);
                        continue;
                    }

                    string replacement = replacements[fullVar];
                    int varStart = match.Index;
                    int varEnd = match.Index + match.Length - 1;

                    // 找出包含变量的所有run
                    var varRuns = runInfos.Where(r =>
                        !(r.EndPos < varStart || r.StartPos > varEnd)).ToList();

                    if (varRuns.Count == 0) continue;

                    // 计算变量在第一个run中的起始位置
                    int firstRunLocalStart = varStart - varRuns.First().StartPos;
                    // 计算变量在最后一个run中的结束位置
                    int lastRunLocalEnd = varEnd - varRuns.Last().StartPos;

                    // 记录原始下划线长度
                    int originalUnderlineLength = match.Length;
                    bool hasUnderline = varRuns.Any(r => r.UnderlineType != UnderlinePatterns.None);

                    // 找出包含"$"符号的run（优先保留这个run）
                    var dollarRun = varRuns.FirstOrDefault(r => r.Text.Contains("$"));
                    if (dollarRun == null) dollarRun = varRuns.First();

                    // 清空或删除其他run的内容
                    foreach (var runInfo in varRuns)
                    {
                        if (runInfo == dollarRun)
                        {
                            // 保留这个run，只替换变量部分
                            string newText = runInfo.Text;
                            if (runInfo == varRuns.First() && runInfo == varRuns.Last())
                            {
                                // 变量完全在一个run中
                                newText = newText.Remove(firstRunLocalStart, match.Length);
                                newText = newText.Insert(firstRunLocalStart, replacement);
                            }
                            else if (runInfo == varRuns.First())
                            {
                                // 变量开始于这个run
                                newText = newText.Remove(firstRunLocalStart);
                                newText += replacement;
                            }
                            else if (runInfo == varRuns.Last())
                            {
                                // 变量结束于这个run
                                newText = newText.Substring(lastRunLocalEnd + 1);
                                newText = replacement + newText;
                            }
                            else
                            {
                                // 变量中间部分在这个run中
                                newText = replacement;
                            }

                            runInfo.Run.SetText(newText);

                            // 如果需要保留下划线，调整下划线长度
                            if (hasUnderline && replacement.Length < originalUnderlineLength)
                            {
                                runInfo.Run.Underline = dollarRun.UnderlineType;
                                runInfo.Run.SetText(newText.PadRight(originalUnderlineLength, ' '));
                            }
                        }
                        else
                        {
                            // 清空其他run
                            runInfo.Run.SetText("");
                        }
                    }
                }
            }

            // 处理所有段落
            foreach (var para in doc.Paragraphs)
            {
                ProcessParagraph(para);
            }

            // 处理表格中的段落
            foreach (var table in doc.Tables)
            {
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.GetTableCells())
                    {
                        foreach (var para in cell.Paragraphs)
                        {
                            ProcessParagraph(para);
                        }
                    }
                }
            }

            return unprocessedVariables;
        }

        private class RunInfo
        {
            public XWPFRun Run { get; set; }
            public string Text { get; set; }
            public UnderlinePatterns UnderlineType { get; set; }
            public int StartPos { get; set; }
            public int EndPos { get; set; }
        }
    }



    
}
