using AppPFashions.Data;
using AppPFashions.Interfaces;
using AppPFashions.Models;
using AppPFashions.Services;
using AppPFashions.Templates;
using Syncfusion.SfKanban.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppPFashions.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : INotifyPropertyChanged
    {
        IDownloader downloader = DependencyService.Get<IDownloader>();
        public ObservableCollection<CustomKanbanModel> Cards { get; set; }
        public ObservableCollection<CustomKanbanModel> CardsNull { get; set; }
        List<taudit00> auditcp;
        List<taudit00> audites;
        List<taudit00> audittr;
        List<taudit00> auditcf;
        taudit00 xoperac;
        mtraba00 ldtraba;
        string xccargo,xcusuar;
        public string dusuar { get; set; }
        public string taudit { get; set; }

        private ApiService apiService;

        public UserPage()
        {
            apiService = new ApiService();
            InitializeComponent();
            //downloader.OnFileDownloaded += OnFileDownloaded;

            BindingContext = this;

            //CargaCards();            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //using (var data = new DataAccess())
            //{
                var user = App.baseDatos.GetUsuario();
                xccargo = user.ccargo;
                xcusuar = user.cusuar;
            //}
            CargaCards();
            //FechaApk();            
        }

        async void FechaApk()
        {
            string response = await apiService.GetFechaApk();            
            DateTime dtapkserver = DateTime.Parse(response.Trim(new char[] {'"'}));
            //FileInfo fi = new FileInfo("/storage/emulated/0/Download/Auditoria.apk");
            DateTime dtapkmobile = File.GetLastWriteTime("/storage/emulated/0/Download/Auditoria.apk");
            
            if (dtapkserver > DateTime.Parse(dtapkmobile.ToString("dd/MM/yyyy HH:mm:ss")))
            {
                if (await DisplayAlert("Aviso", "Existe una actualización para la aplicación, desea descargarla ahora?", "Si", "No"))
                {
                    DependencyService.Get<IDownloader>().Show("Descargando");
                    string rutapdf = "ftp://192.168.2.55/Auditoria.apk";
                    downloader.DownloadFile(rutapdf, "Download");
                }
            }
        }

        private void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                DependencyService.Get<IDownloader>().Hide();
            }
            else
            {
                DisplayAlert("Aviso", "Hubo un error al descargar el archivo", "OK");
            }
        }

        async void CargaCards()
        {
            kanban.Columns.Clear();

            //CardsNull = new ObservableCollection<CustomKanbanModel>();
            //kanban.ItemsSource = CardsNull;
            if (xccargo == "34")
            {
                kanban.Columns.Add(new Syncfusion.SfKanban.XForms.KanbanColumn()
                {
                    Title = "Costura Proceso",
                    Categories = new List<object>() { "Costura Proceso" },
                });

                kanban.Columns.Add(new Syncfusion.SfKanban.XForms.KanbanColumn()
                {
                    Title = "Costura Final",
                    Categories = new List<object>() { "Costura Final" },
                });
            }
            if (xccargo == "05")
            {
                kanban.Columns.Add(new Syncfusion.SfKanban.XForms.KanbanColumn()
                {
                    Title = "Corte",
                    Categories = new List<object>() { "Corte" },
                });
            }

            if (xccargo == "57" || xccargo == "56")
            {
                kanban.Columns.Add(new Syncfusion.SfKanban.XForms.KanbanColumn()
                {
                    Title = "Bordado",
                    Categories = new List<object>() { "Bordado" },
                });

                kanban.Columns.Add(new Syncfusion.SfKanban.XForms.KanbanColumn()
                {
                    Title = "Estampado",
                    Categories = new List<object>() { "Estampado" },
                });

                kanban.Columns.Add(new Syncfusion.SfKanban.XForms.KanbanColumn()
                {
                    Title = "Transfer",
                    Categories = new List<object>() { "Transfer" },
                });
            } 

            try
            {
                //using (var data = new DataAccess())
                //{
                    Cards = new ObservableCollection<CustomKanbanModel>();

                    if (xccargo=="34")
                    { 
                        auditcp = App.baseDatos.GetList<taudit00>(false).Where(x => x.careas == "19" && x.status=="D" && x.sreaud=="N").OrderBy(x=>x.faudit).ToList();
                        TimeSpan ndiascp;
                        TimeSpan ndiascf;                
                        foreach (var recordcp in auditcp)
                        {
                            ndiascp = DateTime.Now - recordcp.faudit;
                            ldtraba = App.baseDatos.GetList<mtraba00>(false).Where(x => x.ctraba == recordcp.ctraba).FirstOrDefault();                    
                            Cards.Add(new CustomKanbanModel()
                            {
                                ID = 1,
                                Nordpr = recordcp.nordpr,
                                Clinea = recordcp.clinea,
                                Dclien = recordcp.dclien,
                                Dopera = recordcp.dopera,
                                Ndiast = ndiascp.Days + " día(s)",                        
                                ImageURL = "circulo_navy.png",
                                Category = "Costura Proceso",                                               
                                ColorKey = "Navy",           
                                //Rating = recordcp.ndefec,
                                Dtraba = ldtraba.ctraba +" - "+ldtraba.dtraba,
                                Careas = recordcp.careas,
                                Faudit = recordcp.faudit,
                                Nsecue = recordcp.nsecue,
                            });                             
                        }

                        auditcf = App.baseDatos.GetList<taudit00>(false).Where(x => x.careas == "FC" && x.status == "D" && x.sreaud == "N").OrderBy(x=>x.faudit).ToList();
                        foreach (var recordcf in auditcf)
                        {
                            ndiascf = DateTime.Now - recordcf.faudit;
                            Cards.Add(new CustomKanbanModel()
                            {
                                ID = 1,
                                Nordpr = recordcf.nordpr,
                                Clinea = recordcf.clinea,
                                Dclien = recordcf.dclien,                        
                                Ndiast = ndiascf.Days + " día(s)",
                                ImageURL = "circulo_purple.png",
                                Category = "Costura Final",                                                
                                ColorKey = "Purple",
                                //Rating = recordcf.ndefec,
                                Dcolor = recordcf.ccarub+" - "+recordcf.dcarub,
                                Careas = recordcf.careas,
                                Faudit = recordcf.faudit,
                                Nsecue = recordcf.nsecue,
                            });
                        }
                    }
                    if (xccargo == "05")
                    {
                        auditcp = App.baseDatos.GetList<taudit00>(false).Where(x => x.careas == "16" && x.status == "D" && x.sreaud == "N").OrderBy(x => x.faudit).ToList();
                        TimeSpan ndiascp;
                        TimeSpan ndiascf;
                        foreach (var recordcp in auditcp)
                        {
                            ndiascp = DateTime.Now - recordcp.faudit;
                            ldtraba = App.baseDatos.GetList<mtraba00>(false).Where(x => x.ctraba == recordcp.ctraba).FirstOrDefault();
                            Cards.Add(new CustomKanbanModel()
                            {
                                ID = 1,
                                Nordpr = recordcp.nordpr,
                                Clinea = recordcp.clinea,
                                Dclien = recordcp.dclien,
                                Dopera = recordcp.nordct,
                                Ndiast = ndiascp.Days + " día(s)",
                                ImageURL = "circulo_navy.png",
                                Category = "Corte",
                                ColorKey = "Navy",
                                Rating = recordcp.ndefec,
                                Dtraba = recordcp.faudit.ToShortDateString(),
                                Careas = recordcp.careas,
                                Faudit = recordcp.faudit,
                                Nsecue = recordcp.nsecue,
                            });
                        }
                    }

                if (xccargo == "57")
                {
                    auditcp = App.baseDatos.GetList<taudit00>(false).Where(x => x.careas == "29" && x.status == "D" && x.sreaud == "N").OrderBy(x => x.faudit).ToList();
                    TimeSpan ndiascp;                    
                    foreach (var recordcp in auditcp)
                    {
                        ndiascp = DateTime.Now - recordcp.faudit;
                        ldtraba = App.baseDatos.GetList<mtraba00>(false).Where(x => x.ctraba == recordcp.ctraba).FirstOrDefault();
                        Cards.Add(new CustomKanbanModel()
                        {
                            ID = 1,
                            Nordpr = recordcp.nordpr,
                            Clinea = recordcp.cmaqui,
                            Dclien = recordcp.dclien,
                            Dopera = recordcp.nordct,
                            Ndiast = ndiascp.Days + " día(s)",
                            ImageURL = "circulo_navy.png",
                            Category = "Bordado",
                            ColorKey = "Navy",
                            //Rating = recordcp.ndefec,
                            Dtraba = recordcp.faudit.ToString("dd-MM-yyyy"),
                            Careas = recordcp.careas,
                            Faudit = recordcp.faudit,
                            Nsecue = recordcp.nsecue,
                        });
                    }

                    audites = App.baseDatos.GetList<taudit00>(false).Where(x => x.careas == "33" && x.status == "D" && x.sreaud == "N").OrderBy(x => x.faudit).ToList();
                    TimeSpan ndiases;                    
                    foreach (var recordes in audites)
                    {
                        ndiases = DateTime.Now - recordes.faudit;
                        ldtraba = App.baseDatos.GetList<mtraba00>(false).Where(x => x.ctraba == recordes.ctraba).FirstOrDefault();
                        Cards.Add(new CustomKanbanModel()
                        {
                            ID = 1,
                            Nordpr = recordes.nordpr,
                            Clinea = recordes.cmaqui,
                            Dclien = recordes.dclien,
                            Dopera = recordes.nordct,
                            Ndiast = ndiases.Days + " día(s)",
                            ImageURL = "circulo_navy.png",
                            Category = "Estampado",
                            ColorKey = "Purple",
                            //Rating = recordcp.ndefec,
                            Dtraba = recordes.faudit.ToString("dd-MM-yyyy"),
                            Careas = recordes.careas,
                            Faudit = recordes.faudit,
                            Nsecue = recordes.nsecue,
                        });
                    }

                    audittr = App.baseDatos.GetList<taudit00>(false).Where(x => x.careas == "31" && x.status == "D" && x.sreaud == "N").OrderBy(x => x.faudit).ToList();
                    TimeSpan ndiastr;                    
                    foreach (var recordtr in audittr)
                    {
                        ndiastr = DateTime.Now - recordtr.faudit;
                        ldtraba = App.baseDatos.GetList<mtraba00>(false).Where(x => x.ctraba == recordtr.ctraba).FirstOrDefault();
                        Cards.Add(new CustomKanbanModel()
                        {
                            ID = 1,
                            Nordpr = recordtr.nordpr,
                            Clinea = recordtr.cmaqui,
                            Dclien = recordtr.dclien,
                            Dopera = recordtr.nordct,
                            Ndiast = ndiastr.Days + " día(s)",
                            ImageURL = "circulo_navy.png",
                            Category = "Transfer",
                            ColorKey = "Navy",
                            //Rating = recordcp.ndefec,
                            Dtraba = recordtr.faudit.ToString("dd-MM-yyyy"),
                            Careas = recordtr.careas,
                            Faudit = recordtr.faudit,
                            Nsecue = recordtr.nsecue,
                        });
                    }

                }

                kanban.ItemsSource = Cards;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", ex.StackTrace, "OK");
            }

        }


        public void User()
        {
            //using (var data = new DataAccess())
            //{
                var user = App.baseDatos.GetUsuario();
                VariableGlobal.ctraba = user.ctraba;
                VariableGlobal.dtraba = user.dusuar;
            //}
            dusuar = VariableGlobal.ctraba +" - "+  VariableGlobal.dtraba;
        }

        private async void Kanban_ItemTapped(object sender, KanbanTappedEventArgs e)
        {
            string newclinea;
            try
            {       
                if (await DisplayAlert("Aviso", "Desea realizar la reauditoria", "Si", "No"))
                {
                    var selkanban = (e.Data) as CustomKanbanModel;
                    //using (var data = new DataAccess())
                    //{
                    if (xccargo == "56" || xccargo == "57") { newclinea = ""; }
                    else { newclinea = selkanban.Clinea; }

                    xoperac = App.baseDatos.GetList<taudit00>(false).Where(x => x.nsecue == selkanban.Nsecue && x.clinea == newclinea && x.status == "D" && x.careas == selkanban.Careas && x.faudit.ToString("dd-MM-yyyy") == selkanban.Faudit.ToString("dd-MM-yyyy")).FirstOrDefault();
                    //}

                    //using (var data = new DataAccess())
                    //{
                        var audenvio = App.baseDatos.GetList<taudit00>(false).Where(x => x.careas == selkanban.Careas && x.clinea == newclinea && x.nsecue == selkanban.Nsecue && x.faudit.ToString("dd-MM-yyyy") == selkanban.Faudit.ToString("dd-MM-yyyy") && x.sreaud == "S").ToList();
                        if (audenvio.Count > 0)
                        {
                            await DisplayAlert("Aviso", "Ya se realizo la reauditoria", "OK");
                            return;
                        }
                    //}

                    var dataok = new List<taudit00>
                    {
                        new taudit00
                        {
                        idaudi = xoperac.idaudi,
                        careas = xoperac.careas.ToString(),
                        faudit = DateTime.Parse(xoperac.faudit.ToString()),
                        nsecue = Int32.Parse(xoperac.nsecue.ToString()),
                        clinea = xoperac.clinea.ToString(),
                        nordpr = xoperac.nordpr.ToString(),
                        ccarub = xoperac.ccarub.ToString(),
                        dcarub = xoperac.dcarub.ToString(),
                        ctraba = xoperac.ctraba.ToString(),
                        copera = xoperac.copera.ToString(),
                        dopera = xoperac.dopera.ToString(),
                        dclien = xoperac.dclien.ToString(),
                        nlotes = Int32.Parse(xoperac.nlotes.ToString()),
                        nmuest = Int32.Parse(xoperac.nmuest.ToString()),
                        status = xoperac.status.ToString(),
                        dobser = xoperac.dobser.ToString(),
                        smodif = "R",
                        nordct = xoperac.nordct.ToString(),
                        npieza = Int32.Parse(xoperac.npieza.ToString()),
                        dpieza = xoperac.dpieza.ToString(),
                        clotei = xoperac.clotei.ToString(),
                        citems = xoperac.citems.ToString(),
                        ditems = xoperac.ditems.ToString(),
                        cencog = xoperac.cencog.ToString(),
                        dtalla = xoperac.dtalla.ToString(),
                        qprend = Int32.Parse(xoperac.qprend.ToString()),
                        npanos = Int32.Parse(xoperac.npanos.ToString()),
                        cmaqui = xoperac.cmaqui.ToString(),
                        cturno = xoperac.cturno.ToString()
                        }
                    };
                
                    if (selkanban.Careas == "19") App.Navigator.PushAsync(new CosturaProcesoPage(dataok));
                    if (selkanban.Careas == "FC") App.Navigator.PushAsync(new CosturaFinalPage(dataok));
                    if (selkanban.Careas == "16") App.Navigator.PushAsync(new AuditoriaCortePage(dataok));
                    if (selkanban.Careas == "29") App.Navigator.PushAsync(new AuditoriaBordadoPage(dataok));
                    if (selkanban.Careas == "33") App.Navigator.PushAsync(new AuditoriaEstampadoPage(dataok));
                    if (selkanban.Careas == "31") App.Navigator.PushAsync(new AuditoriaTransferPage(dataok));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", ex.StackTrace, "OK");
            }
        }

        private void Tlb_viewpdf_Clicked(object sender, EventArgs e)
        {
            App.Navigator.PushAsync(new ProductionOrderPage());
        }
    }

    public class KanbanTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate costuraprocesoTemplate;

        private readonly DataTemplate costurafinalTemplate;

        //private readonly DataTemplate readyToServeTemplate;

        //private readonly DataTemplate deliveryTemplate;


        public KanbanTemplateSelector()
        {
            costuraprocesoTemplate = new DataTemplate(typeof(CosturaProcesoTemplate));
            costurafinalTemplate = new DataTemplate(typeof(CosturaFinalTemplate));
            //readyToServeTemplate = new DataTemplate(typeof(ReadyToServeTemplate));
            //deliveryTemplate = new DataTemplate(typeof(DeliveryTemplate));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var data = item as CustomKanbanModel;
            if (data == null)
                return null;

            string category = data.Category?.ToString();

            if (category == null)
                return null;

            return category.Equals("Costura Proceso") ? costuraprocesoTemplate : 
                           category.Equals("Corte") ? costuraprocesoTemplate :
                           category.Equals("Costura Final") ? costurafinalTemplate : costuraprocesoTemplate;
        }
    }

    public class CustomKanbanModel : KanbanModel
    {
        public string Careas
        {
            get;
            set;
        }
        public string Clinea
        {
            get;
            set;
        }
        public string Nordpr
        {
            get;
            set;
        }
        public string Dclien
        {
            get;
            set;
        }
        public string Dopera
        {
            get;
            set;
        } 
        public string Ndiast
        {
            get;
            set;
        }
        public string Dtraba
        {
            get;
            set;
        }
        public string Dcolor
        {
            get;
            set;
        }
        public int Nsecue
        {
            get;
            set;
        }
        public DateTime Faudit
        {
            get;
            set;
        }
        public float Rating
        {
            get;
            set;
        }
        public string OrderID
        {
            get;
            set;
        }

        public int AnimationDuration
        {
            get;
            set;
        }
    }
}