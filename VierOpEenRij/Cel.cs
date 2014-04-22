using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace VierOpEenRij
{
	public class Cel : Grid
	{
		private Ellipse ellipse = new Ellipse();
		public int rowPos;
		public int colPos;
		public int speler = -1;
		public Boolean vrij = true;
		public static Color[] coloring = { Colors.Green, Colors.Yellow, Colors.Red, Colors.Blue, Colors.OrangeRed };

		public Cel(double width, double height, int rowPos, int colPos)
		{
			Border border = new Border()
			{
				BorderBrush = new SolidColorBrush(Colors.Black),
				BorderThickness = new Windows.UI.Xaml.Thickness(0.1, 0.1, 0.1, 0.1),
				CornerRadius = new Windows.UI.Xaml.CornerRadius(2, 2, 2, 2)
			};
			this.Children.Add(border);

			this.Width = width;
			this.Height = height;
			this.colPos = colPos;
			this.rowPos = rowPos;

			this.SetValue(Canvas.TopProperty, height * rowPos);
			this.SetValue(Canvas.LeftProperty, width * colPos);
			this.IsHitTestVisible = false;
		}

		public void maakBezet(int huidigeSpeler)
		{
			vrij = false;
			speler = huidigeSpeler;
			this.Background = new SolidColorBrush(coloring[speler]);
		}

	}
}
