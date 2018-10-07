using System;
using System.Collections.Generic;

namespace ConsoleUI
{
    public static class CUI
    {
        private static Area _area;
        private static BorderChars _borderChars;
        private static int _cachedWindowPosX;
        private static int _cachedWindowPosY;
        private static Justification _justification = Justification.Left;

        static CUI ()
        {
            Console.Clear();
            Console.SetCursorPosition (0, Console.WindowHeight - 1);
            SetAreaFullScreen ();
            LineStyle = LineStyle.Normal;
        }

        private static LineStyle _lineStyle;
        public static LineStyle LineStyle
        {
            get {
                return _lineStyle;
            }
            set {
                _lineStyle = value;
                switch (_lineStyle) {
                case LineStyle.Thick:
                    _borderChars = new BorderChars ('━', '━', '┃', '┃', '┏', '┓', '┗', '┛', '┳', '┻', '┣', '┫', '╋', '━', '┃');
                    break;
                case LineStyle.Double:
                    _borderChars = new BorderChars ('═', '═', '║', '║', '╔', '╗', '╚', '╝', '╦', '╩', '╠', '╣', '╬', '═', '║');
                    break;
                case LineStyle.Normal:
                default:
                    _borderChars = new BorderChars ('─', '─', '│', '│', '┌', '┐', '└', '┘', '┬', '┴', '├', '┤', '┼', '─', '│');
                    break;
                }
            }
        }
        
        private static Position CursorPosition {
            get => _area.CursorPosition;
            set => _area.CursorPosition = value;
        }

        public static void MoveCursorUp(int distance = 1)
        {
            CursorPosition = CursorPosition.GetUp(distance);
        }

        public static void MoveCursorDown(int distance = 1)
        {
            CursorPosition = CursorPosition.GetDown(distance);
        }

        public static void MoveCursorLeft(int distance = 1)
        {
            CursorPosition = CursorPosition.GetLeft(distance);
        }

        public static void MoveCursorRight(int distance = 1)
        {
            CursorPosition = CursorPosition.GetRight(distance);
        }

        public static void SetArea(int x, int y, int width, int height)
        {
            _area = new Area(new Position(x, y), width, height);
        }

        public static void ScaleArea(int x, int y)
        {
            _area.Width = Math.Max(_area.Width + x, 3);
            _area.Height = Math.Max(_area.Height + y, 3);
        }

        public static void ScaleAreaCentered(int x, int y)
        {
            ScaleArea(2 * x, 2 * y);
            MoveArea(-1 * x, -1 * y);
        }

        public static void MoveArea(int x, int y)
        {
            _area.WindowPosition.X += x;
            _area.WindowPosition.Y += y;
        }

//		static public void MoveAreaInWindowBounds (float percentX, float percentY)
//		{
//			var halfWidth = Lerp ( area.width / 2f;
//			var halfHeight = area.height / 2f;
//
//		}
//
//		static float Lerp (float x, float x0, float x1, float y0, float y1)
//		{
//			if ((x1 - x0) == 0) {
//				return (y0 + y1) / 2;
//			}
//			return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
//		}

        public static void SetAreaFullScreen()
        {
            _area = new Area(Position.Zero, Console.WindowWidth, Console.WindowHeight);
        }

        private static void CacheWindowCursorPos()
        {
            _cachedWindowPosX = Console.CursorLeft;
            _cachedWindowPosY = Console.CursorTop;
        }

        private static void RestoreWindowCursorPos()
        {
            Console.SetCursorPosition(_cachedWindowPosX, _cachedWindowPosY);
        }

        public static void SetJustification(Justification j)
        {
            _justification = j;
        }

        public static void DrawString(string s)
        {
            DrawString(CursorPosition, s);
        }

        public static void DrawString(int x, int y, string s)
        {
            DrawString(new Position(x, y), s);
        }

        public static void DrawString(Position localPos, string s)
        {
            CacheWindowCursorPos();
            CursorPosition = localPos;

            if (localPos.X >= _area.Width || localPos.Y >= _area.Height || localPos.X < 0 || localPos.Y < 0)
            {
                return;
            }

            var windowPos = AreaToWindowPosition(localPos);
            if (windowPos.X >= Console.BufferWidth || windowPos.Y >= Console.BufferHeight || windowPos.X < 0 ||
                windowPos.Y < 0)
            {
                return;
            }

            if (_justification == Justification.Center)
            {
                s = new string(' ', _area.Width / 2 - s.Length / 2) + s;
            }

            // Truncate strings that overflow the area
//			int maxWidth = WindowClippedPositionInArea - localPos.x;
//			if (maxWidth > 0) {
//				s = s.Length <= maxWidth ? s : s.Substring (0, maxWidth);
            // Write text
            Console.SetCursorPosition(windowPos.X, windowPos.Y);
            Console.Write(s);
//			}

            RestoreWindowCursorPos();
        }

        public static void DrawCharacter(char c)
        {
            DrawCharacter(CursorPosition, c);
        }

        public static void DrawCharacter(int x, int y, char c)
        {
            DrawCharacter(new Position(x, y), c);
        }

        public static void DrawCharacter(Position localPos, char c)
        {
            CacheWindowCursorPos();
            CursorPosition = localPos;

            if (localPos.X >= _area.Width || localPos.Y >= _area.Height)
            {
                return;
            }

            var windowPos = AreaToWindowPosition(localPos);
            if (windowPos.X >= Console.BufferWidth || windowPos.Y >= Console.BufferHeight)
            {
                return;
            }

            Console.SetCursorPosition(windowPos.X, windowPos.Y);
            Console.Write(c);

            RestoreWindowCursorPos();
        }

        public static void DrawList(IList<string> contents)
        {
            foreach (var s in contents)
            {
                DrawString(CursorPosition, s);
                MoveCursorDown();
            }
        }

        public static void DrawHorizontalDivider()
        {
            DrawHorizontalDivider(CursorPosition.Y);
        }

        public static void DrawHorizontalDivider(int yPos)
        {
            DrawString(new Position(0, yPos), new string(_borderChars.Horizontal, _area.Width));
        }

        public static void DrawVerticalDivider()
        {
            DrawVerticalDivider(CursorPosition.X);
        }

        public static void DrawVerticalDivider(int xPos)
        {
            var c = _borderChars.Left;
            for (int y = 0; y < _area.Height; y++)
            {
                DrawString(new Position(xPos, y), c.ToString());
            }
        }

        /// <summary>
        /// Border drawing contracts the current area so that borders will not be overwritten by subsequent draw calls.
        /// </summary>
        public static void DrawAreaBorder()
        {
            var cachedAreaPos = CursorPosition;

            // Horizontal
            for (int x = 1; x < _area.Width - 1; x++)
            {
                DrawCharacter(x, 0, _borderChars.Top);
                DrawCharacter(x, _area.Height - 1, _borderChars.Bottom);
            }

            // Vertical
            for (int y = 1; y < _area.Height - 1; y++)
            {
                DrawCharacter(0, y, _borderChars.Left);
                DrawCharacter(_area.Width - 1, y, _borderChars.Right);
            }

            DrawCharacter(_area.TopLeft, _borderChars.TopLeft);
            DrawCharacter(_area.TopRight, _borderChars.TopRight);
            DrawCharacter(_area.BottomLeft, _borderChars.BottomLeft);
            DrawCharacter(_area.BottomRight, _borderChars.BottomRight);
            CursorPosition = Position.Zero;

            CursorPosition = cachedAreaPos;

            ScaleAreaCentered(-1, -1);
        }

        public static void Fill(ConsoleColor color)
        {
            Console.BackgroundColor = color;
            Clear();
            Console.ResetColor();
        }

        public static void Clear()
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            var spaces = new string(' ', _area.Width);
            for (int y = _area.WindowPosition.Y; y < _area.WindowPosition.Y + _area.Height; y++)
            {
                Console.SetCursorPosition(_area.WindowPosition.X, y);
                Console.Write(spaces);
            }

            Console.SetCursorPosition(left, top);
        }

//		Position WindowClippedPositionInArea {
//			get {
//				var x = Math.Min (Console.BufferWidth - area.windowPosition.x, area.width);
//				var y = Math.Max (Console.BufferHeight - area.windowPosition.y, area.height);
//				return new Position (x, y);
//			}
//		}

        public static bool IsWindowPositionInArea(Position p)
        {
            return IsLocalPositionInArea(WindowToAreaPosition(p));
        }

        public static bool IsLocalPositionInArea(Position p)
        {
            return p.X < _area.Width && p.X >= 0 && p.Y < _area.Height && p.Y >= 0;
        }

        private static Position AreaToWindowPosition(Position p)
        {
            return new Position(p.X + _area.WindowPosition.X, p.Y + _area.WindowPosition.Y);
        }

        private static Position WindowToAreaPosition(Position p)
        {
            return new Position(p.X - _area.WindowPosition.X, p.Y - _area.WindowPosition.Y);
        }
    }

}


