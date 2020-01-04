using AppPFashions.Interfaces;
using AppPFashions.Models;
using AppPFashions.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppPFashions.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AuditoriaDefectos : INotifyPropertyChanged
    {
        IDownloader downloader = DependencyService.Get<IDownloader>();
        
        private ApiService apiService;
        private DialogService dialogService;
        Usuario user = App.baseDatos.GetUsuario();
        public List<string> SectorList { get; set; }
        public string xfproce01 { get; set; }
        public string xfproce02 { get; set; }
        public string xfproce03 { get; set; }
        public string xfproce04 { get; set; }
        public string xfproce05 { get; set; }
        public string xfproce06 { get; set; }
        public string xfproce07 { get; set; }
        public string xfproce08 { get; set; }
        public string xfproce09 { get; set; }
        public string xfproce10 { get; set; }
        public string xfproce11 { get; set; }
        public string xfproce12 { get; set; }
        public string xfproce13 { get; set; }
        public string xfproce14 { get; set; }
        public string xfproce15 { get; set; }
        public string xfproce16 { get; set; }

        public string imgpefici15 { get; set; }
        int total01, total02, total03, total04, total05, total06, total07, total15;

        private void Btn_reporte_auditoria_Clicked(object sender, EventArgs e)
        {
            if (pck_bloque.SelectedItem == null)
            {
                DisplayAlert("Aviso", "Debe seleccionar un bloque", "OK");
                return;
            }
            GetEficienciaSem();
        }

        private void DataGrid_QueryCellStyle(object sender, Syncfusion.SfDataGrid.XForms.QueryCellStyleEventArgs e)
        {
            //if (e.CellValue.ToString() == "F")
            //{
            //    if (e.ColumnIndex == 4  ||  e.ColumnIndex == 7 || e.ColumnIndex == 10 || e.ColumnIndex == 13 || e.ColumnIndex == 16 || e.ColumnIndex == 19 || e.ColumnIndex == 22)
            //    {
            //        e.Style.BackgroundColor = Color.White;
            //    }                
            //}
            //if (e.CellValue.ToString() == "A")
            //{
            //    if (e.ColumnIndex == 4 || e.ColumnIndex == 7 || e.ColumnIndex == 10 || e.ColumnIndex == 13 || e.ColumnIndex == 16 || e.ColumnIndex == 19 || e.ColumnIndex == 22)
            //    {
            //        e.Style.BackgroundColor = Color.FromHex("#FFFF99");
            //    }
            //}
            //if (e.CellValue.ToString() == "0")
            //{
            //    if (e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 7 || e.ColumnIndex == 8 || e.ColumnIndex == 10 || e.ColumnIndex == 11 || e.ColumnIndex == 13 || e.ColumnIndex == 14
            //        || e.ColumnIndex == 16 || e.ColumnIndex == 17 || e.ColumnIndex == 19 || e.ColumnIndex == 20 || e.ColumnIndex == 22 || e.ColumnIndex == 23)
            //    {
            //        e.Style.BackgroundColor = Color.FromHex("#66CDAA");
            //    }
            //}

            //if (e.CellValue.ToString().Trim() != "F" && e.CellValue.ToString().Trim() != "A" && e.CellValue.ToString() != "0")
            //{
            //    //if (e.CellValue.ToString() != "0")
            //    //{
            //        if (e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 7 || e.ColumnIndex == 8 || e.ColumnIndex == 10 || e.ColumnIndex == 11 || e.ColumnIndex == 13 || e.ColumnIndex == 14
            //        || e.ColumnIndex == 16 || e.ColumnIndex == 17 || e.ColumnIndex == 19 || e.ColumnIndex == 20 || e.ColumnIndex == 22 || e.ColumnIndex == 23)
            //        {
            //            e.Style.BackgroundColor = Color.PaleVioletRed;
            //    }
            //    //}
            //}
    
            e.Handled = true;
        }

        int total08, total09, total10, total11, total12, total13, total14, total16;
        int taudi01, taudi02, taudi03, taudi04, taudi05, taudi06, taudi07, taudi15;

        private void Tlb_viewpdf_Clicked(object sender, EventArgs e)
        {
            App.Navigator.PushAsync(new ProductionOrderPage());
        }

        public AuditoriaDefectos()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            SectorList = new List<string>
            {
                "01","02","03","04","05","06",
                "07","08","09","10","11","12","SE"
            };

            InitializeComponent();
            
            BindingContext = this;
            pck_bloque.ItemsSource = SectorList;
            
        }

        async void GetEficienciaSem()
        {
            DependencyService.Get<IDownloader>().Show("Descargando");
            ObservableCollection<ReporteAuditDefectos> dealerDetails = new ObservableCollection<ReporteAuditDefectos>();
            var response = await apiService.GetReporteAuditoriaDefectos<ReporteAuditDefectos>(pck_bloque.SelectedItem.ToString());
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Aviso", response.Message);
                DependencyService.Get<IDownloader>().Hide();
                return;
            }
            var fichas = (List<ReporteAuditDefectos>)response.Result;            
            
            string xcolor01 = "", xcolor02 = "", xcolor03 = "", xcolor04 = "";
            string xcolor05 = "", xcolor06 = "";
            string xcolre01 = "", xcolre02 = "", xcolre03 = "", xcolre04 = "";
            string xcolre05 = "", xcolre06 = "";            
            string xcolau01 = "", xcolau02 = "", xcolau03 = "", xcolau04 = "";
            string xcolau05 = "", xcolau06 = "";

            foreach (var recordf in fichas)
            { 
                xfproce01 = recordf.fproce01;
                xfproce02 = recordf.fproce02;
                xfproce03 = recordf.fproce03;
                xfproce04 = recordf.fproce04;
                xfproce05 = recordf.fproce05;
                xfproce06 = recordf.fproce06;                

                if (recordf.qaudia01 > 0) { xcolor01 = "#3D57AE"; }
                if (recordf.qaudia01 == 0) { xcolor01 = "#FFFFFF"; }
                if (recordf.qaudia02 > 0) { xcolor02 = "#3D57AE"; }
                if (recordf.qaudia02 == 0) { xcolor02 = "#FFFFFF"; }
                if (recordf.qaudia03 > 0) { xcolor03 = "#3D57AE"; }
                if (recordf.qaudia03 == 0) { xcolor03 = "#FFFFFF"; }
                if (recordf.qaudia04 > 0) { xcolor04 = "#3D57AE"; }
                if (recordf.qaudia04 == 0) { xcolor04 = "#FFFFFF"; }
                if (recordf.qaudia05 > 0) { xcolor05 = "#3D57AE"; }
                if (recordf.qaudia05 == 0) { xcolor05 = "#FFFFFF"; }
                if (recordf.qaudia06 > 0) { xcolor06 = "#3D57AE"; }
                if (recordf.qaudia06 == 0) { xcolor06 = "#FFFFFF"; }

                if (recordf.qaudir01 > 0) { xcolre01 = "#F20C36"; }
                if (recordf.qaudir01 == 0) { xcolre01 = "#FFFFFF"; }
                if (recordf.qaudir02 > 0) { xcolre02 = "#F20C36"; }
                if (recordf.qaudir02 == 0) { xcolre02 = "#FFFFFF"; }
                if (recordf.qaudir03 > 0) { xcolre03 = "#F20C36"; }
                if (recordf.qaudir03 == 0) { xcolre03 = "#FFFFFF"; }
                if (recordf.qaudir04 > 0) { xcolre04 = "#F20C36"; }
                if (recordf.qaudir04 == 0) { xcolre04 = "#FFFFFF"; }
                if (recordf.qaudir05 > 0) { xcolre05 = "#F20C36"; }
                if (recordf.qaudir05 == 0) { xcolre05 = "#FFFFFF"; }
                if (recordf.qaudir06 > 0) { xcolre06 = "#F20C36"; }
                if (recordf.qaudir06 == 0) { xcolre06 = "#FFFFFF"; }

                if (recordf.ndefec01 == "0") { xcolau01 = "#66CDAA"; }
                if (recordf.ndefec01 != "0") { xcolau01 = "#DB7093"; }
                if (recordf.ndefec01 == "F") { xcolau01 = "#FFFFFF"; }
                if (recordf.ndefec01 == "A") { xcolau01 = "#FFFF99"; }

                if (recordf.ndefec02 == "0") { xcolau02 = "#66CDAA"; }
                if (recordf.ndefec02 != "0") { xcolau02 = "#DB7093"; }
                if (recordf.ndefec02 == "F") { xcolau02 = "#FFFFFF"; }
                if (recordf.ndefec02 == "A") { xcolau02 = "#FFFF99"; }

                if (recordf.ndefec03 == "0") { xcolau03 = "#66CDAA"; }
                if (recordf.ndefec03 != "0") { xcolau03 = "#DB7093"; }
                if (recordf.ndefec03 == "F") { xcolau03 = "#FFFFFF"; }
                if (recordf.ndefec03 == "A") { xcolau03 = "#FFFF99"; }

                if (recordf.ndefec04 == "0") { xcolau04 = "#66CDAA"; }
                if (recordf.ndefec04 != "0") { xcolau04 = "#DB7093"; }
                if (recordf.ndefec04 == "F") { xcolau04 = "#FFFFFF"; }
                if (recordf.ndefec04 == "A") { xcolau04 = "#FFFF99"; }

                if (recordf.ndefec05 == "0") { xcolau05 = "#66CDAA"; }
                if (recordf.ndefec05 != "0") { xcolau05 = "#DB7093"; }
                if (recordf.ndefec05 == "F") { xcolau05 = "#FFFFFF"; }
                if (recordf.ndefec05 == "A") { xcolau05 = "#FFFF99"; }

                if (recordf.ndefec06 == "0") { xcolau06 = "#66CDAA"; }
                if (recordf.ndefec06 != "0") { xcolau06 = "#DB7093"; }
                if (recordf.ndefec06 == "F") { xcolau06 = "#FFFFFF"; }
                if (recordf.ndefec06 == "A") { xcolau06 = "#FFFF99"; }


                var ord = new ReporteAuditDefectos()
                {
                    clinea = recordf.clinea,
                    ctraba = recordf.ctraba,
                    dtraba = recordf.dtraba,

                    fproce01 = recordf.fproce01,

                    nmuest01 = recordf.nmuest01,
                    nmuest02 = recordf.nmuest02,
                    nmuest03 = recordf.nmuest03,
                    nmuest04 = recordf.nmuest04,
                    nmuest05 = recordf.nmuest05,
                    nmuest06 = recordf.nmuest06,
                    nmuest07 = recordf.nmuest07,

                    ndefec01 = recordf.ndefec01,
                    ndefec02 = recordf.ndefec02,
                    ndefec03 = recordf.ndefec03,
                    ndefec04 = recordf.ndefec04,
                    ndefec05 = recordf.ndefec05,
                    ndefec06 = recordf.ndefec06,
                    
                    pordef01 = recordf.pordef01,
                    pordef02 = recordf.pordef02,
                    pordef03 = recordf.pordef03,
                    pordef04 = recordf.pordef04,
                    pordef05 = recordf.pordef05,
                    pordef06 = recordf.pordef06,

                    qaudia01 = recordf.qaudia01,
                    qaudia02 = recordf.qaudia02,
                    qaudia03 = recordf.qaudia03,
                    qaudia04 = recordf.qaudia04,
                    qaudia05 = recordf.qaudia05,
                    qaudia06 = recordf.qaudia06,
                    qaudia07 = recordf.qaudia07,

                    qaudir01 = recordf.qaudir01,
                    qaudir02 = recordf.qaudir02,
                    qaudir03 = recordf.qaudir03,
                    qaudir04 = recordf.qaudir04,
                    qaudir05 = recordf.qaudir05,
                    qaudir06 = recordf.qaudir06,
                    qaudir07 = recordf.qaudir07,
                    colaud01 = xcolor01,
                    colaud02 = xcolor02,
                    colaud03 = xcolor03,
                    colaud04 = xcolor04,
                    colaud05 = xcolor05,
                    colaud06 = xcolor06,
                    colaur01 = xcolre01,
                    colaur02 = xcolre02,
                    colaur03 = xcolre03,
                    colaur04 = xcolre04,
                    colaur05 = xcolre05,
                    colaur06 = xcolre06,
                    coaudi01 = xcolau01,
                    coaudi02 = xcolau02,
                    coaudi03 = xcolau03,
                    coaudi04 = xcolau04,
                    coaudi05 = xcolau05,
                    coaudi06 = xcolau06,
                };
                dealerDetails.Add(ord);
            }

            dataGrid.ItemsSource = dealerDetails;
            DependencyService.Get<IDownloader>().Hide();
        }
    }
}