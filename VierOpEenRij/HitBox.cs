using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace VierOpEenRij
{
	public class HitBox : Grid
	{
		public int colPos;

		public HitBox(int colPos, double height, double width){
			this.colPos = colPos;
			this.Height = height;
			this.Width = width;
			this.Background = new SolidColorBrush(Colors.Transparent);
			this.SetValue(Canvas.LeftProperty, actualColPos());
		}

		public double actualColPos()
		{
			return colPos * Width;
		}

	}
}
