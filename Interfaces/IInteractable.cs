using Gerui.Interactables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerui
{
    public interface IInteractable : IComponent
    {
        public void OnClick(object sender, InteractionData data);
        public void OnHover(object sender, InteractionData data);
        public void OnLeave(object sender, InteractionData data);
    }
}
