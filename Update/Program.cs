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
        private static string VersionStr;
        private static string BaseUrl;

        
        static void Main(string[] args)
        {
            BaseUrl = "http://www.505p.cn/files/";
#if DEBUG
            args = new string[2] { "AlphaYanTools","1.0.0.0" };
#endif
            if (args.Length > 0)
            {
                SourceName = args[0];
                VersionStr = args[1];
                Console.WriteLine($"new version:{SourceName} v{VersionStr}");
                Console.WriteLine($"check files...");
                //下载云端软件目录和版本的校验文件
                DownLoadFile($"{BaseUrl}{SourceName}/{VersionStr}/Release.info", "Release.info");
                if (File.Exists("Release.info"))
                {
                    string jsonRelease = File.ReadAllText("Release.info");
                    List<FileInfo> ServerFileSource = JsonConvert.DeserializeObject<List<FileInfo>>(jsonRelease);

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
                    foreach (FileInfo file in ServerFileSource)
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

                    //下载所需要的文件
                    foreach (FileInfo file in NeedDownLoadFile)
                    {
                        Console.WriteLine($"{file.Name}-{file.MD5}");
                        DownLoadFile($"{BaseUrl}{SourceName}/{VersionStr}/{file.Name}", file.Name);
                    }
                    //下载完成后启动指定的Exe文件,并退出当前窗口
                    //Process.Start($"{Directory.GetCurrentDirectory()}\\{SourceName}.exe");
                }
            }
            //发布之前删除
            Console.Read();
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
    }
    class FileInfo
    {
        public string Name { get; set; }
        public string MD5 { get; set; }
    }
}