using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using STM;

namespace SubtitleMatcher
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

        private void buttonMatch_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "输出字幕文件|*.srt";
            if (saveDlg.ShowDialog() == true)
            {
                var output = saveDlg.FileName;
                SrtTextMatcher matcher = new SrtTextMatcher(m_SrtFilePath, m_TextFilePath, output);
                MessageBox.Show("成功！");
            }
        }

        string m_SrtFilePath;
        string m_TextFilePath;

        private void buttonSrtFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog =
               new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "字幕文件|*.srt";
            if (dialog.ShowDialog() == true)
            {
                m_SrtFilePath = dialog.FileName;

                labelSrt.Content = m_SrtFilePath;
            }
        }

        private void buttonTextFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog =
               new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "文本文件|*.txt";
            if (dialog.ShowDialog() == true)
            {
                m_TextFilePath = dialog.FileName;

                labelText.Content = m_TextFilePath;
            }
        }
    }
}
