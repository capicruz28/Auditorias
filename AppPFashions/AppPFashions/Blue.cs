using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppPFashions
{
    public class Blue : DataGridStyle
    {
        public Blue()
        {
        }

        public override Color GetHeaderBackgroundColor()
        {
            return Color.FromRgb(89, 140, 181);
        }

        public override Color GetHeaderForegroundColor()
        {
            return Color.FromRgb(255, 255, 255);
        }

        public override Color GetSelectionBackgroundColor()
        {
            return Color.FromRgb(100, 181, 246);
        }

        public override Color GetSelectionForegroundColor()
        {
            return Color.FromRgb(255, 255, 255);
        }

        public override Color GetCaptionSummaryRowBackgroundColor()
        {
            return Color.FromRgb(224, 224, 224);
        }

        public override Color GetCaptionSummaryRowForegroundColor()
        {
            return Color.FromRgb(51, 51, 51);
        }

        public override Color GetBorderColor()
        {
            return Color.FromRgb(180, 180, 180);
        }

        public override Color GetAlternatingRowBackgroundColor()
        {
            return Color.FromRgb(227, 242, 253);
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
