using GEngine;
using GEngine.Engine;
using Gerui.EventHandlers;
using Gerui.Interactables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static GEngine.GEngine;

namespace Gerui.Components
{
    public class ContextMenuButton : ContextMenuItem, IButton
    {
        public FontResource? Font { get; set; }
        public string Text { get; set; }
        public ColorRGBA ForeColor { get; set; }
        public ColorRGBA BackColor { get; set; }
        public ColorRGBA BorderColor { get; set; }
        public ColorRGBA HighlightColor { get; set; }
        public ColorRGBA HighlightBorderColor { get; set; }
        public bool Highlight { get; set; }

        public event ButtonEventHandler? OnClick;
        public event ButtonEventHandler? OnMouseOver;
        public event ButtonEventHandler? OnMouseLeave;

        public Coord Padding { get; set; }

        public override Size Size
        {
            get
            {
                if (Text == string.Empty || Font == null)
                    return Size.Empty;
                var size = Graphics.MeasureText(Font, Text);
                return new Size(size.W + Padding.X * 2, size.H + Padding.Y * 2);
            }
        }
        public override bool Visible { get; set; }

        private bool _hovered = false;
        private bool _wasPressed = false;
        private int _minWidth = -1;

        public ContextMenuButton()
        {
            Font = null;
            Text = string.Empty;
            ForeColor = ColorRGBA.BLACK;
            BackColor = ColorRGBA.TRANSPARENT;
            BorderColor = BackColor;
            HighlightColor = new ColorRGBA(240, 240, 240, 40);
            HighlightBorderColor = ColorRGBA.TRANSPARENT;
            Highlight = false;
            Padding = new Coord(4, 4);
            Visible = true;
        }

        public override void Draw(GraphicsEngine graphics, Coord offset, object? data)
        {
            LastDrawnPosition = offset;

            int minWidth = -1;
            if (data is int)
            {
                minWidth = (int)data;
                _minWidth = minWidth;
            }

            if (!Visible)
                return;

            int x1 = offset.X + Offset.X, y1 = offset.Y + Offset.Y;
            int x2 = x1 + (minWidth > 0 ? minWidth : Size.W), y2 = y1 + Size.H;
            int textX = x1 + Padding.X, textY = y1 + (Size.H / 2);
            
            graphics.SetRenderDrawColor(BackColor);
            graphics.DrawRectangleFilled(x1, y1, x2, y2);
            graphics.SetRenderDrawColor(BorderColor);
            graphics.DrawRectangle(x1, y1, x2, y2);

            if (Font != null && Text != string.Empty)
            {
                graphics.SetRenderDrawColor(ForeColor);
                graphics.DrawText(Font, Text, textX, textY, hAlignment: TextHorizontalAlign.Left, vAlignment: TextVerticalAlign.Middle);
            }

            if (Highlight)
            {
                graphics.SetRenderDrawColor(HighlightColor);
                graphics.DrawRectangleFilled(x1, y1, x2, y2);
                graphics.SetRenderDrawColor(HighlightBorderColor);
                graphics.DrawRectangle(x1, y1, x2, y2);
            }
        }

        public override IComponent? GetHoveredComponent()
        {
            return IsMouseOver() ? this : null;
        }

        public override bool IsMouseOver()
        {
            Coord cursor = Input.WindowMouse;
            return cursor.X >= LastDrawnPosition.X && cursor.X <= LastDrawnPosition.X + (_minWidth > 0 ? _minWidth : Size.W) &&
                   cursor.Y >= LastDrawnPosition.Y && cursor.Y <= LastDrawnPosition.Y + Size.H;
        }

        public override void Update(WindowController window, object? data)
        {
            // events
            if (window.HoveredComponent == this)
            {
                if (!_hovered)
                {
                    Highlight = _hovered = true;
                    OnMouseOver?.Invoke(this, new ButtonEventArgs()
                    {

                    });
                }

                if (!_wasPressed && Input.MouseLeftButtonDownPressed)
                {
                    _wasPressed = true;
                }

                if (_wasPressed && Input.MouseLeftButtonDownReleased)
                {
                    _wasPressed = false;
                    OnClick?.Invoke(this, new ButtonEventArgs());
                }
            }
            else
            {
                if (_hovered)
                {
                    Highlight = _hovered = _wasPressed = false;
                    OnMouseLeave?.Invoke(this, new ButtonEventArgs()
                    {

                    });
                }
            }
        }
    }
}
