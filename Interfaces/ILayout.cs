using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerui.Interfaces
{
    public interface ILayout
    {
        public void InitializeComponents(WindowController window);
        public void WindowLoad(object? sender, WindowController window);
        public void Update(object? sender, WindowController window);
    }
}
