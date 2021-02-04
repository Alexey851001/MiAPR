using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using PerseptronLib;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TextClassCount.Text = "3";
            TextInstanceCount.Text = "1";
            TextSignCount.Text = "2";
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
            Perseptron perseptron = new Perseptron(Convert.ToInt32(TextClassCount.Text),
                Convert.ToInt32(TextInstanceCount.Text),
                Convert.ToInt32(TextSignCount.Text));
            perseptron.Evaluate(out string classes, out string functions);
            ClassesBox.Text = classes;
            FunctionBox.Text = functions;
        }
    }
}