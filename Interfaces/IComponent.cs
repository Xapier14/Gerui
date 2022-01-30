using GEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerui
{
    public interface IComponent : IDrawable
    {
        public void Update(WindowController window, object? data);
        public bool IsMouseOver();
        public IComponent? GetHoveredComponent();
        public bool Visible { get; set; }
        internal Coord LastDrawnPosition { get; set; }
    }
}
