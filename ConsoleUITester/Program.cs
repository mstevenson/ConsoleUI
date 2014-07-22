using System;
using ConsoleUI;
using System.Threading;

namespace ConsoleUITester
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var w = new Writer ();
			w.SetArea (5, 5, 30, 10);

			w.LineStyle2 = LineStyle.Thick;
			w.DrawBorder ();
//			w.LineStyle2 = LineStyle.Normal;
			w.DrawBorder ();

			w.Text (Position.Zero, "hello");

//			Thread.Sleep(1000);

//			w.SetArea (new Position (0, 0), 5, 1);
//			for (int i = 0; i < 5; i++) {
//				w.Clear ();
//				w.Text (new Position (i, 0), "hello");
//				Thread.Sleep(500);
//			}

			Thread.Sleep(500);
		}
	}
}
