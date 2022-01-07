using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GEngine;
using GEngine.Game;
using GEngine.Engine;

using static GEngine.GEngine;

using Gerui.Components;

namespace Gerui
{
    public class WindowController
    {
        public List<IDrawable> Drawables { get; private set; }
        public List<Toolbar> Toolbars { get; private set; }
        public List<IDrawable> Modals { get; private set; }
        public WindowController(SceneInstance scene)
        {
            Console.WriteLine("Panel Controller Created!");
            Drawables = new List<IDrawable>();
            Toolbars = new List<Toolbar>();
            Modals = new List<IDrawable>();
        }
        public void DrawWindow(GraphicsEngine graphics)
        {
            var renderColor = graphics.GetRendererDrawColor();
            // Draw toolbars
            int topY = 0;
            int bottomY = graphics.GetInternalResolution().H;
            foreach (Toolbar toolbar in Toolbars)
            {
                if (toolbar.Anchor == ToolbarAnchor.Bottom)
                {
                    toolbar.Draw(graphics, bottomY - toolbar.Height);
                    bottomY -= toolbar.Height;
                } else
                {
                    // assume top anchor
                    toolbar.Draw(graphics, topY);
                    topY += toolbar.Height;
                }
            }

            // Draw drawables
            foreach (IDrawable drawable in Drawables)
            {
                drawable.Draw(graphics);
            }

            // Draw modals
            foreach (IDrawable modal in Modals)
            {
                modal.Draw(graphics);
            }
            graphics.SetRenderDrawColor(renderColor);
        }
    }
}
