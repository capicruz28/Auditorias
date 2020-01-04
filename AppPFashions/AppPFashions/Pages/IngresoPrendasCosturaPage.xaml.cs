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
	public partial class IngresoPrendasCosturaPage : INotifyPropertyChanged
    {
        IDownloader downloader = DependencyService.Get<IDownloader>();
        private ApiService apiService;
        private DialogService dialogService;
        Usuario user = App.baseDatos.GetUsuario();
        public Int32 total01 { get; set; }
        public Int32 total02 { get; set; }
        public Int32 total03 { get; set; }
        public Int32 total04 { get; set; }
        public Int32 total05 { get; set; }
        public Int32 total06 { get; set; }
        public Int32 total07 { get; set; }
        public Int32 total08 { get; set; }
        public Int32 total09 { get; set; }
        public Int32 total10 { get; set; }
        public Int32 total11 { get; set; }
        public Int32 total12 { get; set; }
        public Int32 total13 { get; set; }

        public IngresoPrendasCosturaPage ()
		{
            apiService = new ApiService();
            dialogService = new DialogService();

            InitializeComponent();

            BindingContext = this;
            GetEficienciaSem("F",DateTime.Now.ToShortDateString(), DateTime.Now.ToShortDateString(),"190001","S");
        }

        async void GetEficienciaSem(string ctipos, string wfecini, string wfecfin, string nordpr, string sbloqc)
        {
            DependencyService.Get<IDownloader>().Show("Descargando");
            ObservableCollection<IngresoPrendasCostura> dealerDetails = new ObservableCollection<IngresoPrendasCostura>();
            var response = await apiService.GetIngresoPrendasCostura<IngresoPrendasCostura>(ctipos, wfecini, wfecfin,nordpr, sbloqc);
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Aviso", response.Message);
                DependencyService.Get<IDownloader>().Hide();
                return;
            }
            var fichas = (List<IngresoPrendasCostura>)response.Result;

            total01 = fichas.Sum(x => x.qprend01);
            total02 = fichas.Sum(x => x.qprend02);
            total03 = fichas.Sum(x => x.qprend03);
            total04 = fichas.Sum(x => x.qprend04);
            total05 = fichas.Sum(x => x.qprend05);
            total06 = fichas.Sum(x => x.qprend06);
            total07 = fichas.Sum(x => x.qprend07);
            total08 = fichas.Sum(x => x.qprend08);
            total09 = fichas.Sum(x => x.qprend09);
            total10 = fichas.Sum(x => x.qprend10);
            total11 = fichas.Sum(x => x.qprend11);
            total12 = fichas.Sum(x => x.qprend12);
            total13 = fichas.Sum(x => x.qprend13);

            if (total01 == 0) { progra1.IsHidden = true; } else { progra1.IsHidden = false; }
            if (total02 == 0) { progra2.IsHidden = true; } else { progra2.IsHidden = false; }
            if (total03 == 0) { progra3.IsHidden = true; } else { progra3.IsHidden = false; }
            if (total04 == 0) { progra4.IsHidden = true; } else { progra4.IsHidden = false; }
            if (total05 == 0) { progra5.IsHidden = true; } else { progra5.IsHidden = false; }
            if (total06 == 0) { progra6.IsHidden = true; } else { progra6.IsHidden = false; }
            if (total07 == 0) { progra7.IsHidden = true; } else { progra7.IsHidden = false; }
            if (total08 == 0) { progra8.IsHidden = true; } else { progra8.IsHidden = false; }
            if (total09 == 0) { progra9.IsHidden = true; } else { progra9.IsHidden = false; }
            if (total10 == 0) { progra10.IsHidden = true; } else { progra10.IsHidden = false; }
            if (total11 == 0) { progra11.IsHidden = true; } else { progra11.IsHidden = false; }
            if (total12 == 0) { progra12.IsHidden = true; } else { progra12.IsHidden = false; }
            //if (total13 == 0) { progra13.IsHidden = true; }            

            dataGrid.ItemsSource = fichas;
            DependencyService.Get<IDownloader>().Hide();
        }

        private void Srb_fecha_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (e.IsChecked==true)
            {
                srb_op.IsChecked = false;
                ety_op.IsEnabled = false;
                dpk_fecha.IsEnabled = true;
            }
        }

        private void Srb_op_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (e.IsChecked == true)
            {
                srb_fecha.IsChecked = false;
                dpk_fecha.IsEnabled = false;
                ety_op.IsEnabled = true;
            }
        }

        private void Btn_buscaringreso_Clicked(object sender, EventArgs e)
        {
            if (srb_fecha.IsChecked == true)
            {
                GetEficienciaSem("F", dpk_fecha.Date.ToString("yyyy-MM-dd"), dpk_fecha.Date.ToString("yyyy-MM-dd"), "190001", "S");
            }

            if (srb_op.IsChecked == true)
            {
                GetEficienciaSem("O", "1900-01-01", "1900-01-01", ety_op.Text, "S");
            }

        }

        private void Dpk_fecha_DateSelected(object sender, DateChangedEventArgs e)
        {
            //DisplayAlert("Aviso", dpk_fecha.Date.ToShortDateString() + " "+DateTime.Now.ToShortDateString() , "OK");
        }
    }
}