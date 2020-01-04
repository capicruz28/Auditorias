using AppPFashions.Interfaces;
using AppPFashions.Models;
using AppPFashions.Services;
using Syncfusion.Data;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppPFashions.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProgramaCosturaPage : INotifyPropertyChanged
    {
        IDownloader downloader = DependencyService.Get<IDownloader>();
        private ApiService apiService;
        private DialogService dialogService;
        Usuario user = App.baseDatos.GetUsuario();
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
        public Int32 total14 { get; set; }
        public Int32 total15 { get; set; }

        public ProgramaCosturaPage ()
		{
            apiService = new ApiService();
            dialogService = new DialogService();

            InitializeComponent ();

            BindingContext = this;
            GetEficienciaSem();
        }

        async void GetEficienciaSem()
        {
            DependencyService.Get<IDownloader>().Show("Descargando");
            ObservableCollection<ProgramaCostura> dealerDetails = new ObservableCollection<ProgramaCostura>();
            var response = await apiService.GetProgramaCostura<ProgramaCostura>(user.cbloqu,"2019-09-03","2019-09-16");
            if (!response.IsSuccess)
            {

                await dialogService.ShowMessage("Aviso", response.Message);
                DependencyService.Get<IDownloader>().Hide();
                return;
            }
            var fichas = (List<ProgramaCostura>)response.Result;
            
            total01 = fichas.Sum(x => x.qprogr01);
            total02 = fichas.Sum(x => x.qprogr02);
            total03 = fichas.Sum(x => x.qprogr03);
            total04 = fichas.Sum(x => x.qprogr04);
            total05 = fichas.Sum(x => x.qprogr05);
            total06 = fichas.Sum(x => x.qprogr06);
            total07 = fichas.Sum(x => x.qprogr07);
            total08 = fichas.Sum(x => x.qprogr08);
            total09 = fichas.Sum(x => x.qprogr09);
            total10 = fichas.Sum(x => x.qprogr10);
            total11 = fichas.Sum(x => x.qprogr11);
            total12 = fichas.Sum(x => x.qprogr12);
            total13 = fichas.Sum(x => x.qprogr13);
            total14 = fichas.Sum(x => x.qprogr14);
            total15 = fichas.Sum(x => x.qprogr15);

            if (total01 == 0) { progra1.IsHidden = true; }
            if (total02 == 0) { progra2.IsHidden = true; }
            if (total03 == 0) { progra3.IsHidden = true; }
            if (total04 == 0) { progra4.IsHidden = true; }
            if (total05 == 0) { progra5.IsHidden = true; }
            if (total06 == 0) { progra6.IsHidden = true; }
            if (total07 == 0) { progra7.IsHidden = true; }
            if (total08 == 0) { progra8.IsHidden = true; }
            if (total09 == 0) { progra9.IsHidden = true; }
            if (total10 == 0) { progra10.IsHidden = true; }
            if (total11 == 0) { progra11.IsHidden = true; }
            if (total12 == 0) { progra12.IsHidden = true; }
            if (total13 == 0) { progra13.IsHidden = true; }
            if (total14 == 0) { progra14.IsHidden = true; }
            foreach (var recordf in fichas)
            {
                xfproce01 = recordf.fproce01;
                xfproce02 = recordf.fproce02;
                xfproce03 = recordf.fproce03;
                xfproce04 = recordf.fproce04;
                xfproce05 = recordf.fproce05;
                xfproce06 = recordf.fproce06;
                xfproce07 = recordf.fproce07;
                xfproce08 = recordf.fproce08;
                xfproce09 = recordf.fproce09;
                xfproce10 = recordf.fproce10;
                xfproce11 = recordf.fproce11;
                xfproce12 = recordf.fproce12;
                xfproce13 = recordf.fproce13;
                xfproce14 = recordf.fproce14;
                xfproce15 = recordf.fproce15;

                //var ord = new ProgramaCostura()
                //{
                //    clinea = recordf.clinea,
                //    nordpr = recordf.nordpr,
                //    ccarub = recordf.ccarub,
                //    dcarub = recordf.dcarub,
                //    fproce01 = recordf.fproce01,
                //    fproce02 = recordf.fproce02,
                //    fproce03 = recordf.fproce03,
                //    fproce04 = recordf.fproce04,
                //    fproce05 = recordf.fproce05,
                //    fproce06 = recordf.fproce06,
                //    fproce07 = recordf.fproce07,
                //    fproce08 = recordf.fproce08,
                //    fproce09 = recordf.fproce09,
                //    fproce10 = recordf.fproce10,
                //    fproce11 = recordf.fproce11,
                //    fproce12 = recordf.fproce12,
                //    fproce13 = recordf.fproce13,
                //    fproce14 = recordf.fproce14,
                //    fproce15 = recordf.fproce15,
                //    qprogr01 = recordf.fp
                //};
                //dealerDetails.Add(ord);
            }

                dataGrid.ItemsSource = fichas;
            DependencyService.Get<IDownloader>().Hide();
        }
    }

    public class TableSummaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            for (int x = 1; x <= 14; x++)
            {
                var data = value != null ? value as SummaryRecordEntry : null;
            if (data != null)
            {
                SfDataGrid dataGrid = (SfDataGrid)parameter;
              
                    var summaryText = SummaryCreator.GetSummaryDisplayText(data, "qprogr0"+x, dataGrid.View);

                    return summaryText.ToString();
               
            }
            }
            return null;
           
        }
     

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}