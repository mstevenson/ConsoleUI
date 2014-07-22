using System;
using System.Collections.Generic;

namespace ConsoleUI
{
	public class Writer
	{
		Area area;
		BorderChars chars;

		public Writer ()
		{
			Console.Clear();
			Console.SetCursorPosition (0, Console.WindowHeight - 1);
			area = Area.Fullscreen;
			LineStyle = LineStyle.Normal;
		}

		public void SetArea (int x, int y, int width, int height)
		{
			area = new Area (new Position (x, y), width, height);
		}

		public LineStyle LineStyle
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

		public void ScaleArea (int x, int y)
		{
			area.width = Math.Max (area.width + x, 3);
			area.height = Math.Max (area.height + y, 3);
		}

		public void ScaleAreaCentered (int x, int y)
		{
			ScaleArea (2 * x, 2 * y);
			MoveArea (-1 * x, -1 * y);
		}

		public void MoveArea (int x, int y)
		{
			area.pos.x += x;
			area.pos.y += y;
		}

		public void DrawBorder ()
		{
			// Horizontal
			for (int x = 1; x < area.width - 1; x++) {
				area.WriteChar (new Position (x, 0), chars.top);
				area.WriteChar (new Position (x, area.height - 1), chars.bottom);
			}
			// Vertical
			for (int y = 1; y < area.height - 1; y++) {
				area.WriteChar (new Position (0, y), chars.left);
				area.WriteChar (new Position (area.width - 1, y), chars.right);
			}
			area.WriteChar (area.TopLeft, chars.topLeft);
			area.WriteChar (area.TopRight, chars.topRight);
			area.WriteChar (area.BottomLeft, chars.bottomLeft);
			area.WriteChar (area.BottomRight, chars.bottomRight);
			ScaleAreaCentered (-1, -1);
		}

		public void WriteList (Position pos, string label, IList<string> contents)
		{
			var newPos = pos;
			// Header
			WriteString (newPos, label, Justification.Center);
			newPos.y++;
			// Divider
			HorizontalDivider (newPos.y);
			newPos.y++;
			// List
			foreach (var s in contents)
			{
				WriteString (newPos, s);
				newPos.y++;
			}
		}

		/// <summary>
		/// Write a string that is truncated within the console buffer and return
		/// the cursor position to the beginning of the string.
		/// </summary>
		public void WriteString (Position pos, string s, Justification justification = Justification.Left)
		{
			area.WriteString (pos, s, justification);
		}

		public void WriteChar (Position pos, char c)
		{
			area.WriteChar (pos, c);
		}

		public void HorizontalDivider (int yPos)
		{
			for (int x = 0; x < area.width; x++) {
				char c = chars.horizontal;
				// TODO cache the chars in the area buffer and read them back so that we can intelligently create connecting lines
				area.WriteChar (new Position (x, yPos), c);
			}
		}

		public void VerticalDivider (int xPos)
		{
			var c = chars.left;
			for (int y = 0; y < area.height; y++) {
				area.WriteString (new Position (xPos, y), c.ToString ());
			}
		}

		public void ClearArea ()
		{
			area.Clear ();
		}
	}

}


