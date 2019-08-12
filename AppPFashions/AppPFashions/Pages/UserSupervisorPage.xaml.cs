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
	public partial class UserSupervisorPage : INotifyPropertyChanged
    {
        private ApiService apiService;
        private DialogService dialogService;
        public ObservableCollection<DashboardEficiencia> dealerDetails { get; set; }
        Usuario user = App.baseDatos.GetUsuario();
        public UserSupervisorPage ()
		{
            apiService = new ApiService();
            dialogService = new DialogService();
            InitializeComponent ();
    
            BindingContext = this;
            secondaryAxisLabelStyle.LabelFormat = "#'%'";
            secondaryAxisLabelStyle2.LabelFormat = "#'%'";
            secondaryAxisLabelStyle3.LabelFormat = "#'%'";
            GetDashboardEficiencia();

        }

        async void GetDashboardEficiencia()
        {
            DependencyService.Get<IDownloader>().Show("Descargando");
            dealerDetails = new ObservableCollection<DashboardEficiencia>();
            var response = await apiService.GetDashboardEficiencia<DashboardEficiencia>(user.cbloqu);
            var responselinea = await apiService.GetDashboardEficienciaLinea<DashboardEficienciaLinea>(user.cbloqu);
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Aviso", response.Message);
                DependencyService.Get<IDownloader>().Hide();
                return;
            }
            if (!responselinea.IsSuccess)
            {
                await dialogService.ShowMessage("Aviso", responselinea.Message);
                DependencyService.Get<IDownloader>().Hide();
                return;
            }
            var fichas = (List<DashboardEficiencia>)response.Result;
            var dashefilinea = (List<DashboardEficienciaLinea>)responselinea.Result;

            foreach (var recordf in fichas)
            {
                var ord = new DashboardEficiencia()
                {
                    fprdia = recordf.fprdia,
                    pefici = recordf.pefici,
                    perror = recordf.perror
                };
                dealerDetails.Add(ord);
            }

            foreach (var recordf in dashefilinea)
            {
                chartview3.Label = "Linea " + recordf.linea01;
                chartview4.Label = "Linea " + recordf.linea02;
            }
            chartview1.ItemsSource = dealerDetails;
            chartview2.ItemsSource = dealerDetails;
            chartview3.ItemsSource = dashefilinea;            
            chartview4.ItemsSource = dashefilinea;           

            DependencyService.Get<IDownloader>().Hide();
        }
    }
}