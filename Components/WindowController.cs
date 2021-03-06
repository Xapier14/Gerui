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
using Gerui.EventHandlers;
using Gerui.Interfaces;

namespace Gerui
{
    public class WindowController
    {
        public List<IComponent> Components { get; private set; }
        public List<Toolbar> Toolbars { get; private set; }

        public IComponent? FocusedComponent { get; set; }
        public IComponent? HoveredComponent { get; internal set; }
        internal object? FocusedComponentData { get; set; }

        public event WindowEventHandler? OnLoad;
        public event WindowEventHandler? OnUpdate;
        public WindowController(SceneInstance scene)
        {
            Debug.Log("[Gerui] Window Controller Created!");
            Components = new List<IComponent>();
            Toolbars = new List<Toolbar>();
            FocusedComponent = null;
            HoveredComponent = null;
            FocusedComponentData = null;
            Game.OnWindowResize += WindowResizeEventHook;
        }

        public void Load(ILayout layout)
        {
            Components.Clear();
            Toolbars.Clear();
            FocusedComponent = null;
            HoveredComponent = null;
            FocusedComponentData = null;
            OnLoad += layout.WindowLoad;
            OnUpdate += layout.Update;
            OnLoad?.Invoke(null, this);
        }

        public void Update()
        {
            HoveredComponent = null;
            foreach (Toolbar toolbar in Toolbars)
            {
                HoveredComponent = toolbar.GetHoveredComponent() ?? HoveredComponent;
            }
            if (FocusedComponent == null)
            {
                foreach (IComponent component in Components)
                {
                    HoveredComponent = component.GetHoveredComponent() ?? HoveredComponent;
                }
            }
            else
            {
                HoveredComponent = FocusedComponent.GetHoveredComponent() ?? HoveredComponent;
            }

            foreach (Toolbar toolbar in Toolbars)
            {
                toolbar.Update(this, this);
            }

            foreach (IComponent component in Components)
            {
                component.Update(this, this);
            }

            if (FocusedComponent != null)
            {
                FocusedComponent.Update(this, this);
            }



            OnUpdate?.Invoke(null, this);
        }

        public void DrawWindow(GraphicsEngine graphics)
        {
            var renderColor = graphics.GetRendererDrawColor();
            // Draw toolbars
            Coord top = new();
            Coord bottom = new(0, graphics.GetInternalResolution().H);
            HoveredComponent = null;
            foreach (Toolbar toolbar in Toolbars)
            {
                if (toolbar == FocusedComponent)
                    continue;
                if (toolbar.Anchor == ToolbarAnchor.Bottom)
                {
                    bottom.Y -= toolbar.Height;
                    toolbar.Draw(graphics, bottom, null);
                } else
                {
                    // assume top anchor
                    toolbar.Draw(graphics, top, null);
                    top.Y += toolbar.Height;
                }
            }

            // Draw drawables
            foreach (IComponent component in Components)
            {
                if (component == FocusedComponent)
                    continue;
                component.Draw(graphics, top, new Coord(graphics.GetInternalResolution().W, bottom.Y));
            }

            // Draw focused component
            if (FocusedComponent != null)
            {
                // draw a dark overlay for the whole window
                Size windowSize = graphics.GetInternalResolution();
                ColorRGBA dimColor = new(6, 8, 10, 180);
                graphics.SetRenderDrawColor(dimColor);
                graphics.DrawRectangleFilled(0, top.Y, windowSize.W, bottom.Y);

                // re-draw our component
                graphics.SetRenderDrawColor(ColorRGBA.WHITE);
                FocusedComponent.Draw(graphics, Coord.Zero, FocusedComponentData);
            }
            graphics.SetRenderDrawColor(renderColor);
        }

        public void WindowResizeEventHook(GameEngineEventArgs eventArg)
        {
            Size newSize = (Size)eventArg.Data;
            Graphics.SetInternalResolution(newSize.W, newSize.H);
            foreach (IComponent component in Components)
            {
                if (component is IWindowReactive)
                {
                    IWindowReactive reactiveComponent = (IWindowReactive) component;
                    reactiveComponent.WindowResizedUpdate(this);
                }
            }
        }
    }
}
