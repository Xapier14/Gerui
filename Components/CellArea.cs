using GEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerui.Components
{
    public class CellArea
    {
        // [r,c] -> ([x,y], [w,h])
        private Dictionary<Coord, (Coord, Size)> _cells;
        public CellArea()
        {
            _cells = new Dictionary<Coord, (Coord, Size)>();
        }
        public (Coord, Size) this[int row, int column]
        {
            get
            {
                if (!_cells.ContainsKey(new Coord(row, column)))
                    throw new ArgumentOutOfRangeException("Row/Column index is out-of-range.");
                return _cells[new Coord(row,column)];
            }
            set
            {
                if (_cells.ContainsKey(new Coord(row, column)))
                    _cells.Remove(new Coord(row, column));
                _cells.Add(new Coord(row, column), value);
            }
        }
        public void Clear()
        {
            _cells.Clear();
        }
    }
}
