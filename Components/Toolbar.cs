using GEngine;
using GEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static GEngine.GEngine;

namespace Gerui.Components
{
    public abstract class Toolbar : IComponent
    {
        public ColorRGBA BackColor { get; set; }
        public int Height { get; set; }
        public Coord Offset { get; set; }
        public ToolbarAnchor Anchor { get; set; }
        public List<ToolbarItem> Items { get; private set; }
        public ToolbarItemDirection ItemDirection { get; set; }
        public int ItemSpacing { get; set; }
        public Coord LastDrawnPosition { get; set; }

        public Toolbar()
        {
            Anchor = ToolbarAnchor.Top;
            Height = 30;
            BackColor = ColorRGBA.WHITE;
            Offset = new Coord(0, 0);
            Items = new List<ToolbarItem>();
            ItemDirection = ToolbarItemDirection.LeftToRight;
            ItemSpacing = 4; // px
        }
        public bool IsMouseOver()
        {
            Coord cursor = Input.WindowMouse;
            Size windowSize = Graphics.GetInternalResolution();

            // true if cursor is within item bounds
            return cursor.X >= LastDrawnPosition.X && cursor.X <= LastDrawnPosition.X + windowSize.W &&
                   cursor.Y >= LastDrawnPosition.Y && cursor.Y <= LastDrawnPosition.Y + Height;
        }

        public IComponent? GetHoveredComponent()
        {
            IComponent? component = IsMouseOver() ? this : null;
            foreach (ToolbarItem item in Items)
            {
                IComponent? itemComp = item.GetHoveredComponent();
                if (itemComp != null)
                    component = itemComp;
            }
            return component;
        }

        public void Update(WindowController window, object? data)
        {
            foreach (ToolbarItem item in Items)
            {
                item.Update(window, data);
            }
        }
        public virtual void Draw(GraphicsEngine graphics, Coord offset, object? data)
        {
            // get the window size (we are assuming that the internal resolution is set to the window size)
            // this is the default behavior for GEngine-R's AutoResize and HandleResize.
            Size windowSize = graphics.GetInternalResolution();
            LastDrawnPosition = offset;

            // make pos
            int x1 = offset.X + Offset.X, x2 = offset.Y + windowSize.W + Offset.X;
            int y1 = offset.Y + Offset.Y, y2 = offset.Y + Height + Offset.Y;

            // draw rect
            graphics.SetRenderDrawColor(BackColor);
            graphics.DrawRectangleFilled(x1, y1, x2, y2);

            // draw items
            if (Items.Count > 0)
            {
                // determine where the starting coordinate will be depending on toolbar item direction
                // LeftToRight = ItemSpacing
                // RightToLeft = windowSize.W
                // y is set to 0 and should be updated in foreach loop
                Coord itemPos = new(ItemDirection == ToolbarItemDirection.LeftToRight ?
                                    ItemSpacing : windowSize.W,
                                    0);
                foreach (ToolbarItem item in Items)
                {
                    // update y
                    itemPos.Y = ((Height - item.Size.H) / 2) + offset.Y;

                    // update x if RTL
                    itemPos.X -= ItemDirection == ToolbarItemDirection.RightToLeft ?
                                 ItemSpacing + item.Size.W : 0;

                    // draw item
                    item.LastDrawnPosition = itemPos;
                    item.Draw(graphics, itemPos, null);

                    // update x if LTR
                    itemPos.X += ItemDirection == ToolbarItemDirection.LeftToRight ?
                                 ItemSpacing + item.Size.W : 0;
                }
            }
        }
    }
}
