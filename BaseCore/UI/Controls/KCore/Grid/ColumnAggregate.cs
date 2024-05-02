using System;
using System.Collections.Generic;
using System.Text;

namespace BaseCore.UI.Controls.KCore.Grid
{
    [Flags]
    public enum ColumnAggregate
    {
        Sum = 1,
        Count = 2,
        Average = 4,
        Min = 8,
        Max = 16
    }
}
