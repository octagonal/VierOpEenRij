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

		static Color[] coloring = null;

		public Cel(double width, double height, int rowPos, int colPos)
		{	
			coloring = InitKleuren();
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

		private static Color[] InitKleuren()
		{
			Color[] colors = new Color[Spelbord.AANTAL_SPELERS];
			Random rand = new Random();
			byte lead = 255 / Spelbord.AANTAL_SPELERS;
			for (int i = 0; i < colors.Length; i++)
			{
				Color color = new Color();
				color.A = 255;
				color.G = Convert.ToByte(lead * i);
				color.B = Convert.ToByte(rand.Next(0, 255));
				color.R = Convert.ToByte(rand.Next(0, 255));
				colors[i] = color;
			}
			return colors;
		}

		public void maakBezet(int huidigeSpeler)
		{
			vrij = false;
			speler = huidigeSpeler;
			this.Background = new SolidColorBrush(Cel.coloring[speler-1]);
		}

	}
}
