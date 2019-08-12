using AppPFashions.Interfaces;
using AppPFashions.Models;
using AppPFashions.Services;
using Syncfusion.SfChart.XForms;
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
	public partial class DestajeroDetallePage : INotifyPropertyChanged
    {
        IDownloader downloader = DependencyService.Get<IDownloader>();        
        private ApiService apiService;
        private DialogService dialogService;
        string destajero;
        public ObservableCollection<ReporteEficienciaDestajeroDetalle> dealerDetails { get; set; }   
        public string dtraba { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private ImageSource photo;
        public ImageSource Photo
        {
            get
            {
                return this.photo;
            }

            set
            {
                this.photo = value;
                this.RaisePropertyChanged("Photo");
            }
        }

        private void RaisePropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        Usuario user = App.baseDatos.GetUsuario();
        public DestajeroDetallePage (string seldestajero)
		{
            apiService = new ApiService();
            dialogService = new DialogService();

            InitializeComponent ();
            dealerDetails = new ObservableCollection<ReporteEficienciaDestajeroDetalle>();
            destajero = seldestajero;
            BindingContext = this;
            //secondaryAxisLabelStyle.LabelFormat = "#'%'";
            List<Color> colors = new List<Color>()
            {
                Color.FromHex("47ba9f"),
                Color.FromHex("e58870"),
                Color.FromHex("9686c9"),
                Color.FromHex("e56590"),
                Color.FromHex("6495ED"),
                Color.FromHex("DDA0DD")
            };            
            colorModel.CustomBrushes = colors;
            GetMinEficiencia();
        }

        //protected override void OnSizeAllocated(double width, double height)
        //{
        //    base.OnSizeAllocated(width, height);
        //    if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
        //    {
        //        if (height > 0 && width > 0)
        //        {
        //            if (height > width)
        //            {
        //                Chart.Legend.DockPosition = LegendPlacement.Bottom;
        //                legend.OverflowMode = ChartLegendOverflowMode.Wrap;
        //                mainpage.Orientation = StackOrientation.Horizontal;
        //            }
        //            else
        //            {
        //                mainpage.Orientation = StackOrientation.Vertical;
        //                Chart.Title.Margin = new Thickness(0);
        //                Chart.Legend.DockPosition = LegendPlacement.Right;
        //                legend.OverflowMode = ChartLegendOverflowMode.Scroll;
        //            }
        //        }
        //    }
        //}

        async void GetMinEficiencia()
        {            
            DependencyService.Get<IDownloader>().Show("Descargando");
           
            var response = await apiService.GetEficienciaDestajeroDetalle<EficienciaDestajeroDetalle>(user.cbloqu,destajero.Substring(0,6), destajero.Substring(6, 10));
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Aviso", response.Message);
                DependencyService.Get<IDownloader>().Hide();
                return;
            }

            string xctraba = "";
            if (Int32.Parse(destajero.Substring(1, 5)) > 10000)
            {
                xctraba = destajero.Substring(0, 6);
            }
            else
            {
                xctraba = destajero.Substring(0, 1) + destajero.Substring(2, 4);
            }

            var fichas = (List<EficienciaDestajeroDetalle>)response.Result;
            Photo = ImageSource.FromFile("/storage/emulated/0/Fotos/"+ xctraba + ".bmp");            
            var taudit02 = (from a in fichas.OrderBy(x=>x.cbihor)
                            group a by new
                              {
                                  a.cbihor                                  
                              }
                       into b
                              select new
                              {
                                  Cbihor = b.Key.cbihor,
                                  Pefici = b.Sum(x=>x.pefici)
                              }).ToList();
            foreach (var recordf in taudit02)
            {
            
                var ord = new ReporteEficienciaDestajeroDetalle()
                {
                    cbihor = recordf.Cbihor,
                    pefici = recordf.Pefici,                    
                };
                dealerDetails.Add(ord);
            }
            dtraba = destajero.Substring(16, destajero.Length - 16);
            Chart.Title.Text = dtraba;
            Chart.Title.TextColor = Color.DimGray;
            Chart.Title.FontAttributes = FontAttributes.Bold;
            chartview.ItemsSource = dealerDetails;            
            dataGrid.ItemsSource = fichas;


            DependencyService.Get<IDownloader>().Hide();
        }
    }

    public class IndexToItemSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }

            return new ObservableCollection<object>() { (value as ChartLegendItem).DataPoint };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}