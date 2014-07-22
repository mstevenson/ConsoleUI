using System;

namespace ConsoleUI
{
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

		public Position GetDown (int distance = 1) { return new Position (x, y + distance); }

		public Position GetUp (int distance = 1) { return new Position (x, y - distance); }

		public Position GetLeft (int distance = 1) { return new Position (x - 1, y); }

		public Position GetRight (int distance = 1) { return new Position (x + 1, y); }
	}
}

