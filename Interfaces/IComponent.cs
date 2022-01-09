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
        public void Update(object? data);
        public bool IsMouseOver();
        public IComponent? GetHoveredComponent();
        internal Coord LastDrawnPosition { get; set; }
    }
}
