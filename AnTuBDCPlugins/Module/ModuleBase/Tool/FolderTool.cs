using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBase.Tool
{
    public static class FolderTool
    {
        /// <summary>
        /// 复制文件夹及其所有内容到目标位置
        /// </summary>
        /// <param name="sourceFolder">源文件夹路径</param>
        /// <param name="destFolder">目标文件夹路径</param>
        /// <param name="overwrite">是否覆盖已存在的文件</param>
        public static void CopyFolder(string sourceFolder, string destFolder, bool overwrite = true)
        {
            try
            {
                // 检查源文件夹是否存在
                if (!System.IO.Directory.Exists(sourceFolder))
                {
                    throw new DirectoryNotFoundException($"源文件夹不存在: {sourceFolder}");
                }

                // 如果目标文件夹不存在，则创建
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }

                // 获取源文件夹中的所有文件
                string[] files = Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string destFile = Path.Combine(destFolder, fileName);
                    File.Copy(file, destFile, overwrite);
                }

                // 递归复制子文件夹
                string[] folders = Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string folderName = Path.GetFileName(folder);
                    string destSubFolder = Path.Combine(destFolder, folderName);
                    CopyFolder(folder, destSubFolder, overwrite);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"复制文件夹时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 删除文件夹及其所有内容
        /// </summary>
        /// <param name="folderPath">要删除的文件夹路径</param>
        public static void DeleteFolder(string folderPath)
        {
            try
            {
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"删除文件夹时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 创建文件夹（如果不存在）
        /// </summary>
        /// <param name="folderPath">要创建的文件夹路径</param>
        public static void CreateFolder(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"创建文件夹时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取文件夹大小（字节）
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <returns>文件夹大小（字节）</returns>
        public static long GetFolderSize(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    return 0;
                }

                long size = 0;
                string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    size += fi.Length;
                }
                return size;
            }
            catch (Exception ex)
            {
                throw new Exception($"计算文件夹大小时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取文件夹中的文件数量
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="includeSubfolders">是否包含子文件夹</param>
        /// <returns>文件数量</returns>
        public static int GetFileCount(string folderPath, bool includeSubfolders = true)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    return 0;
                }

                SearchOption option = includeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                return Directory.GetFiles(folderPath, "*.*", option).Length;
            }
            catch (Exception ex)
            {
                throw new Exception($"获取文件数量时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 移动文件夹到新位置
        /// </summary>
        /// <param name="sourceFolder">源文件夹路径</param>
        /// <param name="destFolder">目标文件夹路径</param>
        public static void MoveFolder(string sourceFolder, string destFolder)
        {
            try
            {
                if (!Directory.Exists(sourceFolder))
                {
                    throw new DirectoryNotFoundException($"源文件夹不存在: {sourceFolder}");
                }

                if (Directory.Exists(destFolder))
                {
                    throw new IOException($"目标文件夹已存在: {destFolder}");
                }

                Directory.Move(sourceFolder, destFolder);
            }
            catch (Exception ex)
            {
                throw new Exception($"移动文件夹时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 清空文件夹内容（保留文件夹本身）
        /// </summary>
        /// <param name="folderPath">要清空的文件夹路径</param>
        public static void EmptyFolder(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    return;
                }

                // 删除所有文件
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                }

                // 删除所有子文件夹
                string[] folders = Directory.GetDirectories(folderPath);
                foreach (string folder in folders)
                {
                    DeleteFolder(folder);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"清空文件夹时出错: {ex.Message}", ex);
            }
        }
    }
}
