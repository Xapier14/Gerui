using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GEngine;
using GEngine.Engine;

namespace Gerui
{
    public interface IDrawable
    {
        public Coord Offset { get; internal set; }
        public void Draw(GraphicsEngine engine, Coord offset, object? data);
    }
}
