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
	public struct Direction
	{
		public int horizontal, vertical;

		public Direction(int horizontal, int vertical)
		{
			this.horizontal = horizontal;
			this.vertical = vertical;
		}
	}

	public struct Directions
	{
		public Direction A, B;

		public Directions(Direction left, Direction right)
		{
			this.A = left;
			this.B = right;
		}
	}

	public class Spelbord : Canvas
	{
		public const int KOLOMMEN = 20;
		public const int RIJEN = 20;
		public const int AANTAL_SPELERS = 5;
		public const int AANTAL_OP_RIJ = 4;

		public int huidigeSpeler = 0;
		public double kolw;
		public double rijh;
		private Cel[,] celContainer = new Cel[RIJEN,KOLOMMEN]; 
		private List<Cel> winContainer = new List<Cel>(); 
		private HitBox[] colBoxContainer = new HitBox[KOLOMMEN];
		private Directions[] directionsContainer = { 
			new Directions( new Direction( 1, 0), new Direction(-1, 0) ),
			new Directions( new Direction( 0, 1), new Direction( 0,-1) ),
			new Directions( new Direction( 1, 1), new Direction(-1,-1) ),
			new Directions( new Direction( 1,-1), new Direction(-1, 1) ),
		};

		public Spelbord() { this.Loaded += Init; }

		void Init(object sender, Windows.UI.Xaml.RoutedEventArgs e) {

			this.Background = new SolidColorBrush(Colors.DarkSlateGray);
			kolw = this.ActualWidth / KOLOMMEN;
			rijh = this.ActualHeight / RIJEN;
			VulGridOp();
		}

		public int SameSeek(Cel celin, Directions directions) {
			int punten;
			punten = (TelPunten(celin, directions.A) + TelPunten(celin, directions.B) - 1);

			winContainer.Add(celin);
			if (punten >= AANTAL_OP_RIJ) {
				TekenGewonnen(winContainer);
			}
			winContainer.Clear();

			return punten;
		}

		private void TekenGewonnen(List<Cel> celContainer)
		{
			foreach (Cel celCur in winContainer)
			{
				Border border = new Border()
				{
					BorderBrush = new SolidColorBrush(Colors.Orange),
					BorderThickness = new Windows.UI.Xaml.Thickness(5, 5, 5, 5)
				};
				celCur.Children.Add(border);
			}
		}

		private int TelPunten(Cel celin, Direction direction) {

			int huidige_op_rij = 1; 
			Cel origCel = celin;
			Cel currCel = celin;

			for (int step = 0; step < AANTAL_OP_RIJ; step++) {
				int verShift = currCel.rowPos + direction.horizontal;
				int horShift = currCel.colPos + direction.vertical;
				if (CelExists(verShift, horShift)) {
					currCel = celContainer[verShift, horShift];
					if (currCel.speler == origCel.speler)
					{
						huidige_op_rij++;
						winContainer.Add(currCel);
					}
					else break;
				}
				else break;
			}
			return huidige_op_rij;
		}

		public Boolean CelExists(int horin, int verin)
		{
			if ((horin >= celContainer.GetLength(0) || horin < 0) || (verin >= celContainer.GetLength(1) || verin < 0))
				return false;	
			return true;
		}

		public void BeschikbareStukken(HitBox box)
		{
			for (int rij = RIJEN - 1; rij >= 0; rij--) {
				Cel cel = celContainer[rij, box.colPos];
				if (celContainer[rij, box.colPos].vrij)
				{
					SpelerCycle();
					cel.maakBezet(huidigeSpeler);
					CelCheck(cel);
					break;
				}
			}
		}

		private void CelCheck(Cel cel)
		{
			foreach (Directions directions in directionsContainer)
				SameSeek(cel, directions);
		}

		public void VulGridOp()
		{
			for (int rij = 0; rij < RIJEN; rij++)
			{
				for (int kol = 0; kol < KOLOMMEN; kol++)
				{
					celContainer[rij, kol] = new Cel(kolw, rijh, rij, kol);	
					this.Children.Add(celContainer[rij, kol]);
				}
			}

			for (int kolom = 0; kolom < KOLOMMEN; kolom++)
			{
				colBoxContainer[kolom] = new HitBox(kolom, this.ActualHeight, this.kolw);
				this.Children.Add(colBoxContainer[kolom]);
				colBoxContainer[kolom].PointerPressed += rowBox_PointerPressed;
			}
		}

		private void SpelerCycle()
		{
			if (huidigeSpeler + 1 > AANTAL_SPELERS)
				huidigeSpeler = 1;
			else
				huidigeSpeler++;
		}

		void rowBox_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) { BeschikbareStukken(sender as HitBox); }
	}
}