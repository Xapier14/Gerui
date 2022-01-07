using Gerui.Interactables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerui.Interfaces
{
    public interface IInteractable
    {
        public void OnClick(object sender, InteractionData data);
        public void OnHover(object sender, InteractionData data);
        public void OnLeave(object sender, InteractionData data);
    }
}
