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

using GaussianDistribution;

namespace WpfApp2
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
        public void DrawPoint(System.Windows.Point point, Brush brush)
        {
            Ellipse elipse = new Ellipse();

            elipse.Width = 4;
            elipse.Height = 4;

            elipse.StrokeThickness = 4;
            elipse.Stroke = brush;
            elipse.Margin = new Thickness(point.X, point.Y, 0, 0);

            canvas.Children.Add(elipse);
        }
        public void DrawLine(int X, int Height,Brush brush)
        {
            Rectangle rectangle = new Rectangle();

            rectangle.Width = 1;
            rectangle.Height = Height;

            rectangle.StrokeThickness = 4;
            rectangle.Stroke = brush;
            rectangle.Margin = new Thickness(X, 0, 0, Height);

            canvas.Children.Add(rectangle);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Evaluate();
        }
        private void Evaluate()
        {
            double PC1 = Convert.ToDouble(TextPC1.Text);
            double PC2 = Convert.ToDouble(TextPC2.Text);
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

            int pointsCount = 10000;
            int scaleMode = 250000;
            double mathExcept1 = 0;
            double sigma1 = 0;
            double mathExcept2 = 0;
            double sigma2 = 0;
            
            FaultResult faultResult = GaussianGenerator.Generate(PC1, PC2, pointsCount, out mathExcept1, out sigma1, out mathExcept2, out sigma2);
            canvas.Children.Clear();
            for (int i = 0; i < pointsCount; i++)
            {
                var p1 = GaussianGenerator.GaussFunction(i,mathExcept1,sigma1);
                DrawPoint(new System.Windows.Point(i, canvas.Height - (int)(p1 * PC1 * scaleMode)), Brushes.Orange);
            }
            for (int i = 0; i < pointsCount; i++)
            {
                var p2 = GaussianGenerator.GaussFunction(i,mathExcept2,sigma2);
                DrawPoint(new System.Windows.Point(i, canvas.Height - (int)(p2 * PC2 * scaleMode)), Brushes.Green);
            }

            if (!double.IsNaN(faultResult.borderX)) {
                DrawLine((int)faultResult.borderX, (int)canvas.Height, Brushes.Purple);
            }
            TextFalseAlarm.Text = faultResult.falseAlarmError.ToString();
            TextMissingDetection.Text = faultResult.missingDetectingError.ToString();
            TextSum.Text = faultResult.totalClassificationError.ToString();
        }
    }
}
