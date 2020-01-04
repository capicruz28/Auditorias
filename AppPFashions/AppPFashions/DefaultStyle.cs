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
            return Color.FromHex("#446EA6");
        }

        //public override Color GetHeaderForegroundColor()
        //{
        //    return Color.FromHex("#728EA6");
        //}

        public override Color GetSelectionBackgroundColor()
        {
            return Color.FromHex("#FDEDEC");
        }

        public override Color GetSelectionForegroundColor()
        {
            //return Color.DimGray;
            return Color.FromHex("#4D5656");
        }

        public override Color GetRecordForegroundColor()
        {
            return Color.Black;
        }

        public override Color GetCaptionSummaryRowBackgroundColor()
        {
            return Color.FromHex("#F0F8FF");
        }

        public override Color GetCaptionSummaryRowForegroundColor()
        {
            return Color.Black;
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
            return Color.FromHex("#DBDCD6");
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
            return Color.FromHex("#A3C9D3");
        }

        public override Color GetSelectionForegroundColor()
        {
            return Color.Black;
        }

    }
}
