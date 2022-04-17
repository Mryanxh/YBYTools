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
        static string[] IgnoreFile = new string[] {".pdb", ".xml" };
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();
            List<string> files = Directory.GetFiles(path).ToList();
            List<string> UpdateFiles = new List<string>();
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
            
            List<FileInfo> UpdateInfo = new List<FileInfo>();
            foreach (string file in UpdateFiles)
            {
                UpdateInfo.Add(new FileInfo()
                {
                    Name = Path.GetFileName(file),
                    MD5 = GetMD5WithFilePath(file),
                });
            }
            string UpdateString = JsonConvert.SerializeObject(UpdateInfo, Formatting.Indented);
            File.WriteAllText($"{path}\\Release.info", UpdateString, Encoding.UTF8);
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