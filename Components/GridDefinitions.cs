using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerui.Components
{
    public class RowDefinitions
    {
        private List<int> _rows;
        public int Count {  get => _rows.Count; }
        internal RowDefinitions()
        {
            _rows = new List<int>();
            Add(-1);
        }

        public int this[int rowIndex]
        {
            get
            {
                if (rowIndex < 0 || rowIndex >= _rows.Count)
                    throw new ArgumentOutOfRangeException("Row index is out-of-range.");
                return _rows[rowIndex];
            }
            set
            {
                if (rowIndex < 0 || rowIndex >= _rows.Count)
                    throw new ArgumentOutOfRangeException("Row index is out-of-range.");
                _rows[rowIndex] = value >= -1 ? value : -1;
            }
        }
        public void Add(int rowDefinition)
        {
            _rows.Add(rowDefinition);
        }
        public void Remove()
        {
            if (_rows.Count == 0)
                return;
            Remove(_rows.Count - 1);
        }
        public void Remove(int index)
        {
            if (index < 0 || index >= _rows.Count)
                throw new ArgumentOutOfRangeException("Index is out-of-range.");
            _rows.Remove(index);
        }
        public void Reset()
        {
            _rows.Clear();
            Add(-1);
        }
        public void Reset(int initial)
        {
            _rows.Clear();
            Add(initial);
        }
    }
    public class ColumnDefinitions
    {
        private List<int> _columns;
        public int Count { get => _columns.Count; }

        internal ColumnDefinitions()
        {
            _columns = new List<int>();
            Add(-1);
        }

        public int this[int columnIndex]
        {
            get
            {
                if (columnIndex < 0 || columnIndex >= _columns.Count)
                    throw new ArgumentOutOfRangeException("Column index is out-of-range.");
                return _columns[columnIndex];
            }
            set
            {
                if (columnIndex < 0 || columnIndex >= _columns.Count)
                    throw new ArgumentOutOfRangeException("Column index is out-of-range.");
                _columns[columnIndex] = value >= -1 ? value : -1;
            }
        }
        public void Add(int columnDefinition)
        {
            _columns.Add(columnDefinition);
        }
        public void Remove()
        {
            if (_columns.Count == 0)
                return;
            Remove(_columns.Count - 1);
        }
        public void Remove(int index)
        {
            if (index < 0 || index >= _columns.Count)
                throw new ArgumentOutOfRangeException("Index is out-of-range.");
            _columns.Remove(index);
        }
        public void Reset()
        {
            _columns.Clear();
            Add(-1);
        }
        public void Reset(int initial)
        {
            _columns.Clear();
            Add(initial);
        }
    }
}
