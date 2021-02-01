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
using Maximin;
using Generator;
namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Brush[] _brushes = new[] { Brushes.Blue, Brushes.Gold, Brushes.Green, Brushes.Indigo, Brushes.Lime, Brushes.Maroon, Brushes.Gray, Brushes.Fuchsia, Brushes.Brown, Brushes.Chartreuse, Brushes.Chocolate, Brushes.Cyan, Brushes.Orchid, Brushes.Red, Brushes.Salmon, Brushes.Silver, Brushes.Teal, Brushes.Tomato, Brushes.DarkGreen, Brushes.Bisque };
        private static int Iterator;
        private List<List<IArea>> Areas = new List<List<IArea>>();
        public MainWindow()
        {
            InitializeComponent();
            //while (Areas.Count < 6)
            List<IArea> temp;
            {
                Areas = new List<List<IArea>>();
                temp = new List<IArea>();
                Maximin.ClassesGenerator.Initialize(10000).ForEach(area => temp.Add(new Maximin.Area(area)));
                Areas.Add(temp);
                while (!Maximin.ClassesGenerator.EndIteration())
                {
                    temp = new List<IArea>();
                    Maximin.ClassesGenerator.Iteration().ForEach(area => temp.Add(new Maximin.Area(area)));
                    Areas.Add(temp);
                }
            }
            Iterator = 0;
            KMeans.ClassesGenerator.Initialize(Areas[Areas.Count - 1], Maximin.ClassesGenerator.GetPoints());
            while (!KMeans.ClassesGenerator.EndIteration())
            {
                temp = new List<IArea>();
                KMeans.ClassesGenerator.Iteration().ForEach(area => temp.Add(new Maximin.Area(area)));
                Areas.Add(temp);
            }
            //temp = new List<IArea>();
            //KMeans.ClassesGenerator.Generate().ForEach(area => temp.Add(new KMeans.Area(area)));
            //Areas.Add(temp);
            DrawAreas();
        }
        private void Iteration_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.NumPad6) { Right_Click(null, null); }
            if (e.Key == Key.NumPad4) { Left_Click(null, null); }
        }
        private void Right_Click(object sender, RoutedEventArgs e)
        {
            if (Iterator+1 < Areas.Count) {
                Iterator++;
                DrawAreas();
            } 
        }
        private void Left_Click(object sender, RoutedEventArgs e)
        {
            if (Iterator>0)
            {
                Iterator--;
                DrawAreas();
            }
        }
        private void DrawAreas() {
            int index = 0;
            List<IArea> areas = Areas[Iterator];
            grid1.Children.Clear();
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
        public void DrawPoint(Generator.Point point, Brush brush)
        {            
            Ellipse elipse = new Ellipse();

            elipse.Width = 8;
            elipse.Height = 8;

            elipse.StrokeThickness = 4;
            elipse.Stroke = brush;
            elipse.Margin = new Thickness(point.X - 500, point.Y - 500, 0, 0);

            grid1.Children.Add(elipse);
        }
    }
}
