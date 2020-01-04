using Syncfusion.Data;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AppPFashions
{
    public class TableSummaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value != null ? value as SummaryRecordEntry : null;
            if (data != null)
            {
                SfDataGrid dataGrid = (SfDataGrid)parameter;
                var summaryText = SummaryCreator.GetSummaryDisplayText(data, "qprogr01", dataGrid.View);

                return summaryText.ToString();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
