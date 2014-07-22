using System;

namespace ConsoleUI
{
	public class BorderChars
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
}

