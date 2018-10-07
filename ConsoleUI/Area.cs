namespace ConsoleUI
{
    public struct Area
    {
        public Position WindowPosition; // top left corner in window
        public int Width;
        public int Height;
        public Position CursorPosition;

        public Area(Position pos, int width, int height)
        {
            WindowPosition = pos;
            Width = width;
            Height = height;
            CursorPosition = Position.Zero;
        }

        public Position TopLeft => new Position(0, 0);

        public Position TopRight => new Position(Width - 1, 0);

        public Position TopCenter => new Position(Width / 2, 0);

        public Position BottomLeft => new Position(0, Height - 1);

        public Position BottomCenter => new Position(Width / 2, Height - 1);

        public Position BottomRight => new Position(Width - 1, Height - 1);
    }
}

