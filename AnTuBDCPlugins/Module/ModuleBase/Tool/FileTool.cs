
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuleBase.Tool
{

    // <summary>
    /// 自然字符串比较器（模拟Windows资源管理器的排序）
    /// </summary>
    public static class NaturalStringComparer
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string psz1, string psz2);

        public static int Compare(string x, string y)
        {
            return StrCmpLogicalW(x, y);
        }
    }


    public class FileTool
    {
        public static void ReadFolder(string rootFolderPath, List<(string FolderName, string FileName, long FileSize, string FileType)> fileData)
        {
            try
            {

                foreach (var directory in Directory.GetDirectories(rootFolderPath))
                {
                    ReadFolder(directory, fileData);
                }

                foreach (var file in Directory.GetFiles(rootFolderPath))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    string fileType = fileInfo.Extension;
                    if (fileType.Length > 1)
                        fileType = fileType.Substring(1);

                    fileData.Add((file, Path.GetFileName(file), fileInfo.Length, fileType));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading folder: {ex.Message}");
            }
        }

        public static void WriteTxt(string path, List<string> msgs, bool isOpen = false, bool isMergeError = false)
        {
            if (msgs == null || msgs.Count == 0)
            {
                return;
            }

            if (!Directory.Exists(System.IO.Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            }
            using (StreamWriter writer = new StreamWriter(path))
            {
                if (isMergeError)
                {
                    //统计错误
                    Dictionary<String, int> errorCount = new Dictionary<string, int>();
                    foreach (var item in msgs)
                    {

                        if (errorCount.TryGetValue(item, out int count))
                        {
                            errorCount[item] = count + 1;
                        }
                        else
                        {
                            errorCount[item] = 1;
                        }
                    }
                    foreach (var item in errorCount.Keys)
                    {
                        if (errorCount[item] > 1)
                        {
                            writer.WriteLine(item + $", 共 {errorCount[item]} 条记录");
                        }
                        else
                        {
                            writer.WriteLine(item);
                        }

                    }
                }
                else
                {
                    foreach (var item in msgs)
                    {
                        writer.WriteLine(item);
                    }
                }

                writer.Flush();
            }

            if (isOpen)
            {
                try
                {
                    Process.Start(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"windows 无法开,错误原因，{ex.Message}，需自行打开，文件路径：" + path);
                }

            }
        }

        public static void WriteTxt(string path, Dictionary<string, string> dictionary, bool isOpen = false, bool isMergeError = false)
        {
            List<String> msgs = new List<string>();
            foreach (var item in dictionary)
            {
                msgs.Add($"{item.Key}::{item.Value}");
            }
            WriteTxt(path, msgs);
        }

        /// <summary>
        /// 递归查找并获取指定文件夹下所有的TIF文件路径
        /// </summary>
        /// <param name="folderPath">起始搜索的文件夹路径</param>
        /// <returns>包含所有TIF文件路径的集合</returns>
        public static List<string> FindAllFiles(string rootDir, string filter)// "*.tif"
        {
            /* var files = new List<string>();

             if (!Directory.Exists(rootDir))
             {
                 throw new DirectoryNotFoundException($"指定的目录 '{rootDir}' 不存在。");
             }

             try
             {
                 // 遍历当前文件夹及其子文件夹
                 foreach (string file in FileTool.FindAllFiles(rootDir,filter))
                 {
                     files.Add(file);
                 }
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"在搜索过程中发生错误: {ex.Message}");
             }

             return files;*/
            return GetAllFilesInOrder(rootDir, filter);
        }


        /// <summary>
        /// 递归获取文件夹所有文件，保持与Windows资源管理器相同的显示顺序
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="filter">文件过滤器，例如 "*.txt", "*.jpg", "*.*"</param>
        /// <returns>按自然顺序排序的文件路径列表</returns>
        public static List<string> GetAllFilesInOrder(string folderPath, string filter = "*.*")
        {
            var files = new List<string>();

            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"目录不存在: {folderPath}");
            }

            // 验证filter参数
            if (string.IsNullOrWhiteSpace(filter))
            {
                filter = "*.*";
            }

            try
            {
                // 使用栈进行迭代而非递归，避免栈溢出
                var stack = new Stack<string>();
                stack.Push(folderPath);

                while (stack.Count > 0)
                {
                    string currentPath = stack.Pop();

                    try
                    {
                        // 获取当前目录下的子目录（按名称排序）
                        string[] subDirectories = Directory.GetDirectories(currentPath);
                        Array.Sort(subDirectories, NaturalStringComparer.Compare);

                        // 将子目录逆序压入栈，保证正序处理
                        for (int i = subDirectories.Length - 1; i >= 0; i--)
                        {
                            stack.Push(subDirectories[i]);
                        }

                        // 获取当前目录下的文件（按名称排序，应用过滤器）
                        string[] currentFiles = Directory.GetFiles(currentPath, filter);
                        Array.Sort(currentFiles, NaturalStringComparer.Compare);

                        files.AddRange(currentFiles);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // 跳过无权限访问的目录
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"获取文件列表时出错: {ex.Message}", ex);
            }

            return files;
        }



        /// <summary>
        /// 递归获取文件夹所有文件，支持多个过滤器
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="filters">多个文件过滤器，例如 ["*.txt", "*.jpg", "*.png"]</param>
        /// <returns>按自然顺序排序的文件路径列表</returns>
        public static List<string> GetAllFilesInOrder(string folderPath, string[] filters)
        {
            if (filters == null || filters.Length == 0)
            {
                return GetAllFilesInOrder(folderPath, "*.*");
            }

            var allFiles = new List<string>();

            foreach (string filter in filters)
            {
                var filteredFiles = GetAllFilesInOrder(folderPath, filter);
                allFiles.AddRange(filteredFiles);
            }

            // 对最终结果进行排序（因为不同过滤器的文件是分开获取的）
            allFiles.Sort(NaturalStringComparer.Compare);

            return allFiles;
        }

        /// <summary>
        /// 递归获取文件夹所有文件，支持自定义过滤函数
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="filterFunc">自定义过滤函数，返回true表示包含该文件</param>
        /// <returns>按自然顺序排序的文件路径列表</returns>
        public static List<string> GetAllFilesInOrder(string folderPath, Func<string, bool> filterFunc)
        {
            var files = new List<string>();

            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"目录不存在: {folderPath}");
            }

            if (filterFunc == null)
            {
                return GetAllFilesInOrder(folderPath, "*.*");
            }

            try
            {
                var stack = new Stack<string>();
                stack.Push(folderPath);

                while (stack.Count > 0)
                {
                    string currentPath = stack.Pop();

                    try
                    {
                        // 获取当前目录下的子目录（按名称排序）
                        string[] subDirectories = Directory.GetDirectories(currentPath);
                        Array.Sort(subDirectories, NaturalStringComparer.Compare);

                        // 将子目录逆序压入栈，保证正序处理
                        for (int i = subDirectories.Length - 1; i >= 0; i--)
                        {
                            stack.Push(subDirectories[i]);
                        }

                        // 获取当前目录下的所有文件
                        string[] currentFiles = Directory.GetFiles(currentPath, "*.*");
                        Array.Sort(currentFiles, NaturalStringComparer.Compare);

                        // 应用自定义过滤函数
                        foreach (string file in currentFiles)
                        {
                            if (filterFunc(file))
                            {
                                files.Add(file);
                            }
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // 跳过无权限访问的目录
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"获取文件列表时出错: {ex.Message}", ex);
            }

            return files;
        }
        /// <summary>
        /// 找到所有的文件夹和文件（优化版）
        /// </summary>
        /// <param name="rootDir">根目录</param>
        /// <param name="filter">文件过滤"*.tif"</param>
        /// <returns>所有文件和文件夹路径</returns>
        public static List<string> FindAllFilesAndDirs(string rootDir, string filter)
        {
            var results = new List<string>();

            if (!Directory.Exists(rootDir))
            {
                throw new DirectoryNotFoundException($"指定的目录 '{rootDir}' 不存在。");
            }

            // 使用栈来避免递归，提高性能
            var stack = new Stack<string>();
            stack.Push(rootDir);
            results.Add(rootDir);

            while (stack.Count > 0)
            {
                string currentDir = stack.Pop();

                try
                {
                    // 获取当前目录的子目录
                    foreach (string subDir in Directory.GetDirectories(currentDir))
                    {
                        stack.Push(subDir);
                        results.Add(subDir);
                    }

                    // 获取当前目录中匹配过滤条件的文件
                    foreach (string file in Directory.GetFiles(currentDir, filter))
                    {
                        results.Add(file);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // 跳过没有权限的目录
                    Console.WriteLine($"没有权限访问目录: {currentDir}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"处理目录 {currentDir} 时发生错误: {ex.Message}");
                }
            }

            return results;
        }

        /// <summary>
        /// "Shapefile (*.shp)|*.shp";
        /// </summary>
        /// <param name="title"></param>
        /// <param name="filter"></param>
        /// <returns></returns>

        public static string ShowSaveDialog(String title, String filter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // 设置对话框属性
            saveFileDialog.Filter = filter;
            saveFileDialog.FilterIndex = 1; // 默认选择.shp格式
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = title;
            saveFileDialog.AddExtension = true;

            // 显示对话框并返回结果
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog.FileName;
            }

            return null;
        }






        /// <summary>
        /// 递归复制整个目录（包括子目录和文件）
        /// </summary>
        /// <param name="sourceDir">源目录路径</param>
        /// <param name="destinationDir">目标目录路径</param>
        /// <param name="overwrite">是否覆盖已存在的文件（默认true）</param>
        /// <returns>成功返回true，失败返回false</returns>
        public static bool CopyDirectory(string sourceDir, string destinationDir, bool overwrite = true)
        {
            try
            {
                var dir = new DirectoryInfo(sourceDir);

                // 检查源目录是否存在
                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException($"找不到源目录: {dir.FullName}");
                }

                // 预先获取所有子目录
                DirectoryInfo[] dirs = dir.GetDirectories();

                // 创建目标目录
                Directory.CreateDirectory(destinationDir);

                // 复制所有文件
                foreach (FileInfo file in dir.GetFiles())
                {
                    string targetFilePath = Path.Combine(destinationDir, file.Name);
                    file.CopyTo(targetFilePath, overwrite);
                }

                // 递归复制子目录
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, overwrite);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"目录复制失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 批量复制多个文件到指定目录
        /// </summary>
        /// <param name="sourceFilePaths">源文件路径集合</param>
        /// <param name="destinationDirectory">目标目录路径</param>
        /// <param name="overwrite">是否覆盖已存在的文件（默认true）</param>
        /// <returns>成功复制的文件数量</returns>
        public static int CopyFiles(IEnumerable<string> sourceFilePaths, string destinationDirectory, bool overwrite = true)
        {
            int successCount = 0;

            // 确保目标目录存在
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            foreach (var srcPath in sourceFilePaths)
            {
                if (!File.Exists(srcPath)) continue;

                try
                {
                    string destPath = Path.Combine(destinationDirectory, Path.GetFileName(srcPath));
                    File.Copy(srcPath, destPath, overwrite);
                    successCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"文件 {srcPath} 复制失败: {ex.Message}");
                }
            }

            return successCount;
        }






        public static List<string> GetAllDir(string rootDir)
        {
            List<string> allDirs = new List<string>();

            try
            {
                // 首先添加根目录
                allDirs.Add(rootDir);

                // 获取当前目录下的所有子目录
                string[] subDirs = Directory.GetDirectories(rootDir);

                // 递归处理每个子目录
                foreach (string dir in subDirs)
                {
                    allDirs.AddRange(GetAllDir(dir)); // 递归调用
                }
            }
            catch (UnauthorizedAccessException)
            {
                // 处理无权限访问的目录
                Console.WriteLine($"无权限访问目录: {rootDir}");
            }
            catch (DirectoryNotFoundException)
            {
                // 处理目录不存在的情况
                Console.WriteLine($"目录不存在: {rootDir}");
            }
            catch (Exception ex)
            {
                // 处理其他异常
                Console.WriteLine($"访问目录 {rootDir} 时出错: {ex.Message}");
            }

            return allDirs;
        }

        public static void CopyDir(string srcDir, string descDir)
        {
            // Check if the source directory exists
            if (!Directory.Exists(srcDir))
            {
                throw new DirectoryNotFoundException($"Source directory not found: {srcDir}");
            }

            // Create the destination directory if it doesn't exist
            if (!Directory.Exists(descDir))
            {
                Directory.CreateDirectory(descDir);
            }

            // Get all files in the source directory
            var files = Directory.GetFiles(srcDir);

            // Copy each file to the destination directory
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(descDir, fileName);
                File.Copy(file, destFile, true); // true means overwrite if already exists
            }

            // Get all subdirectories in the source directory
            var subDirs = Directory.GetDirectories(srcDir);

            // Recursively copy each subdirectory
            foreach (string subDir in subDirs)
            {
                string dirName = Path.GetFileName(subDir);
                string destSubDir = Path.Combine(descDir, dirName);
                CopyDir(subDir, destSubDir); // Recursive call
            }
        }

        public static void CopyFile(string srcFilename, string destFileName, bool isOvrrid = true)
        {
            String dir = Path.GetDirectoryName(destFileName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!isOvrrid)
            {
                if (File.Exists(destFileName))
                {
                    return;
                }
            }
            File.Copy(srcFilename, destFileName, isOvrrid);

        }

        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="srcDirName">源文件夹路径</param>
        /// <param name="destDirName">目标文件夹路径</param>
        /// <param name="isOverride">是否覆盖已存在的文件夹</param>
        public static void MoveDir(string srcDirName, string destDirName, bool isOverride)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(srcDirName))
            {
                throw new ArgumentException("源文件夹名不能为空", nameof(srcDirName));
            }

            if (string.IsNullOrWhiteSpace(destDirName))
            {
                throw new ArgumentException("目标文件夹名不能为空", nameof(destDirName));
            }

            // 检查源文件夹是否存在
            if (!Directory.Exists(srcDirName))
            {
                throw new DirectoryNotFoundException($"源文件夹不存在: {srcDirName}");
            }

            // 如果源和目标相同，直接返回
            if (Path.GetFullPath(srcDirName).Equals(Path.GetFullPath(destDirName), StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            try
            {
                // 处理目标文件夹已存在的情况
                if (Directory.Exists(destDirName))
                {
                    if (isOverride)
                    {
                        // 覆盖模式：删除目标文件夹及其所有内容
                        Directory.Delete(destDirName, true);
                    }
                    else
                    {
                        // 不覆盖，抛出异常
                        throw new IOException($"目标文件夹已存在: {destDirName}");
                    }
                }

                // 确保目标目录的父目录存在
                string parentDir = Path.GetDirectoryName(destDirName);
                if (!string.IsNullOrEmpty(parentDir) && !Directory.Exists(parentDir))
                {
                    Directory.CreateDirectory(parentDir);
                }

                // 执行文件夹移动
                Directory.Move(srcDirName, destDirName);
            }
            catch (IOException ex)
            {
                throw new IOException($"移动文件夹时发生IO错误: {ex.Message}", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException($"没有权限移动文件夹: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"移动文件夹时发生未知错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 文件移动
        /// </summary>
        /// <param name="srcFileName">源文件路径</param>
        /// <param name="destFileName">目标文件路径</param>
        /// <param name="isOverride">是否覆盖已存在的文件</param>
        public static void MoveFile(string srcFileName, string destFileName, bool isOverride)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(srcFileName))
            {
                throw new ArgumentException("源文件名不能为空", nameof(srcFileName));
            }

            if (string.IsNullOrWhiteSpace(destFileName))
            {
                throw new ArgumentException("目标文件名不能为空", nameof(destFileName));
            }

            // 检查源文件是否存在
            if (!File.Exists(srcFileName))
            {
                throw new FileNotFoundException($"源文件不存在: {srcFileName}", srcFileName);
            }

            try
            {
                // 确保目标目录存在
                string destDirectory = Path.GetDirectoryName(destFileName);
                if (!string.IsNullOrEmpty(destDirectory) && !Directory.Exists(destDirectory))
                {
                    Directory.CreateDirectory(destDirectory);
                }

                // 处理目标文件已存在的情况
                if (File.Exists(destFileName))
                {
                    if (isOverride)
                    {
                        // 删除已存在的文件
                        File.Delete(destFileName);
                    }
                    else
                    {
                        // 不覆盖，抛出异常或生成新文件名
                        throw new IOException($"目标文件已存在: {destFileName}");
                    }
                }

                // 执行文件移动
                File.Move(srcFileName, destFileName);
            }
            catch (IOException ex)
            {
                // 重新抛出IOException，包含更多信息
                throw new IOException($"移动文件时发生IO错误: {ex.Message}", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException($"没有权限移动文件: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"移动文件时发生未知错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dir">要删除的文件夹路径</param>
        /// <returns>删除是否成功</returns>
        public static bool DeleteDir(string dir)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(dir))
            {
                return true;
            }

            // 如果文件夹不存在，返回true（认为删除成功）
            if (!Directory.Exists(dir))
            {
                return true;
            }

            try
            {
                // 删除文件夹及其所有内容
                Directory.Delete(dir, true);
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"没有权限删除文件夹 '{dir}': {ex.Message}");
                return false;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"删除文件夹 '{dir}' 时发生IO错误: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"删除文件夹 '{dir}' 时发生未知错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">要删除的文件路径</param>
        /// <returns>删除是否成功</returns>
        public static bool DeleteFile(string fileName)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return true;
            }

            // 如果文件不存在，返回true（认为删除成功）
            if (!File.Exists(fileName))
            {
                return true;
            }

            try
            {
                // 删除文件
                File.Delete(fileName);
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"没有权限删除文件 '{fileName}': {ex.Message}");
                return false;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"删除文件 '{fileName}' 时发生IO错误: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"删除文件 '{fileName}' 时发生未知错误: {ex.Message}");
                return false;
            }
        }
    }
}
