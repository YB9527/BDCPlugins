using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using ModuleOffice.Entity;
using System.Windows.Forms;
using ModuleBase.Tool;
using System.Globalization;
using ModuleOffice.ExceptionBase;

namespace ModuleOffice.Tool
{

    public class ExcelTool
    {
        public static IWorkbook ReadExcel(string filePath) {

            if (!File.Exists(filePath))
            {
                throw new ExceptionSheetNotFound("文件没有找到：" + filePath);
            }
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook;
                if (Path.GetExtension(filePath).ToLower() == ".xlsx")
                {
                    workbook = new XSSFWorkbook(fileStream);
                }
                else
                {
                    workbook = new HSSFWorkbook(fileStream);
                }
                return workbook;
            }
           
        }
        /// <summary>
        /// 检查文件是可写
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool CheckExcelWrite(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("文件没有找到：" + filePath);
                return false;
            }
            try
            {
                ReadExcel(filePath);
                return true;
            }
            catch (Exception ex)
            {
                if ( ex.Message.Contains("进程使用"))
                {
                    return false;
                }
                else
                {
                    throw ex;
                }
            }
        }

        

        public static ISheet ReadSheet(string filePath, int index, bool isCpoy = true)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("文件没有找到：" + filePath);
                return null;
            }
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook;
                    if (Path.GetExtension(filePath).ToLower() == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(fileStream);
                    }
                    else
                    {
                        workbook = new HSSFWorkbook(fileStream);
                    }

                    // 获取第一个Sheet
                    ISheet sheet = workbook.GetSheetAt(index);
                    return sheet;
                }
            }
            catch (Exception ex)
            {
                if (isCpoy && ex.Message.Contains("进程使用"))
                {
                    //拷贝一个副本
                    string timestamp = DateTime.Now.ToString("yy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture);
                    string fileName2 = Path.Combine(AppTool.GetTempDirectory(), $"{timestamp}_临时文件" + Path.GetExtension(filePath));

                    FileTool.CopyFile(filePath, fileName2);
                    return ReadSheet(fileName2, index, isCpoy = false);
                }
                else
                {
                    throw ex;
                }
            }
            return null;

        }

        

        public static ISheet ReadSheet(string filePath, String sheetName, bool isCpoy = true)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("文件没有找到：" + filePath);
                return null;
            }


            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook;
                    if (Path.GetExtension(filePath).ToLower() == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(fileStream);
                    }
                    else
                    {
                        workbook = new HSSFWorkbook(fileStream);
                    }

                    // 获取第一个Sheet
                    ISheet sheet = workbook.GetSheet(sheetName);
                    return sheet;
                }
            }
            catch (Exception ex)
            {
                if (isCpoy && ex.Message.Contains("进程使用"))
                {
                    //拷贝一个副本
                    string timestamp = DateTime.Now.ToString("yy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture);
                    string fileName2 = Path.Combine(AppTool.GetTempDirectory(), $"{timestamp}_临时文件" + Path.GetExtension(filePath));

                    FileTool.CopyFile(filePath, fileName2);
                    return ReadSheet(fileName2, sheetName, isCpoy = false);
                }
                else
                {
                    throw ex;
                }
            }
            return null;



        }

        public static void Save2(IWorkbook workbook, FileStream fs)
        {
            workbook.Write(fs);
        }

        public delegate void ReadRowDel(IRow row);
        public static void ReadRow(ISheet sheet, ReadRowDel readRowDel, int startRowIndex)
        {
            for (int i = startRowIndex; i <= sheet.LastRowNum; i++)
            {
                IRow row = ReadRow(sheet, i);
                readRowDel(row);
            }
        }
        public static void ReadRow(string filePath, int index, ReadRowDel readRowDel, int startRowIndex)
        {
            ISheet sheet = ReadSheet(filePath, index);
            ReadRow(sheet, readRowDel, startRowIndex);
        }


        public static void ReadRow(string filePath, String sheetName, ReadRowDel readRowDel, int startRowIndex)
        {
            ISheet sheet = ReadSheet(filePath, sheetName);
            ReadRow(sheet, readRowDel, startRowIndex);
        }

        


        public static IRow ReadRow(ISheet sheet, int rowIndex)
        {

            IRow row = sheet.GetRow(rowIndex);
            return row;
        }
        public ICell ReadCell(IRow row, int cellIndex)
        {
            if (row == null)
            {
                return null;
            }
            ICell cell = row.GetCell(cellIndex);
            return cell;
        }

        public static String ReadCellValueString(IRow row, int cellIndex)
        {
            if (row == null)
            {
                return null;
            }
            ICell cell = row.GetCell(cellIndex);
            String val = GetCellValue(cell);
            return val;
        }



        public static String GetCellValue(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }
            switch (cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        //return cell.DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
                        return cell.DateCellValue.ToString();
                    }
                    else
                    {
                        return cell.NumericCellValue.ToString();
                    }
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Formula:
                    return cell.CellFormula;
                default:
                    return string.Empty;
            }
        }

        public static void Save(IWorkbook workbook, string saveExcelPath)
        {
            try
            {
                // 使用 FileStream 将工作簿写入到指定的文件中
                if (File.Exists(saveExcelPath))
                {
                    File.Delete(saveExcelPath);
                }

                using (FileStream fileStream = new FileStream(saveExcelPath, FileMode.Create))
                {
                    workbook.Write(fileStream);
                    // Console.WriteLine($"Excel 文件已成功保存到：{saveExcelPath}");
                }
            }
            catch (Exception ex)
            {
                // 捕获并处理异常x
                Console.WriteLine("发生错误：");
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public static List<Dictionary<string, string>> ReadExcelToJson(string filePath, int sheetIndex = 0, int headerRowIndex = 0)
        {
            var result = new List<Dictionary<string, string>>();
            var sheet = ReadSheet(filePath, sheetIndex, true);
            if (sheet == null) return result;
            var headerRow = sheet.GetRow(headerRowIndex);
            if (headerRow == null) return result;
            int cellCount = headerRow.LastCellNum;
            List<string> headers = new List<string>();
            for (int i = 0; i < cellCount; i++)
            {
                headers.Add(GetCellValue(headerRow.GetCell(i)));
            }
            for (int i = headerRowIndex + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                var dict = new Dictionary<string, string>();
                for (int j = 0; j < cellCount; j++)
                {
                    dict[headers[j]] = GetCellValue(row.GetCell(j));
                }
                result.Add(dict);
            }
            return result;
        }



        public static List<Dictionary<string, string>> ReadExcelToJson(string filePath, String sheetName, int headerRowIndex = 0)
        {
            var result = new List<Dictionary<string, string>>();
            var sheet = ReadSheet(filePath, sheetName,true);
            if(sheet == null)
            {
                MessageBox.Show($"Excel中没有名为“{sheetName}”的Sheet");
            }
            if (sheet == null) return result;
            var headerRow = sheet.GetRow(headerRowIndex);
            if (headerRow == null) return result;
            int cellCount = headerRow.LastCellNum;
            List<string> headers = new List<string>();
            for (int i = 0; i < cellCount; i++)
            {
                headers.Add(GetCellValue(headerRow.GetCell(i)));
            }
            for (int i = headerRowIndex + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                var dict = new Dictionary<string, string>();
                for (int j = 0; j < cellCount; j++)
                {
                    if (!StringUtils.IsNullOrTrimEmpty(headers[j]))
                    {
                        dict[headers[j]] = GetCellValue(row.GetCell(j));
                    }

                }
                result.Add(dict);
            }
            return result;
        }


        public static void WriteRows(ISheet sheet, JArray array)
        {
            if (array == null || array.Count == 0) return;

            // 获取所有可能的键（使用第一行数据作为参考）
            var firstItem = array[0] as JObject;
            if (firstItem == null) return;

            // 创建标题行（第一行）
            IRow headerRow = sheet.CreateRow(0);
            int colIndex = 0;
            var columnMapping = new Dictionary<string, int>();

            // 写入标题行并建立列名到索引的映射
            foreach (var property in firstItem.Properties())
            {
                headerRow.CreateCell(colIndex).SetCellValue(property.Name);
                columnMapping[property.Name] = colIndex;
                colIndex++;
            }

            // 写入数据行
            for (int rowIndex = 0; rowIndex < array.Count; rowIndex++)
            {
                var rowData = array[rowIndex] as JObject;
                if (rowData == null) continue;

                IRow dataRow = sheet.CreateRow(rowIndex + 1); // +1 因为第一行是标题

                foreach (var property in rowData.Properties())
                {
                    if (columnMapping.TryGetValue(property.Name, out int cellIndex))
                    {
                        var cell = dataRow.CreateCell(cellIndex);
                        SetCellValue(cell, property.Value);
                    }
                }
            }
        }
       
        public static void WriteRowsByHeaderRow(ISheet sheet, List<JObject> datas, int headerRowIndex)
        {

            WriteRowsByHeaderRow(sheet, JArray.FromObject(datas), headerRowIndex);
        }
        public static void WriteRowsByHeaderRow(ISheet sheet, JArray array,int headerRowIndex)
        {
            if (array == null || array.Count == 0) return;

            // 获取所有可能的键（使用第一行数据作为参考）
            var firstItem = array[0] as JObject;
            if (firstItem == null) return;

            // 创建标题行（第一行）
            IRow headerRow = sheet.GetRow(headerRowIndex);
            var columnMapping = new Dictionary<string, int>();

            // 写入标题行并建立列名到索引的映射
            foreach (var property in firstItem.Properties())
            {
               
                for (int i = 0; i <= headerRow.LastCellNum; i++)
                {
                    if(GetCellValue(headerRow,i) == property.Name)
                    {
                        columnMapping[property.Name] = i;
                        break;
                    }
                }
            }

            // 写入数据行
            for (int rowIndex = 0; rowIndex < array.Count; rowIndex++)
            {
                var rowData = array[rowIndex] as JObject;
                if (rowData == null) continue;

                IRow dataRow = sheet.CreateRow(headerRowIndex + rowIndex + 1);
                foreach (var property in rowData.Properties())
                {
                    if (columnMapping.TryGetValue(property.Name, out int cellIndex))
                    {
                        var cell = dataRow.CreateCell(cellIndex);
                        SetCellValue(cell, property.Value);
                    }
                }
            }
        }

        public static void ReplaceCellValueByHeaderRow(ISheet sheet, string primaryKey, List<JObject> datas, int headerRowIndex)
        {

            ReplaceCellValueByHeaderRow(sheet, primaryKey, JArray.FromObject(datas), headerRowIndex);
        }
        public static void ReplaceCellValueByHeaderRow(ISheet sheet, string primaryKey, JArray array, int headerRowIndex)
        {
            SheetEntity sheetEntity = new SheetEntity() {
                PrimaryKey = primaryKey,
                Rows = array.ToObject<List<Dictionary< String,Object>>>()
            };
            Refresh(sheet, sheetEntity, headerRowIndex);
        }
        // 辅助方法：根据JToken类型设置单元格值
        private static void SetCellValue(ICell cell, JToken value)
        {
            if (value == null || value.Type == JTokenType.Null)
            {
                cell.SetCellValue((string)null);
                return;
            }

            switch (value.Type)
            {
                case JTokenType.String:
                    cell.SetCellValue(value.Value<string>());
                    break;
                case JTokenType.Integer:
                    cell.SetCellValue(value.Value<long>());
                    break;
                case JTokenType.Float:
                    cell.SetCellValue(value.Value<double>());
                    break;
                case JTokenType.Boolean:
                    cell.SetCellValue(value.Value<bool>());
                    break;
                case JTokenType.Date:
                    cell.SetCellValue(value.Value<DateTime>());
                    break;
                default:
                    cell.SetCellValue(value.ToString());
                    break;
            }
        }

        public static Dictionary<string, List<string>> ReadExcelCellValues(string filePath, String sheetName, int headerRowIndex = 0)
        {
            var result = new Dictionary<string, List<string>>();
            var sheet = ReadSheet(filePath, sheetName);
            if (sheet == null)
            {
                throw new ExceptionSheetNotFound($"文件{filePath}，表格页名称：" + sheetName + ",不存在");
            };
            var headerRow = sheet.GetRow(headerRowIndex);
            if (headerRow == null) return result;
            int cellCount = headerRow.LastCellNum;
            List<string> values = new List<string>();
            for (int i = 0; i < cellCount; i++)
            {
                String field = GetCellValue(headerRow.GetCell(i));
                values = new List<string>();
                if (result.ContainsKey(field))
                {
                    throw new ExceptionSheetNotFound($"文件{filePath}，表格页名称：" + sheetName + $",标题重复：{field}");
                }
                result.Add(field, values);

                for (int j = headerRowIndex + 1; j <= sheet.LastRowNum; j++)
                {
                    var row = sheet.GetRow(j);
                    if (row == null) continue;
                    var dict = new Dictionary<string, string>();
                    string val = GetCellValue(row.GetCell(i));
                    if (val == null || val.Trim() == "")
                    {
                        continue;
                    }
                    values.Add(val);

                }
            }

            return result;
        }

        public static void WriteRows(ISheet sheet, string primaryFiledName, List<Dictionary<String, String>> values, int headerIndex)
        {
            if (values == null || values.Count == 0) return;



            // 创建标题行（第一行）
            IRow headerRow = sheet.GetRow(headerIndex);
            var columnMapping = new Dictionary<string, int>();

            // 写入标题行并建立列名到索引的映射
            foreach (var property in values[0].Keys)
            {
                if (StringUtils.IsNullOrTrimEmpty(property))
                {
                    continue;
                }
                for (int i = 0; i <= headerRow.LastCellNum; i++)
                {
                    String value = ReadCellValueString(headerRow, i);
                    if (StringUtils.TrimEq(property, value))
                    {
                        columnMapping.Add(value, i);
                        break;
                    }
                }
            }

            // 写入数据行
            for (int rowIndex = headerIndex+1; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex);
                if (row == null) continue;
                String primaryValue = null;
                //找到主键值
                foreach (var item in columnMapping.Keys)
                {
                    if (item == primaryFiledName)
                    {
                        primaryValue = ReadCellValueString(row, columnMapping[item]);
                        break;
                    }
                }
                if (primaryValue == null)
                {
                    continue;
                }
                //找到对象
                Dictionary<String, String> updateData = null;
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i][primaryFiledName] == primaryValue)
                    {
                        updateData = values[i];
                        break;
                    }
                }
                if (updateData == null)
                {
                    continue;
                }
                //修改行数据
                foreach (var key in columnMapping.Keys)
                {
                    SetCellValue(row, columnMapping[key], updateData[key]);
                }
            }
        }
        private static String GetCellValue(IRow row, int ceollIndex)
        {
            ICell cell = row.GetCell(ceollIndex);
            if (cell == null)
            {
                return "";
            }
            return GetCellValue(cell);
        }
        private static void SetCellValue(IRow row, int ceollIndex, String value)
        {
            ICell cell = row.GetCell(ceollIndex);
            if (cell == null)
            {
                cell = row.CreateCell(ceollIndex);
            }
            SetCellValue(cell, value);
        }
        /*public static ISheet ReadSheet(string filePath, int index)
        {

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook;
                if (Path.GetExtension(filePath).ToLower() == ".xlsx")
                {
                    workbook = new XSSFWorkbook(fileStream);

                }
                else
                {
                    workbook = new HSSFWorkbook(fileStream);
                }

                // 获取第一个Sheet
                ISheet sheet = workbook.GetSheetAt(index);
                return sheet;
            }
        }

       

        public static ISheet ReadSheet(string filePath, String sheetName)
        {

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook;
                if (Path.GetExtension(filePath).ToLower() == ".xlsx")
                {
                    workbook = new XSSFWorkbook(fileStream);
                }
                else
                {
                    workbook = new HSSFWorkbook(fileStream);
                }

                // 获取第一个Sheet
                ISheet sheet = workbook.GetSheet(sheetName);
                return sheet;
            }
        }*/
       

       

 

        /*public static List<Dictionary<string, string>> ReadExcelToJson(string filePath, int sheetIndex = 0)
        {
            var result = new List<Dictionary<string, string>>();
            var sheet = ReadSheet(filePath, sheetIndex);
            if (sheet == null) return result;
            var headerRow = sheet.GetRow(0);
            if (headerRow == null) return result;
            int cellCount = headerRow.LastCellNum;
            List<string> headers = new List<string>();
            for (int i = 0; i < cellCount; i++)
            {
                headers.Add(GetCellValue(headerRow.GetCell(i)));
            }
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                var dict = new Dictionary<string, string>();
                for (int j = 0; j < cellCount; j++)
                {
                    dict[headers[j]] = GetCellValue(row.GetCell(j));
                }
                result.Add(dict);
            }
            return result;
        }*/

        public static JArray ReadSheetToJArray(string filePath, int sheetNum, int titleNum)
        {
            var jArray = new JArray();
            var sheet = ReadSheet(filePath, sheetNum);
            if (sheet == null) return jArray;

            // 读取表头
            var headerRow = sheet.GetRow(titleNum);
            if (headerRow == null) return jArray;
            int cellCount = headerRow.LastCellNum;
            List<string> headers = new List<string>();
            for (int i = 0; i < cellCount; i++)
            {
                headers.Add(GetCellValue(headerRow.GetCell(i)));
            }
            // 读取数据行
            for (int i = titleNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                var jObj = new JObject();
                for (int j = 0; j < cellCount; j++)
                {
                    jObj[headers[j]] = GetCellValue(row.GetCell(j));
                }
                jArray.Add(jObj);
            }
            return jArray;
        }


        /// <summary>
        /// 根据主键列更新Excel中的内容（支持自定义标题行）
        /// </summary>
        /// <param name="sheet">工作表对象</param>
        /// <param name="sheetEntity">包含更新数据和主键信息的实体</param>
        /// <param name="headerRowIndex">标题行索引（从0开始，默认0）</param>
        public static void Refresh(ISheet sheet, SheetEntity sheetEntity, int headerRowIndex)
        {
            if (sheet == null)
                throw new ArgumentNullException(nameof(sheet));

            if (sheetEntity == null)
                throw new ArgumentNullException(nameof(sheetEntity));

            if (string.IsNullOrEmpty(sheetEntity.PrimaryKey))
                throw new ArgumentException("PrimaryKey不能为空");

            // 获取标题行
            IRow headerRow = sheet.GetRow(headerRowIndex);
            if (headerRow == null)
                throw new InvalidOperationException($"找不到标题行（行号：{headerRowIndex + 1}）");

            // 获取主键列的索引
            int pkColumnIndex = FindColumnIndex(headerRow, sheetEntity.PrimaryKey);
            if (pkColumnIndex < 0)
                throw new InvalidOperationException($"找不到主键列 '{sheetEntity.PrimaryKey}'");

            // 遍历所有要更新的数据
            foreach (var rowData in sheetEntity.Rows)
            {
                // 获取当前行的主键值
                if (!rowData.TryGetValue(sheetEntity.PrimaryKey, out object primaryKeyValue))
                    continue;

                // 查找包含该主键值的行（从标题行下一行开始）
                int rowIndex = FindRowIndexByPrimaryKey(sheet, pkColumnIndex, primaryKeyValue, headerRowIndex + 1);
                if (rowIndex < 0)
                    continue; // 没找到匹配的行，跳过

                // 获取或创建行
                IRow row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);

                // 更新行数据
                foreach (var kvp in rowData)
                {
                    if (kvp.Key == sheetEntity.PrimaryKey)
                        continue; // 跳过主键列

                    int colIndex = FindColumnIndex(headerRow, kvp.Key);
                    if (colIndex >= 0)
                    {
                        ICell cell = row.GetCell(colIndex) ?? row.CreateCell(colIndex);
                        SetCellValue(cell, kvp.Value);
                    }
                }
            }
        }


        /// <summary>
        /// 在指定标题行中查找列索引
        /// </summary>
        private static int FindColumnIndex(IRow headerRow, string columnName)
        {
            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                ICell cell = headerRow.GetCell(i);
                if (cell != null && GetCellValue(cell).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 根据主键值查找行索引（从指定行开始查找）
        /// </summary>
        private static int FindRowIndexByPrimaryKey(ISheet sheet, int pkColumnIndex, object primaryKeyValue, int startRowIndex)
        {
            if (primaryKeyValue == null)
                return -1;

            string pkValueStr = primaryKeyValue.ToString();

            for (int rowIndex = startRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex);
                if (row == null)
                    continue;

                ICell cell = row.GetCell(pkColumnIndex);
                if (cell == null)
                    continue;

                string cellValue = GetCellValue(cell);
                if (cellValue == pkValueStr)
                    return rowIndex;
            }
            return -1;
        }
        /// <summary>
        /// 根据主键值查找行索引
        /// </summary>
        private static int FindRowIndexByPrimaryKey(ISheet sheet, int pkColumnIndex, object primaryKeyValue)
        {
            if (primaryKeyValue == null)
                return -1;

            string pkValueStr = primaryKeyValue.ToString();

            // 从第1行开始查找（跳过标题行）
            for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex);
                if (row == null)
                    continue;

                ICell cell = row.GetCell(pkColumnIndex);
                if (cell == null)
                    continue;

                string cellValue = GetCellValue(cell);
                if (cellValue == pkValueStr)
                    return rowIndex;
            }

            return -1;
        }

        /// <summary>
        /// 根据列名查找列索引
        /// </summary>
        private static int FindColumnIndex(ISheet sheet, string columnName)
        {
            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null)
                return -1;

            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                ICell cell = headerRow.GetCell(i);
                if (cell != null && GetCellValue(cell).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 根据值的类型设置单元格值
        /// </summary>
        private static void SetCellValue(ICell cell, object value)
        {
            if (value == null)
            {
                cell.SetCellValue(string.Empty);
                return;
            }

            switch (value)
            {
                case string strVal:
                    cell.SetCellValue(strVal);
                    break;
                case int intVal:
                    cell.SetCellValue(intVal);
                    break;
                case double dblVal:
                    cell.SetCellValue(dblVal);
                    break;
                case decimal decVal:
                    cell.SetCellValue((double)decVal);
                    break;
                case bool boolVal:
                    cell.SetCellValue(boolVal);
                    break;
                case DateTime dtVal:
                    cell.SetCellValue(dtVal);
                    break;
                default:
                    cell.SetCellValue(value.ToString());
                    break;
            }
        }

        /// <summary>
        /// 将数据写入sheet中，通过配置的Excel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="configWb"></param>
        /// <param name="configSheetName"></param>
        /// <param name="dataWb"></param>
        /// <param name="dataSheetName"></param>
        public static void WriteSheetByXlsConfig<T>(List<T> datas, SheetCreateRowConfig configSheetConfig, SheetCreateRowConfig dataSheetConfig)
        {
            ISheet configSheet = configSheetConfig.Workbook.GetSheet(configSheetConfig.SheetName);
            ISheet dataSheet = dataSheetConfig.Workbook.GetSheet(dataSheetConfig.SheetName);
            //读取配置文件
            List<CellConfigEntity> cellCofigEntities = ReadCellCofigEntity(configSheet, configSheetConfig.HeaderIndex, dataSheet, dataSheetConfig.HeaderIndex);
           
            for (int i = 0; i < datas.Count; i++)
            {
                Object data = datas[i];
                IRow row = ExcelTool.GetOrCreateRow(dataSheet,dataSheetConfig.HeaderIndex+1+i);
                
                foreach (var cellConfigEntity in cellCofigEntities)
                {
                    int cellIndex = cellConfigEntity.WriteCellIndex;
                    object val =  ValueTool.GetValue(data, cellConfigEntity.CoinfgValue);
                    SetCellValue(GetOrCreateCell(row, cellIndex), val);

                }
            }
        }
        public delegate (List<AttrListT>,String) ListForeach<T,AttrListT>(T t);

        /// <summary>
        /// 将 列表 数据写入sheet中，通过配置的Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="AttrListT"></typeparam>
        /// <param name="datas"></param>
        /// <param name="configSheetConfig"></param>
        /// <param name="dataSheetConfig"></param>
        /// <param name="listForeach"></param>
        public static void WriteSheetByXlsConfig<T, AttrListT>(List<T> datas, SheetCreateRowConfig configSheetConfig, SheetCreateRowConfig dataSheetConfig, ListForeach<T, AttrListT> listForeach)
        {
            ISheet configSheet = configSheetConfig.Workbook.GetSheet(configSheetConfig.SheetName);
            ISheet dataSheet = dataSheetConfig.Workbook.GetSheet(dataSheetConfig.SheetName);
            //读取配置文件
            List<CellConfigEntity> cellCofigEntities = ReadCellCofigEntity(configSheet, configSheetConfig.HeaderIndex, dataSheet, dataSheetConfig.HeaderIndex);
            int index = 0;
            int startRowIndex = dataSheetConfig.HeaderIndex + 1;
            for (int i = 0; i < datas.Count; i++)
            {
                Object data = datas[i];
                (List<AttrListT> items,String setKey )=  listForeach(datas[i]);

                foreach (var item in items)
                {
                    index++;
                    ValueTool.SetValue(data, setKey, item);
                    IRow row = ExcelTool.GetOrCreateRow(dataSheet, startRowIndex);
                    foreach (var cellConfigEntity in cellCofigEntities)
                    {
                        int cellIndex = cellConfigEntity.WriteCellIndex;
                        object val = ValueTool.GetValue(data, cellConfigEntity.CoinfgValue);
                        SetCellValue(GetOrCreateCell(row, cellIndex), val);

                    }
                    startRowIndex++;
                }
                
            }
        }
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="configSheet"></param>
        /// <param name="headerIndex"></param>
        /// <returns></returns>
        private static List<CellConfigEntity> ReadCellCofigEntity(ISheet configSheet, int configHeaderIndex, ISheet dataSheet, int dataheaderIndex)
        {
            List<String> errors = new List<string>();
            Dictionary<String, int> dict = new Dictionary<string, int>();
            {
                IRow row = dataSheet.GetRow(dataheaderIndex);
                if (row == null)
                {
                    throw new ExceptionExcel("数据文件，行没有数据，查看dataheaderIndex是否错误！");
                }
                for (int i = 0; i <= row.LastCellNum; i++)
                {
                    String key = GetCellValue(row, i);
                    if (StringUtils.IsNullOrTrimEmpty(key))
                    {
                        continue;
                    }
                    String updateKey = ExcelTool.HandleKey(key);
                    dict.Add(updateKey, i);
                }
            }
          

            List<CellConfigEntity> cellCofigEntities = new List<CellConfigEntity>();
            for (int i = configHeaderIndex; i <= configSheet.LastRowNum; i++)
            {
                IRow row = configSheet.GetRow(i);
                if(row == null)
                {
                    continue;
                }
                String key = GetCellValue(row, 0);
                String value = GetCellValue(row, 1);
                if(StringUtils.IsNullOrTrimEmpty(key) )
                {
                    continue;
                }
                int writeCellIndex = -1;
                if(!dict.TryGetValue(key,out writeCellIndex))
                {
                    String updateKey = ExcelTool.HandleKey(key);
                    if (!dict.TryGetValue(updateKey, out writeCellIndex))
                    {

                    }
                }
                if(writeCellIndex == -1)
                {
                    errors.Add($"Sheet={configSheet.SheetName},配置文件的key，在数据文件中不存在：{key}");
                }
                cellCofigEntities.Add(new CellConfigEntity()
                {
                    ConfigKey = key,
                    CoinfgValue = value,
                    WriteCellIndex = writeCellIndex
                });
            }
            if(errors.Count > 0)
            {
                FileTool.WriteTxt(AppTool.GetTimeTxt(), errors, true);
            }
            return cellCofigEntities;
        }

        public static string HandleKey(string key)
        {
            String updateKey = key;
            if (updateKey.Contains("(") && updateKey.Contains(")"))
            {

                updateKey = updateKey.Substring(0, updateKey.IndexOf("("));
            }
            else if (updateKey.Contains("（") && updateKey.Contains("）"))
            {
                updateKey = updateKey.Substring(0, updateKey.IndexOf("（"));
            }
            //去除带星号的必填文字
            if (updateKey.Contains("*"))
            {
                updateKey = updateKey.Replace("*", "");
            }
            return updateKey;
        }

        private static IRow GetOrCreateRow(ISheet sheet, int rowIndex)
        {
            IRow row = sheet.GetRow(rowIndex);
            if(row == null)
            {
                row = sheet.CreateRow(rowIndex);
            }
            return row;
        }
        private static ICell GetOrCreateCell(IRow row, int cellIndex)
        {
            ICell cell = row.GetCell(cellIndex);
            if (cell == null)
            {
                cell = row.CreateCell(cellIndex);
            }
            return cell;
        }
    }

}
