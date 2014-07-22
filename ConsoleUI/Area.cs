using System;

namespace ConsoleUI
{
	public struct Area
	{
		public Position windowPosition; // top left corner in window
		public int width;
		public int height;
		public Position cursorPosition;

		public Area (Position pos, int width, int height)
		{
			this.windowPosition = pos;
			this.width = width;
			this.height = height;
			this.cursorPosition = Position.Zero;
		}

		public Position TopLeft { get { return new Position (0, 0); } }

		public Position TopRight { get { return new Position (width - 1, 0); } }

		public Position TopCenter { get { return new Position(width / 2, 0); } }

		public Position BottomLeft { get { return new Position(0, height - 1); } }

		public Position BottomCenter { get { return new Position (width / 2, height - 1); } }

		public Position BottomRight { get { return new Position (width - 1, height - 1); } }
	}
}

