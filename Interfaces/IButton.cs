using Gerui.EventHandlers;
using Gerui.Interactables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerui
{
    public interface IButton : IComponent
    {
        public event ButtonEventHandler OnClick;
        public event ButtonEventHandler OnMouseOver;
        public event ButtonEventHandler OnMouseLeave;
    }
}
