using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GEngine;
using GEngine.Engine;

namespace Gerui
{
    public interface ILabel : IComponent
    {
        public FontResource? Font { get; set; }
        public string Text { get; set; }
        public ColorRGBA ForeColor { get; set; }
        public ColorRGBA BackColor { get; set; }
    }
}
