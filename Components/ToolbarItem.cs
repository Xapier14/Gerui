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
    public abstract class ToolbarItem : IComponent
    {
        public abstract Size Size { get; }
        public Coord Offset { get; set; }
        public abstract Coord Padding { get; set; }
        public abstract ColorRGBA BackColor { get; set; }
        public abstract void Draw(GraphicsEngine graphics, Coord offset, object? data);
        public abstract void Update(WindowController window, object? data);
        public Coord LastDrawnPosition { get; set; }
        public bool IsMouseOver()
        {
            Coord cursor = Input.WindowMouse;

            // true if cursor is within item bounds
            return cursor.X >= LastDrawnPosition.X && cursor.X <= LastDrawnPosition.X + Size.W &&
                   cursor.Y >= LastDrawnPosition.Y && cursor.Y <= LastDrawnPosition.Y + Size.H;
        }
        public IComponent? GetHoveredComponent()
        {
            return IsMouseOver() ? this : null;
        }
    }
}
