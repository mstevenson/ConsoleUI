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
	}
}

