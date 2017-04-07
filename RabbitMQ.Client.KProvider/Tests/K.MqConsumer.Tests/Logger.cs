using System;
using System.Diagnostics;
using System.IO;

namespace K.MqConsumer.Tests
{
    internal static class Logger
    {
        private static readonly object sw_lock = new object();
        private static string folderPath = System.Threading.Thread.GetDomain().BaseDirectory + "\\Log";
        //public static string FolderPath { get { return folderPath; } set { if (value.EndsWith("\\") || value.EndsWith("/"))                    value = value.Remove(value.Length - 1, 1); folderPath = value; } }

        #region Utilities
        /// <summary>
        /// 直接写入日记
        /// </summary>
        /// <param name="logFilePath">路径</param>
        /// <param name="content">写入内容</param>
        public static void Log(string logFilePath, string content, params object[] args)
        {
            try
            {
                if (args.Length > 0)
                    content = String.Format(content, args);
                content = String.Format("---Log Time：{0} ---\r\n {1} \r\n", DateTime.Now.ToString("yyyy/MM/dd H:mm:ss "), content);

                lock (sw_lock)
                {
                    if (!Directory.Exists(Path.GetDirectoryName(logFilePath)))                    //判断路径是否存在
                        Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));            //不存在则创建

                    using (StreamWriter sw = File.AppendText(logFilePath))
                        sw.Write(content);
                }
            }
            catch (Exception e)
            {
                throw new IOException("Logger.Log error", e);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 默认路径  程序集:\\Log\\Log_{DateTime}.txt
        /// </summary>
        /// <param name="content">写入内容</param>
        public static void ErrorLog(string content, params object[] args)
        {
            string path = string.Format("{1}\\Log_Error_{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"), folderPath);
            Log(path, content, args);
        }

        public static void InfoLog(string content, params object[] args)
        {
            string path = string.Format("{1}\\Log_Information_{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"), folderPath);
            Log(path, content, args);
        }
        #endregion

        #region Debug Methods
        /// <summary>
        /// 测试用记录文件 默认路径 程序集:\\Log\\Log_Test_{DateTime}.txt
        /// </summary>
        /// <param name="content">写入内容</param>
        /// <returns></returns>
        [Conditional("DEBUG")]
        public static void DebugLog(string content, params object[] args)
        {
            string path = string.Format("{1}\\Log_Debug_{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"), folderPath);
            Log(path, content, args);
        }

        [Conditional("DEBUG")]
        public static void IfDebugLog(bool condition, string content, params object[] args)
        {
            if (!condition) return;

            DebugLog(content, args);
        }
        #endregion
    }
}
