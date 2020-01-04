using AppPFashions.Interfaces;
using AppPFashions.Models;
using AppPFashions.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppPFashions.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BalanceLineaCosturaPage : ContentPage
	{
        IDownloader downloader = DependencyService.Get<IDownloader>();
        private ApiService apiService;
        private DialogService dialogService;
        Usuario user = App.baseDatos.GetUsuario();
        public BalanceLineaCosturaPage ()
		{
            apiService = new ApiService();
            dialogService = new DialogService();

            InitializeComponent();

            BindingContext = this;
            //GetEficienciaSem();
        }

        async void GetEficienciaSem()
        {
            DependencyService.Get<IDownloader>().Show("Descargando");

            var response = await apiService.GetBalanceLineaCostura<BalanceLineaCostura>(ety_op.Text, user.cbloqu);
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Aviso", response.Message);
                DependencyService.Get<IDownloader>().Hide();
                return;
            }
            var fichas = (List<BalanceLineaCostura>)response.Result;

         

            dataGrid.ItemsSource = fichas;
            DependencyService.Get<IDownloader>().Hide();
        }

        private void Btn_buscaringreso_Clicked(object sender, EventArgs e)
        {
            GetEficienciaSem();
        }
    }
}