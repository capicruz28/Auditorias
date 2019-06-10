using AppPFashions.Data;
using AppPFashions.Models;
using AppPFashions.Services;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppPFashions.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CosturaProcesoDetallePage : ContentPage
	{
        private ApiService apiService;
        private DialogService dialogService;
        private AlertService alertService;
        private Image leftImage;
        private Image rightImage;
        private int swipedRowIndex;
        List<taudit00> xoperac;
        string nclinea;
        string saprob;
        string daprob;
        string areaau;
        string cfaudit;

        string desauditoria;

        public string Desauditoria
        {
            get
            {
                return desauditoria;
            }
            set
            {
                if (desauditoria != value)
                {
                    desauditoria = value;
                    OnPropertyChanged("Desauditoria");
                }
            }
        }

        public CosturaProcesoDetallePage (string xclinea)
		{           
            apiService = new ApiService();
            dialogService = new DialogService();
            alertService = new AlertService();
            nclinea = xclinea.Substring(0,2);
            saprob = xclinea.Substring(2, 1);
            areaau= xclinea.Substring(3, 2);
            cfaudit = xclinea.Substring(5, 10);
            InitializeComponent ();
            BindingContext = this;
            if (saprob == "A") { daprob = "Aprobados"; }
            if (saprob == "D") { daprob = "Desaprobados"; }
            if (saprob == "E") { daprob = "Aprobados Ext."; }
            if (areaau == "19") Desauditoria = "Auditoria Costura Proceso - "+daprob+" - "+cfaudit;
            if (areaau == "FC") Desauditoria = "Auditoria Costura Final - " + daprob + " - " + cfaudit;
            if (areaau == "16") Desauditoria = "Auditoria Corte - " + daprob + " - " + cfaudit;
            if (areaau == "29") Desauditoria = "Auditoria Bordado - " + daprob + " - " + cfaudit;
            this.PropertyChanged += CosturaProcesoDetallePage_PropertyChanged;
            dataGrid.QueryRowHeight += DataGrid_QueryRowHeight;
            LoadDetalleAuditorias(nclinea);            
        }

        protected override void OnAppearing()
        {
            LoadDetalleAuditorias(nclinea);
            base.OnAppearing();
        }

        private void CosturaProcesoDetallePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Width")
            {                
                dataGrid.Opacity = 1.0;
            }
        }

        void DataGrid_QueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            //Sets height of the fifth row
            if (e.RowIndex != 0)
            {
                //Calculates and sets the height of the row based on its content.
                e.Height = dataGrid.GetRowHeight(e.RowIndex);
                e.Handled = true;
            }
        }   

        async void LoadDetalleAuditorias(string xclinea)
        {
            try
            {
                using (var data = new DataAccess())
                {
                    xoperac = data.GetList<taudit00>(false).Where(x=>x.clinea==xclinea && x.status==saprob && x.careas== areaau && x.faudit.ToString("dd-MM-yyyy")==cfaudit).ToList();
                    dataGrid.ItemsSource = xoperac;           
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", ex.StackTrace, "OK");
            }                      
        }

        private void leftImage_Swiping_BindingContextChanged(object sender, EventArgs e)
        {
            if (leftImage == null)
            {
                leftImage = sender as Image;
                (leftImage.Parent as View).GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(Edit) });
                //leftImage.Source = ImageSource.FromResource("SfDataGridSample.EditIcon.png");
            }
        }

        private void Edit()
        {
            var dataok = new List<taudit00>
            {
                new taudit00
                {
                idaudi = xoperac.ElementAt(swipedRowIndex - 1).idaudi,
                careas = xoperac.ElementAt(swipedRowIndex - 1).careas.ToString(),
                faudit = DateTime.Parse(xoperac.ElementAt(swipedRowIndex - 1).faudit.ToString()),
                nsecue = Int32.Parse(xoperac.ElementAt(swipedRowIndex - 1).nsecue.ToString()),
                clinea = xoperac.ElementAt(swipedRowIndex - 1).clinea.ToString(),
                nordpr = xoperac.ElementAt(swipedRowIndex - 1).nordpr.ToString(),
                ccarub = xoperac.ElementAt(swipedRowIndex - 1).ccarub.ToString(),
                dcarub = xoperac.ElementAt(swipedRowIndex - 1).dcarub.ToString(),
                ctraba = xoperac.ElementAt(swipedRowIndex - 1).ctraba.ToString(),
                copera = xoperac.ElementAt(swipedRowIndex - 1).copera.ToString(),
                dopera = xoperac.ElementAt(swipedRowIndex - 1).dopera.ToString(),
                dclien = xoperac.ElementAt(swipedRowIndex - 1).dclien.ToString(),
                nlotes = Int32.Parse(xoperac.ElementAt(swipedRowIndex - 1).nlotes.ToString()),
                nmuest = Int32.Parse(xoperac.ElementAt(swipedRowIndex - 1).nmuest.ToString()),
                status = xoperac.ElementAt(swipedRowIndex - 1).status.ToString(),
                dobser = xoperac.ElementAt(swipedRowIndex - 1).dobser.ToString(),
                smodif = "E",
                nordct = xoperac.ElementAt(swipedRowIndex - 1).nordct.ToString(),
                npieza = Int32.Parse(xoperac.ElementAt(swipedRowIndex - 1).npieza.ToString()),
                dpieza = xoperac.ElementAt(swipedRowIndex - 1).dpieza.ToString(),
                clotei = xoperac.ElementAt(swipedRowIndex - 1).clotei.ToString(),
                citems = xoperac.ElementAt(swipedRowIndex - 1).citems.ToString(),
                ditems = xoperac.ElementAt(swipedRowIndex - 1).ditems.ToString(),
                cencog = xoperac.ElementAt(swipedRowIndex - 1).cencog.ToString(),
                dtalla = xoperac.ElementAt(swipedRowIndex - 1).dtalla.ToString(),
                qprend = Int32.Parse(xoperac.ElementAt(swipedRowIndex - 1).qprend.ToString()),
                npanos = Int32.Parse(xoperac.ElementAt(swipedRowIndex - 1).npanos.ToString()),
                }
            };
            using (var data = new DataAccess())
            {
                var audenvio = data.GetList<paudit01>(false).Where(x => x.careas == areaau && x.nsecue == Int32.Parse(xoperac.ElementAt(swipedRowIndex - 1).nsecue.ToString()) && x.clinea == xoperac.ElementAt(swipedRowIndex - 1).clinea.ToString() && x.faudit == DateTime.Parse(xoperac.ElementAt(swipedRowIndex - 1).faudit.ToString()) && x.senvio=="S").ToList();
                if (audenvio.Count>0)
                {
                    DisplayAlert("Aviso", "No se puede modificar auditoria", "OK");
                    return;
                }
            }
            if (areaau == "19") App.Navigator.PushAsync(new CosturaProcesoPage(dataok));
            if (areaau == "FC") App.Navigator.PushAsync(new CosturaFinalPage(dataok));
            if (areaau == "16") App.Navigator.PushAsync(new AuditoriaCortePage(dataok));
            if (areaau == "29") App.Navigator.PushAsync(new AuditoriaBordadoPage(dataok));

        }

        private void rightImage_Swiping_BindingContextChanged(object sender, EventArgs e)
        {
            if (rightImage == null)
            {                
                rightImage = sender as Image;
                (rightImage.Parent as View).GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(Delete) });
                //rightImage.Source = ImageSource.FromResource("AppPFashions.Android.Resources.Eliminar.png");
            }           
        }

        private async void Delete()
        {
            var result = await alertService.ShowMessage("Aviso", "Desea eliminar el registro.");
            if (result == true)
            {
                using (var data = new DataAccess())
                {
                    taudit00 taudit = new taudit00
                    {
                        idaudi = xoperac.ElementAt(swipedRowIndex - 1).idaudi,
                        careas = xoperac.ElementAt(swipedRowIndex - 1).careas.ToString(),
                        faudit = DateTime.Parse(xoperac.ElementAt(swipedRowIndex - 1).faudit.ToString()),
                        nsecue = Int32.Parse(xoperac.ElementAt(swipedRowIndex - 1).nsecue.ToString()),
                        clinea = xoperac.ElementAt(swipedRowIndex - 1).clinea.ToString(),
                        nordpr = xoperac.ElementAt(swipedRowIndex - 1).nordpr.ToString(),
                        ccarub = xoperac.ElementAt(swipedRowIndex - 1).ccarub.ToString(),
                        dcarub = xoperac.ElementAt(swipedRowIndex - 1).dcarub.ToString(),
                        ctraba = xoperac.ElementAt(swipedRowIndex - 1).ctraba.ToString(),
                        copera = xoperac.ElementAt(swipedRowIndex - 1).copera.ToString(),
                        dopera = xoperac.ElementAt(swipedRowIndex - 1).dopera.ToString(),
                        dclien = xoperac.ElementAt(swipedRowIndex - 1).dclien.ToString(),
                        nlotes = Int32.Parse(xoperac.ElementAt(swipedRowIndex - 1).nlotes.ToString()),
                        nmuest = Int32.Parse(xoperac.ElementAt(swipedRowIndex - 1).nmuest.ToString()),
                        status = xoperac.ElementAt(swipedRowIndex - 1).status.ToString(),
                        dobser = xoperac.ElementAt(swipedRowIndex - 1).dobser.ToString(),
                        smodif = "R",
                    };
                    data.Delete(taudit);
                }
            }
            else // if it's equal to Cancel
            {
                return; // just return to the page and do nothing.
            }

            LoadDetalleAuditorias(nclinea);
        }

        private void dataGrid_SwipeEnded(object sender, SwipeEndedEventArgs e)
        {
            //formView.BindingContext = e.RowData;
            swipedRowIndex = e.RowIndex;
        }

        private void dataGrid_GridTapped(object sender, GridTappedEventArgs e)
        {      
            dataGrid.Opacity = 1.0;
            dataGrid.IsEnabled = true;
        }
    }
}