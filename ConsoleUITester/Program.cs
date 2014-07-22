using System;
using ConsoleUI;
using System.Threading;

namespace ConsoleUITester
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.Title = "ConsoleUI";
			var w = new Writer ();
			w.SetArea (10, 5, 50, 10);

			w.LineStyle = LineStyle.Thick;
			w.DrawBorder ();
			w.LineStyle = LineStyle.Normal;
			w.DrawBorder ();

			w.ScaleAreaCentered (-8, 0);

			w.WriteList (Position.Zero, "List", new[] { "a", "b", "c" });

//			float milliseconds = 0;
//			float duration = 4;
//
//			while (milliseconds < duration * 1000) {
//				var xEase = ExpoEaseInOut (milliseconds / 1000, 0, 40, 4);
//				w.ClearArea ();
//				w.WriteString (new Position ((int)xEase, 0), "hello");
//				milliseconds += 10;
//				Thread.Sleep(10);
//			}

			Thread.Sleep(500);
		}

		/// <summary>
		/// Easing equation function for a quadratic (t^2) easing in/out: 
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static double QuadEaseInOut( double t, double b, double c, double d )
		{
			if ( ( t /= d / 2 ) < 1 )
				return c / 2 * t * t + b;

			return -c / 2 * ( ( --t ) * ( t - 2 ) - 1 ) + b;
		}

		/// <summary>
		/// Easing equation function for an exponential (2^t) easing in/out: 
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static double ExpoEaseInOut( double t, double b, double c, double d )
		{
			if ( t == 0 )
				return b;

			if ( t == d )
				return b + c;

			if ( ( t /= d / 2 ) < 1 )
				return c / 2 * Math.Pow( 2, 10 * ( t - 1 ) ) + b;

			return c / 2 * ( -Math.Pow( 2, -10 * --t ) + 2 ) + b;
		}
	}
}
