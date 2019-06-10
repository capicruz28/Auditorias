using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppPFashions
{
    public class Purple : DataGridStyle
    {
        public Purple()
        {
        }

        public override Color GetHeaderBackgroundColor()
        {
            return Color.FromHex("#83538B");
        }

        public override Color GetHeaderForegroundColor()
        {
            return Color.FromRgb(255, 255, 255);
        }

        public override Color GetSelectionBackgroundColor()
        {
            return Color.FromRgb(149, 117, 205);
        }

        public override Color GetSelectionForegroundColor()
        {
            return Color.FromRgb(255, 255, 255);
        }

        public override Color GetAlternatingRowBackgroundColor()
        {
            return Color.FromRgb(237, 231, 246);
        }

        public override Color GetRecordForegroundColor()
        {
            return Color.FromRgb(0, 0, 0);
        }

        public override GridLinesVisibility GetGridLinesVisibility()
        {
            return GridLinesVisibility.Horizontal;
        }
    }
}
