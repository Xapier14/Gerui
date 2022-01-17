using GEngine;
using GEngine.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerui.Components
{
    public abstract class ContextMenuItem : IComponent
    {
        public Coord LastDrawnPosition { get; set; }
        public Coord Offset { get; set; }
        public abstract Size Size { get; }
        public abstract void Draw(GraphicsEngine graphics, Coord offset, object? data);

        public abstract IComponent? GetHoveredComponent();

        public abstract bool IsMouseOver();

        public abstract void Update(WindowController window, object? data);
    }
}
