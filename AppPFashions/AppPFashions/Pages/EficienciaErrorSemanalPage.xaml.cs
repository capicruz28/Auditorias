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
	public partial class EficienciaErrorSemanalPage : ContentPage
	{
        IDownloader downloader = DependencyService.Get<IDownloader>();
        public List<LSEDatos> Data;
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
        public string xfproce16 { get; set; }

        public string xnseman01 { get; set; }
        public string xnseman02 { get; set; }
        public string xnseman03 { get; set; }
        public string xnseman04 { get; set; }
        public string xnseman05 { get; set; }
        public string xnseman06 { get; set; }
        public string xnseman07 { get; set; }
        public string xnseman08 { get; set; }
        public string xnseman09 { get; set; }
        public string xnseman10 { get; set; }
        public string xnseman11 { get; set; }
        public string xnseman12 { get; set; }
        public string xnseman13 { get; set; }
        public string xnseman14 { get; set; }
        public string xnseman15 { get; set; }
        public string xnseman16 { get; set; }
        public string imgpefici15 { get; set; }
        int total01, total02, total03, total04, total05, total06, total07, total15;
        int total08, total09, total10, total11, total12, total13, total14, total16;
        int taudi01, taudi02, taudi03, taudi04, taudi05, taudi06, taudi07, taudi15;

        private void Tlb_viewpdf_Clicked(object sender, EventArgs e)
        {
            App.Navigator.PushAsync(new ProductionOrderPage());
        }
        
        public EficienciaErrorSemanalPage ()
		{
            apiService = new ApiService();
            dialogService = new DialogService();

            InitializeComponent();

            BindingContext = this;
            GetEficienciaSem();
        }

        async void GetEficienciaSem()
        {
            DependencyService.Get<IDownloader>().Show("Descargando");
            ObservableCollection<ReporteEficienciaErrorSemanal> dealerDetails = new ObservableCollection<ReporteEficienciaErrorSemanal>();
            var response = await apiService.GetEficienciaErrorSemanalBloque<EficienciaErrorSemanal>(user.cbloqu);
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Aviso", response.Message);
                DependencyService.Get<IDownloader>().Hide();
                return;
            }
            var fichas = (List<EficienciaErrorSemanal>)response.Result;


            total01 = fichas.Sum(x => x.pefici01);
            total02 = fichas.Sum(x => x.pefici02);
            total03 = fichas.Sum(x => x.pefici03);
            total04 = fichas.Sum(x => x.pefici04);
            total05 = fichas.Sum(x => x.pefici05);
            total06 = fichas.Sum(x => x.pefici06);
            total07 = fichas.Sum(x => x.pefici07);
            total08 = fichas.Sum(x => x.pefici08);
            total09 = fichas.Sum(x => x.pefici09);
            total10 = fichas.Sum(x => x.pefici10);
            total11 = fichas.Sum(x => x.pefici11);
            total12 = fichas.Sum(x => x.pefici12);
            total13 = fichas.Sum(x => x.pefici13);
            total14 = fichas.Sum(x => x.pefici14);
            total15 = fichas.Sum(x => x.pefici15);
            total16 = fichas.Sum(x => x.pefici16);

            taudi01 = fichas.Sum(x => x.paudit01);
            taudi02 = fichas.Sum(x => x.paudit02);
            taudi03 = fichas.Sum(x => x.paudit03);
            taudi04 = fichas.Sum(x => x.paudit04);
            taudi05 = fichas.Sum(x => x.paudit05);
            taudi06 = fichas.Sum(x => x.paudit06);
            taudi07 = fichas.Sum(x => x.paudit07);
            taudi15 = fichas.Sum(x => x.paudit15);

            if (total06 == 0) { pefi06.IsHidden = true; paudi06.IsHidden = true; }
            if (total07 == 0) { pefi07.IsHidden = true; paudi07.IsHidden = true; }
            if (total08 == 0) { pefi08.IsHidden = true; paudi08.IsHidden = true; }
            if (total09 == 0) { pefi09.IsHidden = true; paudi09.IsHidden = true; }
            if (total10 == 0) { pefi10.IsHidden = true; paudi10.IsHidden = true; }
            if (total11 == 0) { pefi11.IsHidden = true; paudi11.IsHidden = true; }
            if (total12 == 0) { pefi12.IsHidden = true; paudi12.IsHidden = true; }
            if (total13 == 0) { pefi13.IsHidden = true; paudi13.IsHidden = true; }
            if (total14 == 0) { pefi14.IsHidden = true; paudi14.IsHidden = true; }
            if (total15 == 0) { pefi15.IsHidden = true; }
            if (total16 == 0) { pefi16.IsHidden = true; paudi16.IsHidden = true; }
            if (taudi01 == 0) { paudi01.IsHidden = true; paudi01.IsHidden = true; }
            if (taudi02 == 0) { paudi02.IsHidden = true; paudi02.IsHidden = true; }
            if (taudi03 == 0) { paudi03.IsHidden = true; paudi03.IsHidden = true; }
            if (taudi04 == 0) { paudi04.IsHidden = true; paudi04.IsHidden = true; }
            if (taudi05 == 0) { paudi05.IsHidden = true; paudi05.IsHidden = true; }
            if (taudi06 == 0) { paudi06.IsHidden = true; paudi06.IsHidden = true; }
            if (taudi07 == 0) { paudi07.IsHidden = true; paudi07.IsHidden = true; }
            if (taudi15 == 0) { paudi15.IsHidden = true; paudi15.IsHidden = true; }

            foreach (var recordf in fichas)
            {
                Data = new List<LSEDatos>();
                if (total01 > 0) { Data.Add(new LSEDatos { Performance = recordf.pefici01 }); }
                if (total02 > 0) { Data.Add(new LSEDatos { Performance = recordf.pefici02 }); }
                if (total03 > 0) { Data.Add(new LSEDatos { Performance = recordf.pefici03 }); }
                if (total04 > 0) { Data.Add(new LSEDatos { Performance = recordf.pefici04 }); }
                if (total05 > 0) { Data.Add(new LSEDatos { Performance = recordf.pefici05 }); }
                if (total06 > 0) { Data.Add(new LSEDatos { Performance = recordf.pefici06 }); }
                if (total07 > 0) { Data.Add(new LSEDatos { Performance = recordf.pefici07 }); }
                if (total08 > 0) { Data.Add(new LSEDatos { Performance = recordf.pefici08 }); }
                if (total09 > 0) { Data.Add(new LSEDatos { Performance = recordf.pefici09 }); }
                if (total10 > 0) { Data.Add(new LSEDatos { Performance = recordf.pefici10 }); }
                if (total11 > 0) { Data.Add(new LSEDatos { Performance = recordf.pefici11 }); }
                if (total12 > 0) { Data.Add(new LSEDatos { Performance = recordf.pefici12 }); }
                if (total13 > 0) { Data.Add(new LSEDatos { Performance = recordf.pefici13 }); }
                if (total14 > 0) { Data.Add(new LSEDatos { Performance = recordf.pefici14 }); }
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
                xfproce16 = recordf.fproce16;

                xnseman01 = recordf.nseman01;
                xnseman02 = recordf.nseman02;
                xnseman03 = recordf.nseman03;
                xnseman04 = recordf.nseman04;
                xnseman05 = recordf.nseman05;
                xnseman06 = recordf.nseman06;
                xnseman07 = recordf.nseman07;
                xnseman08 = recordf.nseman08;
                xnseman09 = recordf.nseman09;
                xnseman10 = recordf.nseman10;
                xnseman11 = recordf.nseman11;
                xnseman12 = recordf.nseman12;
                xnseman13 = recordf.nseman13;
                xnseman14 = recordf.nseman14;
                xnseman15 = recordf.nseman15;
                xnseman16 = recordf.nseman16;

                string imgepefici15 = "", imgepefici16 = "";
                string imgepaudit15 = "ic_pesima.png", imgepaudit16 = "ic_pesima.png";
                if (recordf.pefici15 < 30) { imgepefici15 = "ic_pesima.png"; }
                if (recordf.pefici15 >= 30 && recordf.pefici15 < 50) { imgepefici15 = "ic_baja.png"; }
                if (recordf.pefici15 >= 50 && recordf.pefici15 < 80) { imgepefici15 = "ic_regular.png"; }
                if (recordf.pefici15 >= 80) { imgepefici15 = "ic_buena.png"; }

                if (recordf.pefici15 < 30) { imgepefici16 = "ic_pesima.png"; }
                if (recordf.pefici15 >= 30 && recordf.pefici15 < 50) { imgepefici16 = "ic_baja.png"; }
                if (recordf.pefici15 >= 50 && recordf.pefici15 < 80) { imgepefici16 = "ic_regular.png"; }
                if (recordf.pefici15 >= 80) { imgepefici16 = "ic_buena.png"; }

                if (recordf.paudit06 > 0) { }
                var ord = new ReporteEficienciaErrorSemanal()
                {
                    clinea = recordf.clinea,
                    ctraba = recordf.ctraba,
                    dtraba = recordf.dtraba,
                    pefici01 = recordf.pefici01,
                    paudit01 = recordf.paudit01,
                    pefici02 = recordf.pefici02,
                    paudit02 = recordf.paudit02,
                    pefici03 = recordf.pefici03,
                    paudit03 = recordf.paudit03,
                    pefici04 = recordf.pefici04,
                    paudit04 = recordf.paudit04,
                    pefici05 = recordf.pefici05,
                    paudit05 = recordf.paudit05,
                    pefici06 = recordf.pefici06,
                    paudit06 = recordf.paudit06,
                    pefici07 = recordf.pefici07,
                    paudit07 = recordf.paudit07,
                    pefici15 = recordf.pefici15,
                    paudit15 = recordf.paudit15,
                    imgpefici15 = imgepefici15,
                    imgpaudit15 = imgepaudit15,
                    pefici08 = recordf.pefici08,
                    paudit08 = recordf.paudit08,
                    pefici09 = recordf.pefici09,
                    paudit09 = recordf.paudit09,
                    pefici10 = recordf.pefici10,
                    paudit10 = recordf.paudit10,
                    pefici11 = recordf.pefici11,
                    paudit11 = recordf.paudit11,
                    pefici12 = recordf.pefici12,
                    paudit12 = recordf.paudit12,
                    pefici13 = recordf.pefici13,
                    paudit13 = recordf.paudit13,
                    pefici14 = recordf.pefici14,
                    paudit14 = recordf.paudit14,
                    pefici16 = recordf.pefici16,
                    paudit16 = recordf.paudit16,
                    imgpefici16 = imgepefici16,
                    imgpaudit16 = imgepaudit16,
                    Data = Data
                };
                dealerDetails.Add(ord);
            }

            dataGrid.ItemsSource = dealerDetails;
            DependencyService.Get<IDownloader>().Hide();
        }
    }
}