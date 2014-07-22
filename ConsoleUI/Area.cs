using System;

namespace ConsoleUI
{
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

		public bool IsPositionInArea (Position localPosition)
		{
			return localPosition.x < width && localPosition.x >= 0 && localPosition.y < height && localPosition.y >= 0;
		}

		int WindowClippedWidth { get { return Math.Min (Console.BufferWidth - pos.x, width); } }

		int WindowClippedHeight { get { return Math.Max (Console.BufferHeight - pos.y, height); } }

		Position LocalToWindowPosition (Position p)
		{
			return new Position (p.x + pos.x, p.y + pos.y);
		}

		Position WindowToLocalPosition (Position p)
		{
			return new Position (p.x - pos.x, p.y - pos.y);
		}

		/// <summary>
		/// Write a string that is clipped within the area.
		/// </summary>
		public void WriteString (Position localPos, string s, Justification justification = Justification.Left)
		{
			int oldLeft = Console.CursorLeft;
			int oldTop = Console.CursorTop;

			if (localPos.x >= width || localPos.y >= height || localPos.x < 0 || localPos.y < 0) {
				return;
			}
			var windowPos = LocalToWindowPosition (localPos);
			if (windowPos.x >= Console.BufferWidth || windowPos.y >= Console.BufferHeight || windowPos.x < 0 || windowPos.y < 0) {
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

		public void WriteChar (Position localPos, char c)
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
}

