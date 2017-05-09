using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBViewer.UI.WPF
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class WPFMainForm : Window
    {
        public WPFMainForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            parameterConfig.SaveConfig();
        }

        private void btnRebuildTrigger_Click(object sender, RoutedEventArgs e)
        {
            TableListForm form = new TableListForm();
            form.ShowDialog();
        }
    }
}
