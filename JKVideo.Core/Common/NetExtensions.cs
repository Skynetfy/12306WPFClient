using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JKVideo.Core.Common
{
    /// <summary>
    /// 扩展方法 - 工具类
    /// </summary>
    public static class NetExtensions
    {
        /// <summary>
        /// json 2 object
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="jsonString">json源</param>
        /// <returns>对象</returns>
        public static T DeserializeObject<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// Unicode转中文
        /// </summary>
        /// <param name="source">源</param>
        /// <returns>中文</returns>
        public static string Unicode2String(this string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                         source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        /// <summary>
        /// 字符串是否符合url地址
        /// </summary>
        /// <param name="source">源</param>
        /// <returns>true:是，false:否</returns>
        public static bool IsUrl(this string source)
        {
            if (string.IsNullOrEmpty(source)) return false;
            return Regex.IsMatch(source,
                @"^((http|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?$");
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="content">内容</param>
        /// <param name="append">是否追加</param>
        public static void WriteFile(string filePath, string content, bool append = false)
        {
            string folder = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            using (FileStream fileStream = new FileStream(filePath, append ? FileMode.Append : FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    streamWriter.WriteLine(content);
                    streamWriter.Flush();
                    streamWriter.Close();
                    fileStream.Close();
                }
            }
        }

        /// <summary>
        /// 写文件 - 流
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="stream">流</param>
        /// <param name="progressAction">进度 action</param>
        public static void WriteFile(string filePath, Stream stream, Action<long> progressAction = null)
        {
            string folderPath = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(folderPath) && !Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                const int size = 1024 * 1024 * 1;

                MemoryStream ms = new MemoryStream();
                byte[] buffer = new byte[size];
                while (true)
                {
                    int sz = stream.Read(buffer, 0, size);
                    if (sz == 0) break;
                    ms.Write(buffer, 0, sz);
                }
                ms.Position = 0;
                stream = ms;

                byte[] cacheBytes = new byte[size];
                int result = stream.Read(cacheBytes, 0, size);

                long position = 0,
                    totalSize = stream.Length;
                position += result;
                if (progressAction != null)
                {
                    progressAction(position / totalSize);
                }

                while (result > 0)
                {
                    fileStream.Write(cacheBytes, 0, size);
                    result = stream.Read(cacheBytes, 0, size);

                    position += result;
                    if (progressAction != null)
                    {
                        progressAction(position / totalSize);
                    }
                }

                fileStream.Close();
            }

        }

        /// <summary>
        /// 对象二进制序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filePath">文件路径</param>
        public static void BinarySerialize(this object obj, string filePath)
        {
            string folder = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, obj);
            }
        }

        /// <summary>
        /// 对象二进制反序列化
        /// </summary>
        /// <typeparam name="T">结果类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <returns>对象</returns>
        public static T BinaryDeserialize<T>(string filePath) where T : new()
        {
            T result;
            if (!File.Exists(filePath)) { result = new T(); }
            else
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    result = (T)formatter.Deserialize(fileStream);
                }
            }
            return result;
        }
    }
}
