using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AlphaYanTools
{
    /// <summary>
    /// ExportPdfImg.xaml 的交互逻辑
    /// </summary>
    public partial class ExportPdfImg : UserControl
    {
        string[] ExcelExtension = new string[] { ".pdf" };
        public ExportPdfImg()
        {
            InitializeComponent();
        }
        private void Border_PreviewDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    string exten = System.IO.Path.GetExtension(file);
                    if (string.IsNullOrEmpty(exten))
                    {
                        MessageBox.Show($"暂时不支持文件夹,等我心情好了再写!!!");
                        return;
                    }
                    string fileName = System.IO.Path.GetFileName(file).Replace(exten, "");
                    if (!ExcelExtension.Contains(System.IO.Path.GetExtension(file)))
                    {
                        MessageBox.Show($"暂时不支持 {exten} 类型文件");
                    }
                    else
                    {
                        PdfDocument doc = new PdfDocument();
                        doc.LoadFromFile(file);

                        List<System.Drawing.Image> listImages = new List<System.Drawing.Image>();

                        for (int i = 0; i < doc.Pages.Count; i++)
                        {
                            // 实例化一个Spire.Pdf.PdfPageBase对象
                            PdfPageBase page = doc.Pages[i];

                            // 获取所有pages里面的图片
                            System.Drawing.Image[] images = page.ExtractImages();
                            if (images != null && images.Length > 0)
                            {
                                listImages.AddRange(images);
                            }
                        }

                        // 将提取到的图片保存到本地路径
                        if (listImages.Count > 0)
                        {
                            for (int i = 0; i < listImages.Count; i++)
                            {
                                System.Drawing.Image image = listImages[i];
                                image.Save($"{DateTime.Now.ToString("yyyyMMdd")}\\{fileName}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.png", ImageFormat.Png);
                            }
                        }
                        //打开图片保存路径
                        Process.Start($"{Directory.GetCurrentDirectory()}\\{DateTime.Now.ToString("yyyyMMdd")}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
        }
    }
}
