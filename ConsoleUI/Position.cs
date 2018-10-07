namespace ConsoleUI
{
    public struct Position
    {
        public int X;
        public int Y;

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Position Zero => new Position(0, 0);

        public Position GetDown(int distance = 1)
        {
            return new Position(X, Y + distance);
        }

        public Position GetUp(int distance = 1)
        {
            return new Position(X, Y - distance);
        }

        public Position GetLeft(int distance = 1)
        {
            return new Position(X - 1, Y);
        }

        public Position GetRight(int distance = 1)
        {
            return new Position(X + 1, Y);
        }
    }
}

