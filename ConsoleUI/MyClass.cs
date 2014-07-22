using System;
using System.Collections.Generic;

namespace ConsoleUI
{
	public enum Justification {
		Left,
		Center,
		Right
	}

	public struct Position
	{
		public int x;
		public int y;

		public Position (int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public static Position Zero { get { return new Position (0, 0); } }
	}

	public enum LineStyle
	{
		Normal,
		Thick,
		Double,
	}

	public struct Area
	{
		public Position pos;
		public int width;
		public int height;

		public Area (Position pos, int width, int height)
		{
			this.pos = pos;
			this.width = width;
			this.height = height;
		}

		public Position TopLeft { get { return new Position (0, 0); } }

		public Position TopRight { get { return new Position (width - 1, 0); } }

		public Position TopCenter { get { return new Position(width / 2, 0); } }

		public Position BottomLeft { get { return new Position(0, height - 1); } }

		public Position BottomCenter { get { return new Position (width / 2, height - 1); } }

		public Position BottomRight { get { return new Position (width - 1, height - 1); } }

		public static Area Fullscreen { get { return new Area (Position.Zero, Console.WindowWidth, Console.WindowHeight); } }

		public void Clear ()
		{
			int left = Console.CursorLeft;
			int top = Console.CursorTop;
			var spaces = new string (' ', width);
			for (int y = pos.y; y < pos.y + height; y++) {
				Console.SetCursorPosition (pos.x, y);
				Console.Write (spaces);
			}
			Console.SetCursorPosition (left, top);
		}

//		public bool IsPositionInArea (Position localPosition)
//		{
//			return localPosition.x < Console.BufferWidth && x >= 0 && y < Console.BufferHeight && y >= 0;
//		}

		Position LocalToWindowPosition (Position p)
		{
			return new Position (p.x + pos.x, p.y + pos.y);
		}

		Position WindowToLocalPosition (Position p)
		{
			return new Position (p.x - pos.x, p.y - pos.y);
		}

		int WindowClippedWidth { get { return Math.Min (Console.BufferWidth - pos.x, width); } }

		int WindowClippedHeight { get { return Math.Max (Console.BufferHeight - pos.y, height); } }

		/// <summary>
		/// Write a string that is truncated within the console buffer and return
		/// the cursor position to the beginning of the string.
		/// </summary>
		public void Write (Position localPos, string s, Justification justification = Justification.Left)
		{
			int oldLeft = Console.CursorLeft;
			int oldTop = Console.CursorTop;

			if (localPos.x >= width || localPos.y >= height) {
				return;
			}
			var windowPos = LocalToWindowPosition (localPos);
			if (windowPos.x >= Console.BufferWidth || windowPos.y >= Console.BufferHeight) {
				return;
			}
			if (justification == Justification.Center) {
				s = new string(' ', width / 2 - s.Length / 2) + s;
			}

			// Truncate strings that overflow the area
			int maxWidth = WindowClippedWidth - localPos.x;
			if (maxWidth > 0) {
				s = s.Length <= maxWidth ? s : s.Substring (0, maxWidth);
				// Write text
				Console.SetCursorPosition (windowPos.x, windowPos.y);
				Console.Write (s);
			}
			Console.SetCursorPosition (oldLeft, oldTop);
		}

		public void Write (Position localPos, char c)
		{
			int origLeft = Console.CursorLeft;
			int origTop = Console.CursorTop;

			if (localPos.x >= width || localPos.y >= height) {
				return;
			}
			var windowPos = LocalToWindowPosition (localPos);
			if (windowPos.x >= Console.BufferWidth || windowPos.y >= Console.BufferHeight) {
				return;
			}
			Console.SetCursorPosition (windowPos.x, windowPos.y);
			Console.Write (c);
			Console.SetCursorPosition (origLeft, origTop);
		}
	}

	public class Writer
	{
		Area area;
		BorderChars chars;

		public Writer ()
		{
			Console.Clear();
			Console.SetCursorPosition (0, Console.WindowHeight - 1);
			area = Area.Fullscreen;
			LineStyle2 = LineStyle.Normal;
		}

		public void SetArea (int x, int y, int width, int height)
		{
			area = new Area (new Position (x, y), width, height);
		}

		public LineStyle LineStyle2
		{
			get {
				return _lineStyle;
			}
			set {
				_lineStyle = value;
				switch (_lineStyle) {
				case LineStyle.Thick:
					chars = new BorderChars ('━', '━', '┃', '┃', '┏', '┓', '┗', '┛', '┳', '┻', '┣', '┫', '╋', '━', '┃');
					break;
				case LineStyle.Double:
					chars = new BorderChars ('═', '═', '║', '║', '╔', '╗', '╚', '╝', '╦', '╩', '╠', '╣', '╬', '═', '║');
					break;
				case LineStyle.Normal:
				default:
					chars = new BorderChars ('─', '─', '│', '│', '┌', '┐', '└', '┘', '┬', '┴', '├', '┤', '┼', '─', '│');
					break;
				}
			}
		}
		LineStyle _lineStyle;

		public void ContractArea (int distance)
		{
			if (area.width < 3 || area.height < 3) {
				return;
			}
			area = new Area (new Position (area.pos.x + 1, area.pos.y + 1), area.width - 2, area.height - 2);
		}

		public void ExpandArea (int distance)
		{
			area = new Area (new Position (area.pos.x - 1, area.pos.y - 1), area.width + 2, area.height + 2);
		}

		class BorderChars
		{
			public char top;
			public char bottom;
			public char left;
			public char right;
			public char topLeft;
			public char topRight;
			public char bottomLeft;
			public char bottomRight;
			public char teeTop;
			public char teeBottom;
			public char teeLeft;
			public char teeRight;
			public char cross;
			public char horizontal;
			public char vertical;

			public BorderChars (char top, char bottom, char left, char right, char topLeft, char topRight, char bottomLeft, char bottomRight, char teeTop, char teeBottom, char teeLeft, char teeRight, char cross, char horizontal, char vertical) {
				this.top = top;
				this.bottom = bottom;
				this.left = left;
				this.right = right;
				this.topLeft = topLeft;
				this.topRight = topRight;
				this.bottomLeft = bottomLeft;
				this.bottomRight = bottomRight;
				this.horizontal = horizontal;
				this.vertical = vertical;
				this.teeTop = teeTop;
				this.teeBottom = teeBottom;
				this.teeLeft = teeLeft;
				this.teeRight = teeRight;
				this.cross = cross;
			}
		}

		public void DrawBorder ()
		{
			// Horizontal
			for (int x = 1; x < area.width - 1; x++) {
				Text (new Position (x, 0), chars.top);
				Text (new Position (x, area.height - 1), chars.bottom);
			}
			// Vertical
			for (int y = 1; y < area.height - 1; y++) {
				Text (new Position (0, y), chars.left);
				Text (new Position (area.width - 1, y), chars.right);
			}
			Text (area.TopLeft, chars.topLeft);
			Text (area.TopRight, chars.topRight);
			Text (area.BottomLeft, chars.bottomLeft);
			Text (area.BottomRight, chars.bottomRight);
			ContractArea (1);
		}

//		public void WriteList (Position pos, int width, string label, IList<string> contents)
//		{
//			if (!pos.IsInArea)
//			{
//				return;
//			}
//			var newPos = pos;
//
//			// Header
//			Write(newPos, label, width, Justification.Center);
//			newPos.y++;
//			// Divider
//			Write(newPos, new String('-', width), width);
//			newPos.y++;
//			// List
//			foreach (var s in contents)
//			{
//				if (newPos.IsInArea)
//				{
//					Write(newPos, s, width);
//					newPos.y++;
//				}
//				else
//				{
//					break;
//				}
//			}
//			Console.SetCursorPosition(pos.x, pos.y);
//		}

		/// <summary>
		/// Write a string that is truncated within the console buffer and return
		/// the cursor position to the beginning of the string.
		/// </summary>
		public void Text (Position pos, string s, Justification justification = Justification.Left)
		{
			area.Write (pos, s, justification);
		}

		public void Text (Position pos, char c)
		{
			area.Write (pos, c);
		}

		public void HorizontalDivider (int yPos)
		{
			for (int x = 0; x < area.width; x++) {
				char c = chars.horizontal;
				// TODO cache the chars in the area buffer and read them back so that we can intelligently create connecting lines
				area.Write (new Position (x, yPos), c);
			}
		}

		public void VerticalDivider (int xPos)
		{
			var c = chars.left;
			for (int y = 0; y < area.height; y++) {
				area.Write (new Position (xPos, y), c.ToString ());
			}
		}

		public void Clear ()
		{
			area.Clear ();
		}
	}

}


