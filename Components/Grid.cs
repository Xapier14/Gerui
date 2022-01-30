using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gerui;
using Gerui.Components;
using Gerui.Interactables;
using Gerui.EventHandlers;
using GEngine.Engine;
using GEngine;

using static SDL2.SDL;
using static GEngine.GEngine;

namespace Gerui.Components
{
    public class Grid : IComponent, IWindowReactive
    {
        public HorizontalAnchor HorizontalAnchor { get; set; }
        public VerticalAnchor VerticalAnchor { get; set; }
        public Coord Offset { get; set; }
        public Coord LastDrawnPosition { get; set; }
        public List<IGridable> Items { get; private set; }
        public RowDefinitions Rows { get; private set; }
        public ColumnDefinitions Columns { get; private set; }
        private CellArea _cells { get; set; }
        private Size _Size;
        public Size Size { get => _Size; set { _Size = value; } }
        public bool Visible { get; set; }
        public Grid()
        {
            HorizontalAnchor = HorizontalAnchor.Left;
            VerticalAnchor = VerticalAnchor.Top;
            Rows = new RowDefinitions();
            Columns = new ColumnDefinitions();
            Items = new List<IGridable>();
            _cells = new CellArea();
            Size = Size.Empty;
            Visible = true;
        }

        public void WindowResizedUpdate(WindowController window)
        {

        }

        public void Update(WindowController window, object? data)
        {

        }

        public bool IsMouseOver()
        {
            Coord cursor = Input.WindowMouse;
            return cursor.X >= LastDrawnPosition.X && cursor.Y >= LastDrawnPosition.Y &&
                   cursor.X <= LastDrawnPosition.X + Size.W && cursor.Y <= LastDrawnPosition.Y + Size.H;
        }

        public IComponent? GetHoveredComponent()
        {
            IComponent? hovered = IsMouseOver() ? this : null;
            if (hovered != null)
            {
                foreach (IComponent gridableComponent in Items)
                {
                    hovered = gridableComponent.GetHoveredComponent() ?? hovered;
                }
            }
            return hovered;
        }

        public void Draw(GraphicsEngine engine, Coord offset, object? data)
        {
            LastDrawnPosition = offset;
            if (data is not Coord)
                throw new ArgumentNullException(nameof(data));

            // update size
            Coord brPoint = (Coord)data;
            _Size.W = brPoint.X - offset.X;
            _Size.H = brPoint.Y - offset.Y;
            UpdateCells();
            if (!Visible)
                return;
            // use viewports here
            foreach (IGridable gridableComponent in Items)
            {
                int row = gridableComponent.Row;
                int col = gridableComponent.Column;
                if (row < 0 || col < 0 ||
                    row >= Rows.Count || col >= Columns.Count)
                {
                    Debug.Log($"[Grid] Component {gridableComponent} is out-of-range.");
                    continue;
                }
                try
                {
                    (Coord, Size) viewportData = _cells[row, col];
                    viewportData.Item1 += LastDrawnPosition + Offset;
                    SDL_Rect viewport = new()
                    {
                        x = viewportData.Item1.X,
                        y = viewportData.Item1.Y,
                        w = viewportData.Item2.W,
                        h = viewportData.Item2.H
                    };

                    // adjust for row & col span
                    if (gridableComponent.ColumnSpan > 1)
                    {
                        for (int i = 1; i < gridableComponent.ColumnSpan && i < Columns.Count; i++)
                        {
                            viewport.w += _cells[0, gridableComponent.Column + i].Item2.W;
                        }
                        /*
                        for (int i = gridableComponent.Column; i < i + gridableComponent.ColumnSpan - 1 &&
                            i < Columns.Count - 1; i++)
                        {
                            viewport.w += _cells[0, i + 1].Item2.W;
                        };
                        */
                    }
                    if (gridableComponent.RowSpan > 1)
                    {

                        for (int i = 1; i < gridableComponent.RowSpan && i < Rows.Count; i++)
                        {
                            viewport.h += _cells[gridableComponent.Row + i, 0].Item2.H;
                        }
                        /*
                        for (int i = gridableComponent.Row; i < i + gridableComponent.RowSpan - 1 &&
                            i < Rows.Count - 1; i++)
                        {
                            viewport.h += _cells[i + 1, 0].Item2.H;
                        };
                        */
                    }
                    SDL_RenderSetViewport(engine.Renderer, ref viewport);
                    if (gridableComponent is IDrawable)
                    {
                        IDrawable drawableComponent = (IDrawable)gridableComponent;
                        drawableComponent.Draw(engine, Coord.Zero, (new Size(viewport.w, viewport.h), viewportData.Item1));
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Error drawing...");
                }
            }
            var intRes = engine.GetInternalResolution();
            SDL_Rect fullArea = new SDL_Rect()
            {
                x = 0,
                y = 0,
                w = intRes.W,
                h = intRes.H
            };
            SDL_RenderSetViewport(engine.Renderer, ref fullArea);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Updates cell sizes.
        /// </summary>
        private void UpdateCells()
        {
            // clear cells
            _cells.Clear();

            /* create a list of autosized rows/cols
             * since we need to compute first the
             * fixed size rows/cols to determine the
             * size of the autosized rows/cols
             */
            List<int> autoSizedRows = new();
            List<int> autoSizedColumns = new();
            int[] computedRows = new int[Rows.Count];
            int[] computedColumns = new int[Columns.Count];
            int fixedRowAgg = 0;
            int fixedColAgg = 0;

            // calculate rows
            for (int i = 0; i < Rows.Count; ++i)
            {
                if (Rows[i] != -1)
                {
                    fixedRowAgg += Rows[i];
                    computedRows[i] = Rows[i];
                } else
                {
                    autoSizedRows.Add(i);
                }
            }
            autoSizedRows.Reverse();
            int rowLow = (int)Math.Floor((double)(Size.H - fixedRowAgg) / autoSizedRows.Count);
            int rowHigh = (Size.H - fixedRowAgg) - rowLow * (autoSizedRows.Count - 1);
            if (rowLow > 0 && rowHigh > 0)
            {
                // use low row
                for (int i = 0; i < autoSizedRows.Count - 1; ++i)
                {
                    computedRows[autoSizedRows[i]] = rowLow;
                }
                // use high row for the last row
                computedRows[autoSizedRows[^1]] = rowHigh;
            } else
            {
                Debug.Log("[Grid] Row computation error. Size might be empty.");
            }

            // calculate columns
            for (int i = 0; i < Columns.Count; ++i)
            {
                if (Columns[i] != -1)
                {
                    fixedColAgg += Columns[i];
                    computedColumns[i] = Columns[i];
                }
                else
                {
                    autoSizedColumns.Add(i);
                }
            }
            autoSizedColumns.Reverse();
            int columnLow = (int)Math.Floor((double)(Size.W - fixedColAgg) / autoSizedColumns.Count);
            int columnHigh = (Size.W - fixedColAgg) - columnLow * (autoSizedColumns.Count - 1);
            if (columnLow > 0 && columnHigh > 0)
            {
                // use low column
                for (int i = 0; i < autoSizedRows.Count - 1; ++i)
                {
                    computedColumns[autoSizedColumns[i]] = columnLow;
                }
                // use high column for the last column
                computedColumns[autoSizedRows[^1]] = columnHigh;
            }
            else
            {
                Debug.Log("[Grid] Column computation error. Size might be empty.");
            }

            for (int row = 0; row < computedRows.Length; ++row)
            {
                Coord rowStartPos = new(LastDrawnPosition.X, GetRowYOffset(computedRows, row));
                for (int column = 0; column < computedColumns.Length; ++column)
                {
                    Size size = new(computedColumns[column], computedRows[row]);
                    Coord position = new(rowStartPos);
                    position.X += GetColumnXOffset(computedColumns, column);

                    _cells[row, column] = (position, size);
                }
            }

            //Debug.Log("[Grid] Updated cell area data.");
        }

        private int GetRowYOffset(int[] computedRows, int index)
        {
            int aggregate = 0;
            for (int i = 0; i < index; ++i)
                aggregate+= computedRows[i];
            return aggregate;
        }
        private int GetColumnXOffset(int[] computedColumns, int index)
        {
            int aggregate = 0;
            for (int i = 0; i < index; ++i)
                aggregate += computedColumns[i];
            return aggregate;
        }
    }
}
