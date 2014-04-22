﻿using System;
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

	/*
	 * Lambda voor coord checken:
	 * delegate int del(int i) ::
	 * static void Main(string[] args)
	 * {
	 *	del myDelegate = x => x * x;
	 *	int j = myDelegate(5); //j = 25
	 * }
	 */

	public class Spelbord : Canvas
	{
		public const int KOLOMMEN = 7;
		public const int RIJEN = 1;
		public const int AANTAL_SPELERS = 1;
		public const int AANTAL_OP_RIJ = 4;

		public int huidigeSpeler = 0;
		public double kolw;
		public double rijh;
		private Cel[,] celContainer = new Cel[RIJEN,KOLOMMEN]; 
		private List<Cel> winContainer = new List<Cel>(); 
		private HitBox[] colBoxContainer = new HitBox[KOLOMMEN];

		public Directions horizontaal = new Directions(
			new Direction( 1, 0), 
			new Direction(-1, 0)
		);

		public Directions verticaal = new Directions(
			new Direction( 0, 1), 
			new Direction( 0,-1)
		);

		public Directions diagonaalLinks = new Directions(
			new Direction( 1, 1), 
			new Direction(-1,-1)
		);

		public Directions diagonaalRechts = new Directions(
			new Direction( 1,-1), 
			new Direction(-1, 1)
		);

		public Spelbord() { this.Loaded += Init; }

		void Init(object sender, Windows.UI.Xaml.RoutedEventArgs e) {

			this.Background = new SolidColorBrush(Colors.DarkSlateGray);
			kolw = this.ActualWidth / KOLOMMEN;
			rijh = this.ActualHeight / RIJEN;
			VulGridOp();
			BouwGrid();
		}

		public int SameSeek(Cel celin, Directions directions) {
			int punten;
			punten = (TelPunten(celin, directions.A) + TelPunten(celin, directions.B) - 1);

			winContainer.Add(celin);
			if (punten == AANTAL_OP_RIJ) {
				TekenGewonnen(winContainer);
			}
			winContainer.Clear();

			return punten;
		}

		private void TekenGewonnen(List<Cel> celContainer)
		{
			foreach (Cel celCur in winContainer)
			{
				Debug.WriteLine("[" + celCur.colPos + "|" + celCur.rowPos + "]");
				Border border = new Border()
				{
					BorderBrush = new SolidColorBrush(Colors.Orange),
					BorderThickness = new Windows.UI.Xaml.Thickness(5, 5, 5, 5)
				};
				celCur.Children.Add(border);
				//celCur.Background = new SolidColorBrush(Colors.Orange);
			}
			Debug.WriteLine("---");
		}

		private int TelPunten(Cel celin, Direction direction) {

			int huidige_op_rij = 1; //De huidige cel telt ook al voor een punt
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
			Debug.WriteLine("---");
		}

		private void CelCheck(Cel cel)
		{
			SameSeek(cel, diagonaalLinks);
			SameSeek(cel, diagonaalRechts);
			SameSeek(cel, horizontaal);
			SameSeek(cel, verticaal);
		}

		public void BouwGrid()
		{
			/* 
			 * Bouwt grid op
			 * aan de hand van
			 * de KOLOMMEN en RIJEN const
			 */
			for (int row = 0; row < RIJEN; row++){


				for (int col = 0; col < KOLOMMEN; col++)
				{
					Cel cel = celContainer[row, col];
					this.Children.Add(cel);
				}
			}
		}

		public void VulGridOp()
		{
			/*
			 * Vul elke cel met een
			 * Cel instantie
			 */

			for (int rij = 0; rij < RIJEN; rij++)
			{
				for (int kol = 0; kol < KOLOMMEN; kol++)
				{
					Cel cel = new Cel(kolw, rijh, rij, kol);	
					cel.SetValue(Canvas.TopProperty, rijh * rij);
					cel.SetValue(Canvas.LeftProperty, kolw * kol);
					cel.IsHitTestVisible = false;
					celContainer[rij, kol] = cel;
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
				huidigeSpeler = 0;
			else
				huidigeSpeler++;
		}

		void rowBox_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) { BeschikbareStukken(sender as HitBox); }
	}
}