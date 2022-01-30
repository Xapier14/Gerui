using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gerui.Components;

namespace Gerui
{
    public interface IGridable
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; }
        public int ColumnSpan { get; set; }
        public HorizontalAnchor HorizontalAnchor { get; set; }
        public VerticalAnchor VerticalAnchor { get; set; }
    }
}
