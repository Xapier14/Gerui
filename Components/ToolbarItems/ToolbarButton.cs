using GEngine;
using GEngine.Engine;
using static GEngine.GEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gerui.EventHandlers;
using Gerui.Interactables;

namespace Gerui.Components
{
    public class ToolbarButton : ToolbarItem, IButton
    {
        public string Text { get; set; }
        public FontResource? Font { get; set; }
        public override ColorRGBA BackColor { get; set; }
        public ColorRGBA ForeColor { get; set; }
        public ColorRGBA BorderColor { get; set; }
        public ColorRGBA HighlightColor { get; set; }
        public ColorRGBA HighlightBorderColor { get; set; }
        public override Size Size { get
            {
                if (Text == string.Empty || Font == null)
                    return Size.Empty;
                var size = Graphics.MeasureText(Font, Text);
                return new Size(size.W + Padding.X * 2, size.H + Padding.Y * 2);
            }
        }
        public override bool Visible { get; set; }
        public override Coord Padding { get; set; }
        public bool Highlight { get; set; }
        public TextHorizontalAlign TextHorizontalAlign { get; set; }
        public TextVerticalAlign TextVerticalAlign { get; set; }

        public event ButtonEventHandler? OnClick;
        public event ButtonEventHandler? OnMouseOver;
        public event ButtonEventHandler? OnMouseLeave;

        private bool _lastHovered, _wasPressed;

        public ToolbarButton()
        {
            Text = "Button";
            BackColor = ColorRGBA.TRANSPARENT;
            ForeColor = ColorRGBA.BLACK;
            HighlightColor = new ColorRGBA(240, 240, 240, 40);
            HighlightBorderColor = ColorRGBA.TRANSPARENT;
            BorderColor = BackColor;
            Padding = new(4, 4);
            Highlight = false;
            LastDrawnPosition = Coord.Zero;
            _lastHovered = false;
            _wasPressed = false;
            Visible = true;
            TextHorizontalAlign = TextHorizontalAlign.Center;
            TextVerticalAlign = TextVerticalAlign.Middle;
        }

        public override void Update(WindowController window, object? data)
        {
            bool hovering = window.HoveredComponent == this;
            Highlight = hovering;
            if (hovering)
            {
                // started hovering
                if (!_lastHovered)
                    OnMouseOver?.Invoke(this, new ButtonEventArgs());

                // check if lmb press event happened while hovering
                if (!_wasPressed && Input.MouseLeftButtonDownPressed)
                {
                    _wasPressed = true;
                }

                // if released while press event is on button, raise OnClick.
                if (_wasPressed && Input.MouseLeftButtonDownReleased)
                {
                    _wasPressed = false;
                    OnClick?.Invoke(this, new ButtonEventArgs());
                }
            }
            else
            {
                // if mouse left while lmb press was started on button, reset lmb press state
                if (_wasPressed)
                    _wasPressed = false;

                // ended hovering
                if (_lastHovered)
                    OnMouseLeave?.Invoke(this, new ButtonEventArgs());
            }
            _lastHovered = hovering;
        }

        public override void Draw(GraphicsEngine graphics, Coord offset, object? data)
        {
            LastDrawnPosition = new(offset);

            // skip draw if invisible
            if (!Visible)
                return;

            // get rectangle coords
            int x1 = offset.X + Offset.X, y1 = offset.Y + Offset.Y;
            int x2 = x1 + Size.W + Offset.X, y2 = y1 + Size.H + Offset.Y;

            // draw button back
            graphics.SetRenderDrawColor(BackColor);
            graphics.DrawRectangleFilled(x1, y1, x2, y2);

            // draw button border
            graphics.DrawRectangle(x1, y2, x2, y2);

            // draw text if text and font is not empty/null
            if (Text != string.Empty && Font != null)
            {
                graphics.SetRenderDrawColor(ForeColor);
                graphics.DrawText(Font, Text, x1 + Size.W / 2, y1 + Size.H / 2,
                    hAlignment: TextHorizontalAlign,
                    vAlignment: TextVerticalAlign);
            }

            // draw highlight if set
            if (Highlight)
            {
                graphics.SetRenderDrawColor(HighlightColor);
                graphics.DrawRectangleFilled(x1, y1, x2, y2);
                graphics.SetRenderDrawColor(HighlightBorderColor);
                graphics.DrawRectangle(x1, y1, x2, y2);
            }
        }
    }
}
