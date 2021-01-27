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
using KMeans;
using Point = System.Windows.Point;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Brush[] _brushes = new[] {Brushes.Blue, Brushes.Brown, Brushes.Chartreuse, Brushes.Chocolate, Brushes.Cyan, Brushes.Gold, Brushes.Gray, Brushes.Fuchsia, Brushes.Green, Brushes.Indigo, Brushes.Lime, Brushes.Maroon, Brushes.Orchid, Brushes.Red, Brushes.Salmon, Brushes.Silver, Brushes.Teal, Brushes.Tomato, Brushes.DarkGreen, Brushes.Bisque };
        public MainWindow()
        {
            InitializeComponent();
            
            ClassesGenerator.Initialize(1000, 2);

            List<Area> areas = ClassesGenerator.Generate();
            int index = 0;
            foreach (var area in areas)
            {
                Brush brush = _brushes[index];
                foreach (var point in area.AreaPoints)
                {
                    DrawPoint(point, brush);
                }
                
                DrawPoint(area.Kernel, Brushes.Black);
                index++;
            }
        }

        public void DrawPoint(KMeans.Point point, Brush brush)
        {
            Ellipse elipse = new Ellipse();
 
            elipse.Width = 8;
            elipse.Height = 8;
 
            elipse.StrokeThickness = 4;
            elipse.Stroke = brush;
            elipse.Margin = new Thickness(point.X-500, point.Y-500, 0, 0);
 
            grid1.Children.Add(elipse);
        }
    }
}