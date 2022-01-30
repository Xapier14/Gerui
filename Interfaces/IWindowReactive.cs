using Gerui.EventHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerui
{
    public interface IWindowReactive
    {
        internal void WindowResizedUpdate(WindowController window);
    }
}
