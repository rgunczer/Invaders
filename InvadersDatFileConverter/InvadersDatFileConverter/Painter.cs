using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvadersDatFileConverter
{    
    public sealed class Painter
    {
        private enum DrawKind
        {
            None,
            Title,
            Player,
            Enemy,
            PlayerLive
        };

        private DrawKind _drawKind = DrawKind.None;
        private List<byte> _currentData;
        private int _XOffset = 200;

        public Painter(int xOffset)
        {
            _XOffset = xOffset;
            Scale = 1;
        }

        public int Scale { get; set; }

        public void ReDraw(Graphics g)
        {
            switch(_drawKind)
            {
                case DrawKind.None:
                    break;

                case DrawKind.Enemy:
                    DrawEnemy(g, _currentData);
                    break;

                case DrawKind.Player:
                    DrawPlayer(g, _currentData);
                    break;

                case DrawKind.PlayerLive:
                    DrawPlayer(g, _currentData);
                    break;

                case DrawKind.Title:
                    DrawTitle(g, _currentData);
                    break;
            }
        }

        public void DrawTitle(Graphics g, List<byte> data)
        {
            _currentData = data;
            _drawKind = DrawKind.Title;

            g.Clear(SystemColors.Control);
            Brush brush = new SolidBrush(Color.Gray);
            int x = _XOffset;
            int y = 0;
            int scale = Scale;
            int pos = 0;
            int length = data.Count;
            while (pos < length)
            {
                byte b1 = data[pos];

                ++pos;

                y += scale;

                if (b1 == 11)
                {
                    g.FillRectangle(brush, x, y, scale, scale);
                }

                if (b1 == 200)
                {
                    y = 0;
                    x += scale;
                }
            }
        }

        public void DrawPlayer(Graphics g, List<byte> data)
        {
            _currentData = data;
            _drawKind = DrawKind.Player;

            g.Clear(SystemColors.Control);
            Brush brush = new SolidBrush(Color.Gray);
            int x = _XOffset;
            int y = 0;
            int scale = Scale;
            int pos = 0;
            int length = data.Count;
            while (pos < length)
            {
                byte b1 = data[pos];

                ++pos;

                y += scale;

                if (b1 == 15)
                {
                    g.FillRectangle(brush, x, y, scale, scale);
                }

                if (b1 == 200)
                {
                    y = 0;
                    x += scale;
                }
            }
        }

        public void DrawEnemy(Graphics g, List<byte> data)
        {
            _currentData = data;
            _drawKind = DrawKind.Enemy;

            g.Clear(SystemColors.Control);
            Brush brush = new SolidBrush(Color.Gray);
            int x = _XOffset;
            int y = 0;
            int scale = Scale;
            int pos = 0;
            int length = data.Count;
            while (pos < length)
            {
                byte b1 = data[pos];

                ++pos;

                y += scale;

                if (b1 != 0 && b1 != 200 && b1 != 255)
                {
                    g.FillRectangle(brush, x, y, scale, scale);
                }

                if (b1 == 200)
                {
                    y = 0;
                    x += scale;
                }
            }

        }


    }
}
