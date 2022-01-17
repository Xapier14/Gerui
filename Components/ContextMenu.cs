using GEngine;
using GEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gerui.EventHandlers;

using static GEngine.GEngine;

namespace Gerui.Components
{
    public class ContextMenu : IComponent
    {
        public Coord LastDrawnPosition { get; set; }
        public Coord Offset { get; set; }
        public List<ContextMenuItem> Items { get; private set; }
        public Coord Padding { get; set; }
        public ColorRGBA BackColor { get; set; }
        public bool Visible { get; set; }
        public Size Size
        {
            get
            {
                int maxWidth = 0;
                int accHeight = 0;
                foreach (var item in Items)
                {
                    if (item.Size.W > maxWidth)
                        maxWidth = item.Size.W;
                    accHeight += item.Size.H + 1;
                }
                return new Size(maxWidth + Padding.X, accHeight + Padding.Y);
            }
        }
        public Coord? Position { get; set; }

        public ContextMenu()
        {
            Items = new List<ContextMenuItem>();
            Padding = new Coord(4, 4);
            BackColor = ColorRGBA.WHITE;
            Visible = false;
            Position = null;
        }
        public void Draw(GraphicsEngine graphics, Coord offset, object? data)
        {
            Size windowSize = graphics.GetInternalResolution();

            if (Position != null)
            {
                offset = Position.Value;
            }
            LastDrawnPosition = offset;
            if (Visible)
            {
                graphics.SetRenderDrawColor(BackColor);

                int x1 = offset.X, y1 = offset.Y;
                int x2 = x1 + Size.W, y2 = y1 + Size.H;

                // maintain within window
                if (x2 > windowSize.W)
                {
                    x1 -= Size.W;
                    x2 -= Size.W;
                }
                if (y2 > windowSize.H)
                {
                    y1 -= Size.H;
                    y2 -= Size.H;
                }

                graphics.DrawRectangleFilled(x1, y1, x2, y2);

                Coord itemPos = new Coord(x1 + (Padding.X/2), (y1 + Padding.Y/2));
                int width = Size.W - Padding.X;

                foreach (var item in Items)
                {
                    item.Draw(graphics, itemPos, item.Size.W != width ? width : null);
                    itemPos.Y += item.Size.H + 1;
                }
            }
        }

        public IComponent? GetHoveredComponent()
        {
            if (!Visible)
                return null;

            IComponent? comp = IsMouseOver() ? this : null;

            foreach (var item in Items)
            {
                comp = item.GetHoveredComponent() ?? comp;
            }

            return comp;
        }

        public bool IsMouseOver()
        {
            Size windowSize = Graphics.GetInternalResolution();

            int x1 = LastDrawnPosition.X, y1 = LastDrawnPosition.Y;
            int x2 = x1 + Size.W, y2 = y1 + Size.H;
            var cursor = Input.WindowMouse;

            if (x2 > windowSize.W)
            {
                x1 -= Size.W;
                x2 -= Size.W;
            }
            if (y2 > windowSize.H)
            {
                y1 -= Size.H;
                y2 -= Size.H;
            }

            return cursor.X >= x1 && cursor.X <= x2 &&
                   cursor.Y >= y1 && cursor.Y <= y2 && Visible;
        }

        public void Update(WindowController window, object? data)
        {
            if (Visible)
            {
                foreach (var item in Items)
                {
                    item.Update(window, null);
                }
            }
        }
    }
}
