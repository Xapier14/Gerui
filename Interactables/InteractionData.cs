using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerui.Interactables
{
    public struct InteractionData
    {
        public object? Data { get; private set; }

        public InteractionData(object? data = null)
        {
            Data = data;
        }
    }
}
