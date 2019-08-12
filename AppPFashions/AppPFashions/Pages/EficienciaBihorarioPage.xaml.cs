using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.SfDataGrid.XForms;
using AppPFashions.Services;
using AppPFashions.Models;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using PCLStorage;
using AppPFashions.Interfaces;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Globalization;
using Syncfusion.Data;

namespace AppPFashions.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EficienciaBihorarioPage : INotifyPropertyChanged
    {
        IDownloader downloader = DependencyService.Get<IDownloader>();
        //List<MinutosEficiencia> fichas = new List<MinutosEficiencia>();
        private ApiService apiService;
        private DialogService dialogService;        
        string dclien;

        public List<LDatos> Data;
        Usuario user = App.baseDatos.GetUsuario();

        public EficienciaBihorarioPage()
		{
            apiService = new ApiService();
            dialogService = new DialogService();
           

            InitializeComponent ();

            BindingContext =this;
            DownloadPhotos();
            GetMinEficiencia();
           
        }


        async void GetMinEficiencia()
        {
            DependencyService.Get<IDownloader>().Show("Descargando");
            ObservableCollection<ReporteEficienciaBihorario> dealerDetails = new ObservableCollection<ReporteEficienciaBihorario>();            
            var response = await apiService.GetEficienciaBihorarioBloque<EficienciaBihorario>(user.cbloqu);
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Aviso", response.Message);
                DependencyService.Get<IDownloader>().Hide();
                return;
            }
            var fichas = (List<EficienciaBihorario>)response.Result;

            int total01 = fichas.Sum(x => x.pefici01);
            int total02 = fichas.Sum(x => x.pefici02);
            int total03 = fichas.Sum(x => x.pefici03);
            int total04 = fichas.Sum(x => x.pefici04);
            int total05 = fichas.Sum(x => x.pefici05);
            int total06 = fichas.Sum(x => x.pefici06);

            if (total02 == 0)
            {
                mdi02.IsHidden = true;
                pef02.IsHidden = true;
            }

            if (total03 == 0)
            {
                mdi03.IsHidden = true;
                pef03.IsHidden = true;
            }

            if (total04 == 0)
            {
                mdi04.IsHidden = true;
                pef04.IsHidden = true;
            }
            if (total05 == 0)
            {
                mdi05.IsHidden = true;
                pef05.IsHidden = true;
            }

            if (total06 == 0)
            {
                mdi06.IsHidden = true;
                pef06.IsHidden = true;
            }


            foreach (var recordf in fichas)
            {
                Data = new List<LDatos>();
                if (total01 > 0) { Data.Add(new LDatos { Performance = recordf.pefici01 }); }
                if (total02 > 0) { Data.Add(new LDatos { Performance = recordf.pefici02 }); }
                if (total03 > 0) { Data.Add(new LDatos { Performance = recordf.pefici03 }); }
                if (total04 > 0) { Data.Add(new LDatos { Performance = recordf.pefici04 }); }
                if (total05 > 0) { Data.Add(new LDatos { Performance = recordf.pefici05 }); }
                if (total06 > 0) { Data.Add(new LDatos { Performance = recordf.pefici06 }); }

                string xctraba = "";
                if (Int32.Parse(recordf.ctraba.Substring(1, 5)) > 10000)
                {
                    xctraba = recordf.ctraba;
                }          
                else
                {
                    xctraba =recordf.ctraba.Substring(1,1)+recordf.ctraba.Substring(2, 4);
                }
                
                // Data = new List<LDatos>()
                //{
                //    new LDatos { Performance = recordf.pefici01 },
                //    new LDatos { Performance = recordf.pefici02 },
                //    new LDatos { Performance = recordf.pefici03 },
                //    new LDatos { Performance = recordf.pefici04 },
                //    new LDatos { Performance = recordf.pefici05 },
                //    new LDatos { Performance = recordf.pefici06 }
                //};

                string imgepefici01 = "", imgepefici02 = "", imgepefici03 = "";
                string imgepefici04 = "", imgepefici05 = "", imgepefici06 = "";
                string imgtpefici = "",linecolorefi="";


                if (recordf.pefici01 < 30) { imgepefici01 = "ic_pesima.png"; }
                if (recordf.pefici01 >= 30 && recordf.pefici01 < 50) { imgepefici01 = "ic_baja.png"; }
                if (recordf.pefici01 >= 50 && recordf.pefici01 < 80) { imgepefici01 = "ic_regular.png"; }
                if (recordf.pefici01 >= 80) { imgepefici01 = "ic_buena.png"; }

                if (recordf.pefici02 < 30) { imgepefici02 = "ic_pesima.png"; }
                if (recordf.pefici02 >= 30 && recordf.pefici02 < 50) { imgepefici02 = "ic_baja.png"; }
                if (recordf.pefici02 >= 50 && recordf.pefici02 < 80) { imgepefici02 = "ic_regular.png"; }
                if (recordf.pefici02 >= 80) { imgepefici02 = "ic_buena.png"; }

                if (recordf.pefici03 < 30) { imgepefici03 = "ic_pesima.png"; }
                if (recordf.pefici03 >= 30 && recordf.pefici03 < 50) { imgepefici03 = "ic_baja.png"; }
                if (recordf.pefici03 >= 50 && recordf.pefici03 < 80) { imgepefici03 = "ic_regular.png"; }
                if (recordf.pefici03 >= 80) { imgepefici03 = "ic_buena.png"; }

                if (recordf.pefici04 < 30) { imgepefici04 = "ic_pesima.png"; }
                if (recordf.pefici04 >= 30 && recordf.pefici04 < 50) { imgepefici04 = "ic_baja.png"; }
                if (recordf.pefici04 >= 50 && recordf.pefici04 < 80) { imgepefici04 = "ic_regular.png"; }
                if (recordf.pefici04 >= 80) { imgepefici04 = "ic_buena.png"; }

                if (recordf.pefici05 < 30) { imgepefici05 = "ic_pesima.png"; }
                if (recordf.pefici05 >= 30 && recordf.pefici05 < 50) { imgepefici05 = "ic_baja.png"; }
                if (recordf.pefici05 >= 50 && recordf.pefici05 < 80) { imgepefici05 = "ic_regular.png"; }
                if (recordf.pefici05 >= 80) { imgepefici05 = "ic_buena.png"; }

                if (recordf.pefici06 < 30) { imgepefici06 = "ic_pesima.png"; }
                if (recordf.pefici06 >= 30 && recordf.pefici06 < 50) { imgepefici06 = "ic_baja.png"; }
                if (recordf.pefici06 >= 50 && recordf.pefici06 < 80) { imgepefici06 = "ic_regular.png"; }
                if (recordf.pefici06 >= 80) { imgepefici06 = "ic_buena.png"; }

                if (recordf.tpefici < 30) { imgtpefici = "ic_pesima.png"; }
                if (recordf.tpefici >= 30 && recordf.tpefici < 50) { imgtpefici = "ic_baja.png"; }
                if (recordf.tpefici >= 50 && recordf.tpefici < 80) { imgtpefici = "ic_regular.png"; }
                if (recordf.tpefici >= 80) { imgtpefici = "ic_buena.png"; }

                if (recordf.tpefici < 50 ) { linecolorefi = "#9400D3"; }
                if (recordf.tpefici >= 50) { linecolorefi = "#00BFFF"; }

                var ord = new ReporteEficienciaBihorario()
                {                    
                    ctraba = recordf.ctraba,
                    //Photo = ImageSource.FromFile("/storage/emulated/0/Fotos/" + xctraba + ".bmp"),
                    dtraba = recordf.dtraba,
                    clinea = recordf.clinea,
                    fproce = recordf.fproce,
                    qmindi01 = recordf.qmindi01,
                    pefici01 = recordf.pefici01,
                    imgpefici01 = imgepefici01,
                    qmindi02 = recordf.qmindi02,
                    imgpefici02 = imgepefici02,
                    pefici02 = recordf.pefici02,
                    qmindi03 = recordf.qmindi03,
                    pefici03 = recordf.pefici03,
                    imgpefici03 = imgepefici03,
                    qmindi04 = recordf.qmindi04,
                    pefici04 = recordf.pefici04,
                    imgpefici04 = imgepefici04,
                    qmindi05 = recordf.qmindi05,
                    pefici05 = recordf.pefici05,
                    imgpefici05 = imgepefici05,
                    qmindi06 = recordf.qmindi06,
                    pefici06 = recordf.pefici06,
                    imgpefici06 = imgepefici06,
                    tqmindi = recordf.tqmindi,
                    tpefici = recordf.tpefici,
                    imgtpefici = imgtpefici,
                    linecolor= linecolorefi,
                    Data = Data,
                };
                dealerDetails.Add(ord);                                                
            }
               
            dataGrid.ItemsSource = dealerDetails;
            DependencyService.Get<IDownloader>().Hide();
        }
   
        async void DownloadPhotos()
        {                        
            DependencyService.Get<IDownloader>().Show("Descargando");
            var response = await apiService.GetEficienciaBihorarioBloque<EficienciaBihorario>(user.cbloqu);
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Aviso", response.Message);
                DependencyService.Get<IDownloader>().Hide();
                return;
            }
            var fichas = (List<EficienciaBihorario>)response.Result;
            //            dataGrid.ItemsSource = fichas;

            //********** INICIO DESCARGA DE ARCHIVOS DESDE FTP **********//            
            foreach (var recordf in fichas)
            {
                string xctraba = "";
                if (Int32.Parse(recordf.ctraba.Substring(1, 5)) > 10000)
                {
                    xctraba = recordf.ctraba;
                }
                else
                {
                    xctraba = recordf.ctraba.Substring(0, 1) + recordf.ctraba.Substring(2, 4);
                }
                IFolder rootFolder = await FileSystem.Current.GetFolderFromPathAsync("/storage/emulated/0/Fotos/");
                ExistenceCheckResult folderexist = await rootFolder.CheckExistsAsync(xctraba);
                if (folderexist == ExistenceCheckResult.FileExists)
                {
                    IFile file = await rootFolder.GetFileAsync(xctraba + ".bmp");
                    await file.DeleteAsync();
                }

                string rutapdf = "ftp://192.168.2.55:22/" + xctraba + ".bmp";
                downloader.DownloadFile(rutapdf, "Fotos");        
            }            
            DependencyService.Get<IDownloader>().Hide();
            //********** FIN DESCARGA DE ARCHIVOS DESDE FTP **********//
        }

        private void Tlb_viewpdf_Clicked(object sender, EventArgs e)
        {
            App.Navigator.PushAsync(new ProductionOrderPage());
        }

        private void DataGrid_GridDoubleTapped(object sender, GridDoubleTappedEventArgs e)
        {
            var seldestajero = (e.RowData) as ReporteEficienciaBihorario;
            string dtraba = seldestajero.ctraba + " - " + seldestajero.dtraba;
            //dialogService.ShowMessage("Aviso", seldestajero.fproce.ToString("yyyy-MM-dd"));
            App.Navigator.PushAsync(new DestajeroDetallePage(seldestajero.ctraba.Trim()+seldestajero.fproce.ToString("yyyy-MM-dd")+dtraba));
        }
    }

    public class GroupCaptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value != null ? value as Group : null;
            if (data != null)
            {
                SfDataGrid dataGrid = (SfDataGrid)parameter;
                var summaryText = SummaryCreator.GetSummaryDisplayTextForRow((value as Group).SummaryDetails, dataGrid.View);

                return summaryText;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}