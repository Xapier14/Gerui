using GEngine;
using GEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerui.Components
{
    public class Label : IComponent, ILabel
    {
        public FontResource? Font { get; set; }
        public string Text { get; set; }
        public ColorRGBA ForeColor { get; set; }
        public ColorRGBA BackColor { get; set; }
        public Coord LastDrawnPosition { get; set; }
        public Coord Offset { get; set; }
        public bool Visible { get; set; }

        public Label()
        {
            Text = "";
            Font = null;
            ForeColor = ColorRGBA.BLACK;
            BackColor = ColorRGBA.TRANSPARENT;
            Offset = Coord.Zero;
            Visible = true;
        }

        public void Draw(GraphicsEngine engine, Coord offset, object? data)
        {
            throw new NotImplementedException();
        }

        public IComponent? GetHoveredComponent()
        {
            throw new NotImplementedException();
        }

        public bool IsMouseOver()
        {
            throw new NotImplementedException();
        }

        public void Update(WindowController window, object? data)
        {
            if (!Visible)
                return;
            throw new NotImplementedException();
        }
    }
}
