using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GEngine;

namespace Gerui
{
    public interface IPanel : IDrawable, IComponent
    {
        public bool HasFocus { get; internal set; }
        public Size Area { get; internal set; }
        public ColorRGBA BackColor { get; internal set; }

    }
}
