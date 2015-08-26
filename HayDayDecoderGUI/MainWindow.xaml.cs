using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HayDayDecoder;

namespace HayDayDecoderGUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
        }

        private void FolderPicker_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fBD = new System.Windows.Forms.FolderBrowserDialog();
            fBD.Description = "选择存储HayDay CSV文件的目录\nSelect the folder which saved HayDay's CSV files.";
            if(fBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FolderPath.Content = fBD.SelectedPath;
                Convert.IsEnabled = true;
            }
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            
            var decoder = new HayDayDecoder.Decoder();
            try
            {
                foreach(var result in decoder.unzipDirectory((string)FolderPath.Content))
                {
                    Result.Text += "\n" + result;
                    ResultScroll.ScrollToBottom();
                }
            }
            catch
            {
                Result.Text += "\n[Error]\tDirectory doesn't exist.";
            }
        }
    }
}
