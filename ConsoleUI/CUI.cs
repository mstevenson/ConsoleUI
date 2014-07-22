using System;
using System.Collections.Generic;

namespace ConsoleUI
{
	public static class CUI
	{
		static Area area;
		static BorderChars borderChars;
		static int cachedWindowPosX;
		static int cachedWindowPosY;
		static Justification justification = Justification.Left;

		static CUI ()
		{
			Console.Clear();
			Console.SetCursorPosition (0, Console.WindowHeight - 1);
			SetAreaFullScreen ();
			LineStyle = LineStyle.Normal;
		}

		static public LineStyle LineStyle
		{
			get {
				return _lineStyle;
			}
			set {
				_lineStyle = value;
				switch (_lineStyle) {
				case LineStyle.Thick:
					borderChars = new BorderChars ('━', '━', '┃', '┃', '┏', '┓', '┗', '┛', '┳', '┻', '┣', '┫', '╋', '━', '┃');
					break;
				case LineStyle.Double:
					borderChars = new BorderChars ('═', '═', '║', '║', '╔', '╗', '╚', '╝', '╦', '╩', '╠', '╣', '╬', '═', '║');
					break;
				case LineStyle.Normal:
				default:
					borderChars = new BorderChars ('─', '─', '│', '│', '┌', '┐', '└', '┘', '┬', '┴', '├', '┤', '┼', '─', '│');
					break;
				}
			}
		}
		static LineStyle _lineStyle;

		static public Position CursorPosition {
			get {
				return area.cursorPosition;
			}
			set {
				area.cursorPosition = value;
			}
		}

		static public void MoveCursorUp (int distance = 1)
		{
			CursorPosition = CursorPosition.GetUp (distance);
		}

		static public void MoveCursorDown (int distance = 1)
		{
			CursorPosition = CursorPosition.GetDown (distance);
		}

		static public void MoveCursorLeft (int distance = 1)
		{
			CursorPosition = CursorPosition.GetLeft (distance);
		}

		static public void MoveCursorRight (int distance = 1)
		{
			CursorPosition = CursorPosition.GetRight (distance);
		}

		static public void SetArea (int x, int y, int width, int height)
		{
			area = new Area (new Position (x, y), width, height);
		}

		static public void ScaleArea (int x, int y)
		{
			area.width = Math.Max (area.width + x, 3);
			area.height = Math.Max (area.height + y, 3);
		}

		static public void ScaleAreaCentered (int x, int y)
		{
			ScaleArea (2 * x, 2 * y);
			MoveArea (-1 * x, -1 * y);
		}

		static public void MoveArea (int x, int y)
		{
			area.windowPosition.x += x;
			area.windowPosition.y += y;
		}

//		static public void MoveAreaInWindowBounds (float percentX, float percentY)
//		{
//			var halfWidth = Lerp ( area.width / 2f;
//			var halfHeight = area.height / 2f;
//
//		}
//
//		static float Lerp (float x, float x0, float x1, float y0, float y1)
//		{
//			if ((x1 - x0) == 0) {
//				return (y0 + y1) / 2;
//			}
//			return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
//		}

		static public void SetAreaFullScreen ()
		{
			area = new Area (Position.Zero, Console.WindowWidth, Console.WindowHeight);
		}

		static void CacheWindowCursorPos ()
		{
			cachedWindowPosX = Console.CursorLeft;
			cachedWindowPosY = Console.CursorTop;
		}

		static void RestoreWindowCursorPos ()
		{
			Console.SetCursorPosition (cachedWindowPosX, cachedWindowPosY);
		}

		static public void SetJustification (Justification j)
		{
			justification = j;
		}

		static public void DrawString (string s)
		{
			DrawString (CursorPosition, s);
		}

		static public void DrawString (int x, int y, string s)
		{
			DrawString (new Position (x, y), s);
		}

		static public void DrawString (Position localPos, string s)
		{
			CacheWindowCursorPos ();
			CursorPosition = localPos;

			if (localPos.x >= area.width || localPos.y >= area.height || localPos.x < 0 || localPos.y < 0) {
				return;
			}
			var windowPos = AreaToWindowPosition (localPos);
			if (windowPos.x >= Console.BufferWidth || windowPos.y >= Console.BufferHeight || windowPos.x < 0 || windowPos.y < 0) {
				return;
			}
			if (justification == Justification.Center) {
				s = new string(' ', area.width / 2 - s.Length / 2) + s;
			}

			// Truncate strings that overflow the area
//			int maxWidth = WindowClippedPositionInArea - localPos.x;
//			if (maxWidth > 0) {
//				s = s.Length <= maxWidth ? s : s.Substring (0, maxWidth);
				// Write text
				Console.SetCursorPosition (windowPos.x, windowPos.y);
				Console.Write (s);
//			}

			RestoreWindowCursorPos ();
		}

		static public void DrawCharacter (char c)
		{
			DrawCharacter (CursorPosition, c);
		}

		static public void DrawCharacter (int x, int y, char c)
		{
			DrawCharacter (new Position (x, y), c);
		}

		static public void DrawCharacter (Position localPos, char c)
		{
			CacheWindowCursorPos ();
			CursorPosition = localPos;

			if (localPos.x >= area.width || localPos.y >= area.height) {
				return;
			}
			var windowPos = AreaToWindowPosition (localPos);
			if (windowPos.x >= Console.BufferWidth || windowPos.y >= Console.BufferHeight) {
				return;
			}
			Console.SetCursorPosition (windowPos.x, windowPos.y);
			Console.Write (c);

			RestoreWindowCursorPos ();
		}

		static public void DrawList (IList<string> contents)
		{
			foreach (var s in contents)
			{
				DrawString (CursorPosition, s);
				MoveCursorDown ();
			}
		}

		static public void DrawHorizontalDivider ()
		{
			DrawHorizontalDivider (CursorPosition.y);
		}

		static public void DrawHorizontalDivider (int yPos)
		{
			DrawString (new Position (0, yPos), new string (borderChars.horizontal, area.width));
		}

		static public void DrawVerticalDivider ()
		{
			DrawVerticalDivider (CursorPosition.x);
		}

		static public void DrawVerticalDivider (int xPos)
		{
			var c = borderChars.left;
			for (int y = 0; y < area.height; y++) {
				DrawString (new Position (xPos, y), c.ToString ());
			}
		}

		/// <summary>
		/// Border drawing contracts the current area so that borders will not be overwritten by subsequent draw calls.
		/// </summary>
		static public void DrawAreaBorder ()
		{
			var cachedAreaPos = CursorPosition;

			// Horizontal
			for (int x = 1; x < area.width - 1; x++) {
				DrawCharacter (x, 0, borderChars.top);
				DrawCharacter (x, area.height - 1, borderChars.bottom);
			}
			// Vertical
			for (int y = 1; y < area.height - 1; y++) {
				DrawCharacter (0, y, borderChars.left);
				DrawCharacter (area.width - 1, y, borderChars.right);
			}
			DrawCharacter (area.TopLeft, borderChars.topLeft);
			DrawCharacter (area.TopRight, borderChars.topRight);
			DrawCharacter (area.BottomLeft, borderChars.bottomLeft);
			DrawCharacter (area.BottomRight, borderChars.bottomRight);
			CursorPosition = Position.Zero;

			CursorPosition = cachedAreaPos;

			ScaleAreaCentered (-1, -1);
		}

		static public void Fill (ConsoleColor color)
		{
			Console.BackgroundColor = color;
			Clear ();
			Console.ResetColor ();
		}

		static public void Clear ()
		{
			int left = Console.CursorLeft;
			int top = Console.CursorTop;
			var spaces = new string (' ', area.width);
			for (int y = area.windowPosition.y; y < area.windowPosition.y + area.height; y++) {
				Console.SetCursorPosition (area.windowPosition.x, y);
				Console.Write (spaces);
			}
			Console.SetCursorPosition (left, top);
		}

//		Position WindowClippedPositionInArea {
//			get {
//				var x = Math.Min (Console.BufferWidth - area.windowPosition.x, area.width);
//				var y = Math.Max (Console.BufferHeight - area.windowPosition.y, area.height);
//				return new Position (x, y);
//			}
//		}

		static public bool IsWindowPositionInArea (Position p)
		{
			return IsLocalPositionInArea (WindowToAreaPosition (p));
		}

		static public bool IsLocalPositionInArea (Position p)
		{
			return p.x < area.width && p.x >= 0 && p.y < area.height && p.y >= 0;
		}

		static Position AreaToWindowPosition (Position p)
		{
			return new Position (p.x + area.windowPosition.x, p.y + area.windowPosition.y);
		}

		static Position WindowToAreaPosition (Position p)
		{
			return new Position (p.x - area.windowPosition.x, p.y - area.windowPosition.y);
		}
	}

}


