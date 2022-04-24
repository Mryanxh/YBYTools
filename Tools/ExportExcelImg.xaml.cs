using Spire.Xls;
using System;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AlphaYanTools
{
    /// <summary>
    /// ExportImg.xaml 的交互逻辑
    /// </summary>
    public partial class ExportExcelImg : UserControl
    {
        string[] ExcelExtension = new string[] { ".xlsx", ".xls" };
        public ExportExcelImg()
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
                        Workbook workbook = new Workbook();
                        workbook.LoadFromFile(file);
                        foreach (Worksheet sheet in workbook.Worksheets)
                        {
                            foreach (ExcelPicture picture in sheet.Pictures)
                            {
                                if (null != picture && picture.Picture != null)
                                {
                                    picture.Picture.Save($"{fileName}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.png", ImageFormat.Png);
                                    picture.Picture.Dispose();
                                    picture.Dispose();
                                }
                            }
                            sheet.Dispose();
                        }
                        workbook.Dispose();
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