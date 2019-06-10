using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppPFashions
{
    public class DefaultStyle : DataGridStyle
    {
        public DefaultStyle()
        {
        }

        public override Color GetHeaderBackgroundColor()
        {
            return Color.FromHex("#e0e0e0");
        }

        public override Color GetSelectionBackgroundColor()
        {
            return Color.FromHex("#b2d8f7");
        }

        public override Color GetSelectionForegroundColor()
        {
            return Color.Black;
        }

        public override Color GetRecordForegroundColor()
        {
            return Color.Black;
        }

        public override Color GetCaptionSummaryRowBackgroundColor()
        {
            return Color.FromHex("#e6e6e6");
        }

        public override Color GetRecordBackgroundColor()
        {
            return Color.White;
        }
        public override ImageSource GetGroupCollapseIcon()
        {
            if (Device.RuntimePlatform == Device.macOS)
                return base.GetGroupCollapseIcon();
            return null;
        }

        public override ImageSource GetHeaderSortIndicatorUp()
        {
            return base.GetHeaderSortIndicatorUp();
        }

        public override ImageSource GetHeaderSortIndicatorDown()
        {
            if (Device.RuntimePlatform == Device.macOS)
                return base.GetHeaderSortIndicatorDown();
            return null;
        }
    }

    public class SelectionStyle : DataGridStyle
    {
        public SelectionStyle()
        {
        }

        public override Color GetHeaderBackgroundColor()
        {
            return Color.FromHex("#e0e0e0");
        }

        public override Color GetRecordForegroundColor()
        {
            return Color.Black;
        }

        public override Color GetRecordBackgroundColor()
        {
            return Color.White;
        }
    }

    public class CellTemplateStyle : DataGridStyle
    {
        public override Color GetSelectionBackgroundColor()
        {
            return Color.FromHex("#cce5fa");
        }

        public override Color GetSelectionForegroundColor()
        {
            return Color.Black;
        }

    }
}
