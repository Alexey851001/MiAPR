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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TextPC1.Text = 0.5.ToString();
            TextPC2.Text = 0.5.ToString();
        }
        
        public void DrawPoint(Point point, Brush brush)
        {
            Ellipse elipse = new Ellipse();
 
            elipse.Width = 8;
            elipse.Height = 8;
 
            elipse.StrokeThickness = 4;
            elipse.Stroke = brush;
            elipse.Margin = new Thickness(point.X, point.Y, 0, 0);
 
            canvas.Children.Add(elipse);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Evaluate();
        }

        private void Evaluate()
        {
            double PC1 = Convert.ToDouble(TextPC1.Text);
            double PC2 = Convert.ToDouble(TextPC1.Text);
            //TODO Если РС1 == 0 и РС2 = 1 то все равно выводиться MessageBox
            if (PC1 + PC2 != 1)
            {
                MessageBox.Show("Cумма вероятностей должна быть равна 1.", "Ошибка ввода данных",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (PC1 > 1 || PC2 > 1)
            {
                MessageBox.Show("Значения вероятностей должны быть не больше 1.", "Ошибка ввода данных",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            int pointsCount = 15000;
            int scaleMode = 250000;
            double mu1 = 0, mu2 = 0, sigma1 = 0, sigma2 = 0, p1 = 0, p2 = 0;
            
            //TODO Сделай функцию по прототипу void GetGaussParament(out double mu1, out double mu2, out double sigma1, out double sigma2, out int pointCount)
            
            for (int i = 0; i < pointsCount; i++)
            {
                p1 = Math.Exp(-0.5 * Math.Pow((i - mu1) / sigma1, 2)) /
                     (sigma1 * Math.Sqrt(2 * Math.PI));
                DrawPoint(new Point(i, canvas.Height - (int) (p1 * PC1 * scaleMode)), Brushes.Red);
            }
            for (int i = 0; i < pointsCount; i++)
            {
                p2 = Math.Exp(-0.5 * Math.Pow((i - mu2) / sigma2, 2)) /
                     (sigma2 * Math.Sqrt(2 * Math.PI));
                DrawPoint(new Point(i, canvas.Height - (int) (p2 * PC2 * scaleMode)), Brushes.Green);
            }

            double falseAlarm = 0, missingDetection = 0;
            
            //TODO Ложная тревога, пропуск обнаружения и сумма void GetP(out double falseArarm, out missingDetection)
            
            TextFalseAlarm.Text = falseAlarm.ToString();
            TextMissingDetection.Text = missingDetection.ToString();
            TextSum.Text = (falseAlarm + missingDetection).ToString();
        }
    }
}