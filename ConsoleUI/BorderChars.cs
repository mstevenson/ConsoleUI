namespace ConsoleUI
{
    public struct BorderChars
    {
        public readonly char Top;
        public readonly char Bottom;
        public readonly char Left;
        public readonly char Right;
        public readonly char TopLeft;
        public readonly char TopRight;
        public readonly char BottomLeft;
        public readonly char BottomRight;
        public readonly char TeeTop;
        public readonly char TeeBottom;
        public readonly char TeeLeft;
        public readonly char TeeRight;
        public readonly char Cross;
        public readonly char Horizontal;
        public readonly char Vertical;

        public BorderChars(char top, char bottom, char left, char right, char topLeft, char topRight, char bottomLeft,
            char bottomRight, char teeTop, char teeBottom, char teeLeft, char teeRight, char cross, char horizontal,
            char vertical)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;
            Horizontal = horizontal;
            Vertical = vertical;
            TeeTop = teeTop;
            TeeBottom = teeBottom;
            TeeLeft = teeLeft;
            TeeRight = teeRight;
            Cross = cross;
        }
    }
}

