using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachhCityManagementTool.Classes
{
    public class ColumnConfig<T>
    {
        public string Header { get; set; } // Column header text
        public Func<T, object> Selector { get; set; } // How to extract value from a model instance
        public double Width { get; set; } = 15; // Column width
        public Action<IXLCell, object, T> StyleAction { get; set; } = null; // Optional style formatter
        public bool IsVisible { get; set; } = true; // Whether the column should be visible
    }

}
