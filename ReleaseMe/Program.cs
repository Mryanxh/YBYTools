using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;

namespace ReleaseMe
{
    internal class Program
    {
        //忽略的文件类型
        static string[] IgnoreFile = new string[] {".pdb", ".xml" };
        
        static void Main(string[] args)
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                List<string> files = Directory.GetFiles(path).ToList();
                List<string> UpdateFiles = new List<string>();
                //生成MD数据
                foreach (string file in files)
                {
                    if (IgnoreFile.Contains(Path.GetExtension(file)))
                    {
                        continue;
                    }
                    else
                    {
                        UpdateFiles.Add(file);
                    }
                }
                //生成所有文件信息
                List<FileInfo> UpdateInfo = new List<FileInfo>();
                foreach (string file in UpdateFiles)
                {
                    UpdateInfo.Add(new FileInfo()
                    {
                        Name = Path.GetFileName(file),
                        MD5 = GetMD5WithFilePath(file),
                    });
                }
                Console.Write("请输入更新日志:");
                string VersionUpdateInfo = Console.ReadLine();

                SoftwareVersion newVersionInfo = new SoftwareVersion()
                {
                    FileInfo = UpdateInfo,
                    MustUpdate = true,
                    Updatetime = DateTime.Now,
                    UpdateInfo = VersionUpdateInfo,
                    VersionStr = "1.0.0.1"
                };

                //保存成本地文件
                string newVersionStr = JsonConvert.SerializeObject(newVersionInfo, Formatting.Indented);
                File.WriteAllText($"{path}\\Release.info", newVersionStr, Encoding.UTF8);

                Console.WriteLine("文件生成成功,请及时上传至服务器！");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.Read();
            }
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

    public class SoftwareVersion
    {
        public DateTime Updatetime { get; set; }

        public string VersionStr { get; set; }

        public string UpdateInfo { get; set; }

        public bool MustUpdate { get; set; }

        public List<FileInfo> FileInfo { get; set; }
    }
    public class FileInfo
    {
        public string Name { get; set; }
        public string MD5 { get; set; }
    }
}