using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SystemModule.Core.IO
{
    /// <summary>
    /// 文件操作
    /// </summary>
    [IntelligentCoder.AsyncMethodPoster(Flags = IntelligentCoder.MemberFlags.Public)]
    public static partial class FileUtility
    {
        /// <summary>
        /// 获取不重复文件名。
        /// <para>例如：New.txt已存在时，会返回New(1).txt</para>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetDuplicateFileName(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return fileName;
            }

            int index = 0;
            while (true)
            {
                index++;
                string newPath = Path.Combine(Path.GetDirectoryName(fileName), $"{Path.GetFileNameWithoutExtension(fileName)}({index}){Path.GetExtension(fileName)}");
                if (!File.Exists(newPath))
                {
                    return newPath;
                }
            }
        }

        /// <summary>
        /// 获取不重复文件夹名称.
        /// <para>例如：NewDir已存在时，会返回NewDir(1)</para>
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public static string GetDuplicateDirectoryName(string dirName)
        {
            if (!Directory.Exists(dirName))
            {
                return dirName;
            }

            int index = 0;
            while (true)
            {
                index++;
                string newPath = Path.Combine(Path.GetDirectoryName(dirName), $"{Path.GetFileNameWithoutExtension(dirName)}({index})");
                if (!System.IO.Directory.Exists(newPath))
                {
                    return newPath;
                }
            }
        }

        /// <summary>
        /// 转化为文件大小的字符串，类似10B，10Kb，10Mb，10Gb。
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ToFileLengthString(long length)
        {
            if (length < 1024)
            {
                return $"{length}B";
            }
            else if (length < 1024 * 1024)
            {
                return $"{length / 1024.0:0.00}Kb";
            }
            else if (length < 1024 * 1024 * 1024)
            {
                return $"{length / (1024.0 * 1024):0.00}Mb";
            }
            else
            {
                return $"{length / (1024.0 * 1024 * 1024):0.00}Gb";
            }
        }

        /// <summary>
        /// 获取文件MD5
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileMD5(string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return GetStreamMD5(fileStream);
            }
        }

        /// <summary>
        /// 获取流MD5
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static string GetStreamMD5(Stream fileStream)
        {
            using (HashAlgorithm hash = System.Security.Cryptography.MD5.Create())
            {
                return GetStreamHash(fileStream, hash);
            }
        }

        /// <summary>
        /// 获得文件Hash值
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GetFileHash256(string filePath)
        {
            try
            {
                HashAlgorithm hash = SHA256.Create();
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    byte[] HashValue = hash.ComputeHash(fileStream);
                    return BitConverter.ToString(HashValue).Replace("-", "");
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获得流Hash值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetStreamHash256(Stream stream)
        {
            try
            {
                HashAlgorithm hash = SHA256.Create();
                byte[] HashValue = hash.ComputeHash(stream);
                return BitConverter.ToString(HashValue).Replace("-", "");
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获得文件Hash值
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static string GetFileHash(string filePath, HashAlgorithm hash)
        {
            try
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    byte[] HashValue = hash.ComputeHash(fileStream);
                    return BitConverter.ToString(HashValue).Replace("-", "");
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获得流Hash值
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static string GetStreamHash(Stream stream, HashAlgorithm hash)
        {
            try
            {
                byte[] HashValue = hash.ComputeHash(stream);
                return BitConverter.ToString(HashValue).Replace("-", "");
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取仅当前文件夹中包含的文件名称，不含全路径。
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static string[] GetIncludeFileNames(string dirPath)
        {
            return Directory.GetFiles(dirPath).Select(s => Path.GetFileName(s)).ToArray();
        }

        /// <summary>
        /// 获取相对路径。
        /// </summary>
        /// <param name="relativeTo"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetRelativePath(string relativeTo, string path)
        {
            if (string.IsNullOrEmpty(relativeTo))
            {
                throw new ArgumentNullException(nameof(relativeTo));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            Uri fromUri = new Uri(relativeTo);
            Uri toUri = new Uri(path);

            if (fromUri.Scheme != toUri.Scheme)
            {
                // 不是同一种路径，无法转换成相对路径。
                return path;
            }

            if (fromUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase)
                && !relativeTo.EndsWith("/", StringComparison.OrdinalIgnoreCase)
                && !relativeTo.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                // 如果是文件系统，则视来源路径为文件夹。
                fromUri = new Uri(relativeTo + Path.DirectorySeparatorChar);
            }

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        /// <summary>
        /// 删除路径文件
        /// </summary>
        /// <param name="path"></param>
        public static void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
                File.Delete(path);
            }
        }
    }
}