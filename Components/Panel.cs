using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GEngine;
using GEngine.Engine;
using Gerui;

using static GEngine.GEngine;

namespace Gerui.Components
{
    public class Panel : IComponent, IGridable
    {
        public Coord LastDrawnPosition { get; set; }
        public Coord Offset { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; }
        public int ColumnSpan { get; set; }
        public ColorRGBA BackColor { get; set; }
        public ColorRGBA BorderColor { get; set; }
        public HorizontalAnchor HorizontalAnchor { get; set; }
        public VerticalAnchor VerticalAnchor { get; set; }
        public Size Size { get; set; }
        public bool Visible { get; set; }
        public Panel()
        {
            Row = 0;
            Column = 0;
            RowSpan = 1;
            ColumnSpan = 1;
            BackColor = ColorRGBA.TRANSPARENT;
            BorderColor = ColorRGBA.TRANSPARENT;
            HorizontalAnchor = HorizontalAnchor.Left;
            VerticalAnchor = VerticalAnchor.Top;
            Size = new Size(100, 100);
            Visible = true;
        }

        public void Draw(GraphicsEngine engine, Coord offset, object? data)
        {
            Size size;
            Coord drawPosition = new(offset);
            LastDrawnPosition = new(offset);
            if (data is not GEngine.Size && data is not (GEngine.Size, Coord))
            {
                size = engine.GetInternalResolution();
            } else
            {
                if (data is Coord)
                {
                    size = (Size)data;
                } else
                {
                    size = (((Size, Coord))data).Item1;
                    LastDrawnPosition += (((Size, Coord))data).Item2;
                }
                Size = new(size);
            }
            if (!Visible)
                return;

            if (HorizontalAnchor == HorizontalAnchor.Right)
            {
                drawPosition.X -= Offset.X;
            }
            else
            {
                drawPosition.X += Offset.X;
            }
            if (VerticalAnchor == VerticalAnchor.Bottom)
            {
                drawPosition.Y -= Offset.Y;
            }
            else
            {
                drawPosition.Y += Offset.Y;
            }

            engine.SetRenderDrawColor(BackColor);
            engine.DrawRectangleFilled(drawPosition.X, drawPosition.Y,
                                       drawPosition.X + size.W, drawPosition.Y + size.H);
            engine.SetRenderDrawColor(BorderColor);
            engine.DrawRectangle(drawPosition.X, drawPosition.Y,
                                       drawPosition.X + size.W, drawPosition.Y + size.H);
            engine.SetRenderDrawColor(ColorRGBA.WHITE);
            engine.DrawText(Resources.GetFontResource("fnt_ui"), "This is a panel", 4, 4);
        }

        public IComponent? GetHoveredComponent()
        {
            return IsMouseOver() ? this : null;
        }

        public bool IsMouseOver()
        {
            Coord cursor = Input.WindowMouse;
            return cursor.X >= LastDrawnPosition.X && cursor.Y >= LastDrawnPosition.Y &&
                   cursor.X <= LastDrawnPosition.X + Size.W && cursor.Y <= LastDrawnPosition.Y + Size.H;
        }

        public void Update(WindowController window, object? data)
        {

        }
    }
}
