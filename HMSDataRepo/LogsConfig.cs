using System;
using System.Collections.Generic;
using System.Linq;

namespace HMSDataRepo
{
    public class LogsConfig
    {
        private System.IO.FileInfo _CSFile;
        private string DirPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Logs";
        LogsConfig()
        {
            Init();
        }
        public static LogsConfig Instance { get; } = new LogsConfig();
        void Init()
        {
            System.IO.Directory.CreateDirectory(DirPath);
            string fileName = null;
            var files = System.IO.Directory.EnumerateFiles(DirPath).Select(j => new System.IO.FileInfo(j));
            if (files?.Count() > 0)
            {
                files.OrderBy(k => k.Name);
                fileName = DirPath + "//"+(int.Parse(System.IO.Path.GetFileNameWithoutExtension(files.Last().FullName)) + 1).ToString() + ".log";
            }
            else
            {
                fileName = DirPath + "//0.log";
            }
            System.IO.File.Create(fileName, 1024, System.IO.FileOptions.RandomAccess).Close();
            _CSFile = new System.IO.FileInfo(fileName);
        }
        public void Log(Exception e)
        {
            List<string> lines = new List<string>();
            lines.Add("\n-----------Error--------------");
            lines.Add("\nException: " + e.GetType().Name);
            lines.Add("Time: " + DateTime.Now.ToUniversalTime());
            lines.Add("Message: " + e.Message);
            lines.Add("Details: " + e.ToString());
            Write(lines);
        }
        public void Log(string e, string header)
        {
            List<string> lines = new List<string>();
            lines.Add("\n-----------" + e + "--------------");
            lines.Add("\nInfo : " + header);
            lines.Add("Time: " + DateTime.Now.ToUniversalTime());
            Write(lines);
        }
        private void Write(IEnumerable<string> l)
        {
            System.IO.File.AppendAllLines(_CSFile.FullName, l);
        } 
    }
}