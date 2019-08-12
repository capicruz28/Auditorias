namespace AppPFashions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Syncfusion.SfDataGrid.XForms;
    using Xamarin.Forms;

    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]

    /// <summary>
    ///  Base generic class for generalized user-defined behaviors that can respond to arbitrary conditions and events.
    ///  Type parameters:T: The type of the objects with which this Forms.Behavior`1 can be associated in the DiagonalScrolling samples
    /// </summary>
    public class DiagonalScrollingBehaviors : Behavior<Syncfusion.SfDataGrid.XForms.SfDataGrid>
    {
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.Private field does not need documentation")]
        private Syncfusion.SfDataGrid.XForms.SfDataGrid dataGrid;

        /// <summary>
        /// You can override this method to subscribe to AssociatedObject events and initialize properties.
        /// </summary>
        /// <param name="bindAble">DataGrid type of bindAble parameter</param>
        protected override void OnAttachedTo(Syncfusion.SfDataGrid.XForms.SfDataGrid bindAble)
        {
            //this.dataGrid = bindAble;
            //if (Device.Idiom == TargetIdiom.Phone)
            //{
            //    this.dataGrid.DefaultColumnWidth = 120;
            //}
            //else
            //{
            //    this.dataGrid.DefaultColumnWidth = 160;
            //}

            //this.dataGrid.AutoGeneratingColumn += this.DataGrid_AutoGeneratingColumn;
            //base.OnAttachedTo(bindAble);
        }

        /// <summary>
        /// You can override this method while View was detached from window
        /// </summary>
        /// <param name="bindAble">DataGrid type of bindAble parameter</param>
        protected override void OnDetachingFrom(Syncfusion.SfDataGrid.XForms.SfDataGrid bindAble)
        {
            this.dataGrid.AutoGeneratingColumn -= this.DataGrid_AutoGeneratingColumn;
            this.dataGrid = null;
            base.OnDetachingFrom(bindAble);
        }

        /// <summary>
        /// Fired when column is generated in DataGrid
        /// </summary>
        /// <param name="sender">DataGrid_AutoGeneratingColumn sender</param>
        /// <param name="e">DataGrid_AutoGeneratingColumn event args e</param>
        private void DataGrid_AutoGeneratingColumn(object sender, AutoGeneratingColumnEventArgs e)
        {
            e.Column.HeaderFontAttribute = FontAttributes.Bold;
            e.Column.Padding = new Thickness(5, 0, 5, 0);
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    e.Column.HeaderCellTextSize = 14;
                    break;
                case Device.iOS:
                    e.Column.HeaderCellTextSize = 12;
                    break;
                case Device.UWP:
                    e.Column.HeaderCellTextSize = 12;
                    break;
            }

            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    e.Column.CellTextSize = 14;
                    break;
                case Device.iOS:
                    e.Column.CellTextSize = 12;
                    break;
                case Device.UWP:
                    e.Column.CellTextSize = 12;
                    break;
            }

            e.Column.HeaderTextAlignment = TextAlignment.Start;

            if (e.Column.MappingName == "clinea")
            {
                e.Column.TextAlignment = TextAlignment.End;
                e.Column.HeaderText = "LINEA";
                
            }
            else if (e.Column.MappingName == "ctraba")
            {
                e.Column.TextAlignment = TextAlignment.Start;
                e.Column.HeaderText = "CODIGO";                
            }
            else if (e.Column.MappingName == "dtraba")
            {
                e.Column.TextAlignment = TextAlignment.Start;
                e.Column.HeaderText = "TRABAJADOR";                
            }
            else if (e.Column.MappingName == "qmindi01")
            {
                e.Column.TextAlignment = TextAlignment.Start;
                e.Column.HeaderText = "M.D.";               
            }
            else if (e.Column.MappingName == "pefici01")
            {
                e.Column.TextAlignment = TextAlignment.Center;
                e.Column.HeaderText = "% EFI.";
                e.Column.Format = "P";                
            }
            else if (e.Column.MappingName == "qmindi02")
            {
                e.Column.HeaderText = "M.D.";
                e.Column.TextAlignment = TextAlignment.Start;                
            }
            else if (e.Column.MappingName == "pefici02")
            {
                e.Column.HeaderText = "% EFI.";
                e.Column.TextAlignment = TextAlignment.End;
                e.Column.Format = "P";
            }
            else if (e.Column.MappingName == "qmindi03")
            {
                e.Column.HeaderText = "M.D.";
                e.Column.TextAlignment = TextAlignment.Start;                
            }
            else if (e.Column.MappingName == "pefici03")
            {
                e.Column.HeaderText = "% EFI.";
                e.Column.TextAlignment = TextAlignment.End;                
            }
            else if (e.Column.MappingName == "qmindi04")
            {
                e.Column.HeaderText = "M.D.";
                e.Column.TextAlignment = TextAlignment.Start;                
            }
            else if (e.Column.MappingName == "pefici04")
            {
                e.Column.HeaderText = "% EFI.";
                e.Column.TextAlignment = TextAlignment.End;                
            }
            else if (e.Column.MappingName == "qmindi05")
            {
                e.Column.HeaderText = "M.D.";
                e.Column.TextAlignment = TextAlignment.Start;
            }
            else if (e.Column.MappingName == "pefici05")
            {
                e.Column.HeaderText = "% EFI.";
                e.Column.TextAlignment = TextAlignment.End;
            }
            else if (e.Column.MappingName == "qmindi06")
            {
                e.Column.HeaderText = "M.D.";
                e.Column.TextAlignment = TextAlignment.Start;
            }
            else if (e.Column.MappingName == "pefici06")
            {
                e.Column.HeaderText = "% EFI.";
                e.Column.TextAlignment = TextAlignment.End;
            }
        }
    }
}
