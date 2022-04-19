using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;

namespace Update
{
    internal class Program
    {
        private static string SourceName;
        private static string LocalVersion;
        private static string BaseUrl;

        
        static void Main(string[] args)
        {
            BaseUrl = "http://www.505p.cn/files/";

            Process.Start($"{Directory.GetCurrentDirectory()}\\");
            args = new string[2] { "AlphaYanTools","1.0.0.0" };
            if (args.Length > 0)
            {
                SourceName = args[0];
                LocalVersion = args[1];
                Console.WriteLine($"check update...");
                //下载云端软件目录和版本的校验文件
                DownLoadFile($"{BaseUrl}{SourceName}/Release.info", "Release.info");
                if (File.Exists("Release.info"))
                {
                    string jsonRelease = File.ReadAllText("Release.info");
                    SoftwareVersion ServerFileSource = JsonConvert.DeserializeObject<SoftwareVersion>(jsonRelease);
                    if (NeedUpdate(LocalVersion, ServerFileSource.VersionStr))
                    {
                        Console.WriteLine($"find new version:v{LocalVersion} ->v{ServerFileSource.VersionStr}");
                        string path = Directory.GetCurrentDirectory();
                        List<string> LocalFiles = Directory.GetFiles(path).ToList();
                        List<FileInfo> LocalFileSource = new List<FileInfo>();
                        foreach (string file in LocalFiles)
                        {
                            LocalFileSource.Add(new FileInfo()
                            {
                                Name = Path.GetFileName(file),
                                MD5 = GetMD5WithFilePath(file),
                            });
                        }
                        List<FileInfo> NeedDownLoadFile = new List<FileInfo>();
                        foreach (FileInfo file in ServerFileSource.FileInfo)
                        {
                            //本地缺失的文件
                            FileInfo localfile = LocalFileSource.Find(s => s.Name == file.Name);
                            if (localfile == null)
                            {
                                NeedDownLoadFile.Add(file);
                            }
                            else if (localfile.MD5 != file.MD5)
                            {
                                NeedDownLoadFile.Add(file);
                            }
                        }
                        string newVersionPath = $"{path}\\{ServerFileSource.VersionStr}";
                        Directory.CreateDirectory(newVersionPath);
                        //下载所需要的文件
                        foreach (FileInfo file in NeedDownLoadFile)
                        {
                            Console.WriteLine($"download:{file.Name}-{file.MD5}");
                            DownLoadFile($"{BaseUrl}{SourceName}/{ServerFileSource.VersionStr}/{file.Name}", $"{newVersionPath}\\{file.Name}");
                        }
                        //下载完成后启动指定的Exe文件,并退出当前窗口
                        Process.Start($"{newVersionPath}\\{SourceName}.exe");
                    }
                }
            }
            //发布之前删除
            Console.Read();
        }

        private static bool NeedUpdate(string oldversion, string newversion)
        {
            string[] newV = newversion.Split('.');
            string[] oldV = oldversion.Split('.');
            if (oldV.Length == newV.Length)
            {
                for (int i = 0; i < oldV.Length; i++)
                {
                    if (int.Parse(oldV[i]) > int.Parse(newV[i]))
                        return true;
                }
            }
            return false;
        }
        private static string downloadfilename = string.Empty;
        private static void DownLoadFile(string url,string filename)
        {
            downloadfilename = filename;
            WebClient web = new WebClient();
            web.DownloadProgressChanged += Web_DownloadProgressChanged;
            web.DownloadFile(url, filename);
        }
        private static void Web_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine($"download {downloadfilename}...{e.ProgressPercentage}%");
        }

        static public string GetMD5WithFilePath(string filePath)
        {
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash_byte = md5.ComputeHash(file);
            string str = BitConverter.ToString(hash_byte);
            str = str.Replace("-", "");
            return str;
        }

        /// <summary>
        /// 创建一个快捷方式
        /// </summary>
        /// <param name="lnkFilePath">快捷方式的完全限定路径。</param>
        /// <param name="workDir"></param>
        /// <param name="args">快捷方式启动程序时需要使用的参数。</param>
        /// <param name="targetPath"></param>
        public static void CreateShortcut(string lnkFilePath, string targetPath, string workDir, string args = "")
        {
            var shellType = Type.GetTypeFromProgID("WScript.Shell");
            dynamic shell = Activator.CreateInstance(shellType);
            var shortcut = shell.CreateShortcut(lnkFilePath);
            shortcut.TargetPath = targetPath;
            shortcut.Arguments = args;
            shortcut.WorkingDirectory = workDir;
            shortcut.Save();
        }
    }

    public class SoftwareVersion
    {
        public DateTime Updatetime { get; set; }

        public string UpdateInfo { get; set; }
        public string VersionStr { get; set; }

        public bool MustUpdate { get; set; }

        public string Path { get; set; }

        public List<FileInfo> FileInfo { get; set; }
    }
    public class FileInfo
    {
        public string Name { get; set; }
        public string MD5 { get; set; }
    }
}