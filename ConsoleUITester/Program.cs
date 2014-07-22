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
			CUI.SetArea (10, 5, 50, 10);

			Thread.Sleep (500);

			CUI.LineStyle = LineStyle.Thick;
			CUI.DrawAreaBorder ();
			CUI.SetJustification (Justification.Center);
			CUI.DrawString ("Label");
			CUI.MoveCursorDown ();
			CUI.DrawHorizontalDivider ();
			CUI.SetJustification (Justification.Left);
			CUI.MoveCursorDown ();
			CUI.DrawList (new[] { "a", "b", "c" });

		}
	}
}
