using GEngine;
using GEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerui.Components
{
    public abstract class Toolbar : IDrawable
    {
        public ColorRGBA BackColor { get; set; }
        public int Height { get; set; }
        public Coord Offset { get; set; }
        public ToolbarAnchor Anchor { get; set; }

        public Toolbar()
        {
            Anchor = ToolbarAnchor.Top;
            Height = 30;
            BackColor = ColorRGBA.WHITE;
            Offset = new Coord(0, 0);
        }
        public virtual void Draw(GraphicsEngine graphics)
            => Draw(graphics, 0);
        public virtual void Draw(GraphicsEngine graphics, int y)
        {
            Size windowSize = graphics.GetInternalResolution();

            // make pos
            int x1 = 0, x2 = windowSize.W;
            int y1 = y, y2 = y + Height;

            // draw rect
            graphics.SetRenderDrawColor(BackColor);
            graphics.DrawRectangleFilled(x1, y1, x2, y2);
        }
    }
}
