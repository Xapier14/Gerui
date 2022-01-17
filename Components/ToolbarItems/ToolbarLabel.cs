using GEngine;
using GEngine.Engine;
using static GEngine.GEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gerui;

namespace Gerui.Components
{
    public class ToolbarLabel : ToolbarItem, ILabel
    {
        public override Size Size
        {
            get
            {
                if (Text == string.Empty || Font == null)
                    return Size.Empty;
                var size = Graphics.MeasureText(Font, Text);
                return new Size(size.W + Padding.X * 2, size.H + Padding.Y * 2);
            }
        }

        public override Coord Padding { get; set; }
        public override ColorRGBA BackColor { get; set; }
        public ColorRGBA ForeColor { get; set; }
        public FontResource? Font { get; set; }
        public string Text { get; set; }
        
        public ToolbarLabel()
        {
            Font = null;
            Text = string.Empty;
            BackColor = ColorRGBA.TRANSPARENT;
            ForeColor = ColorRGBA.BLACK;
            Padding = new(2, 2);
        }

        public override void Draw(GraphicsEngine graphics, Coord offset, object? data)
        {
            int x1 = offset.X, y1 = offset.Y;
            int x2 = x1 + Size.W, y2 = y1 + Size.H;
            graphics.SetRenderDrawColor(BackColor);
            graphics.DrawRectangleFilled(x1, y1, x2, y2);

            if (Text != string.Empty && Font != null)
            {
                graphics.SetRenderDrawColor(ForeColor);
                graphics.DrawText(Font, Text, x1 + Size.W / 2, y1 + Size.H / 2,
                    hAlignment: TextHorizontalAlign.Center,
                    vAlignment: TextVerticalAlign.Middle);
            }
        }

        public override void Update(WindowController window, object? data)
        {

        }
    }
}
