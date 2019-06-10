using Acr.UserDialogs;
using AppPFashions.Data;
using AppPFashions.Models;
using AppPFashions.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppPFashions.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResumenAuditoriasPage : INotifyPropertyChanged
    {
        public ObservableCollection<AuditFolder> Folders { get; set; }

        public ObservableCollection<SubFolder> SubFolders { get; set; }
              
        private ApiService apiService;
        private DialogService dialogService;
        private AlertService alertService;
        List<taudit00> xoperac;
        List<taudit00> xoperacs;
        string taudit01;
        string dimgdef = "";
        string desauditoria;
        int newnsecue;

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

        public ResumenAuditoriasPage (string taudit)
		{
            apiService = new ApiService();
            dialogService = new DialogService();
            taudit01 = taudit;

            InitializeComponent();
            BindingContext = this;

            if (taudit01 == "19") Desauditoria = "Auditoria Costura Proceso - Bloques";
            if (taudit01 == "FC") Desauditoria = "Auditoria Costura Final - Bloques";
            if (taudit01 == "16") Desauditoria = "Auditoria Corte - Modulos";
            if (taudit01 == "29") Desauditoria = "Auditoria Bordado";
            LoadResumenAuditorias();
        }

        protected override void OnAppearing()
        {
            LoadResumenAuditorias();
            base.OnAppearing();
        }

        async void LoadResumenAuditorias()
        {
            try
            {
                Folders = GenerateItems();
                treeView.ItemsSource = Folders;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", ex.StackTrace, "OK");
            }                      
        }

        private ObservableCollection<AuditFolder> GenerateItems()
        {
            int conta = 1;
            var folders = new ObservableCollection<AuditFolder>(); 
            using (var data = new DataAccess())
            {
                xoperac = data.GetList<taudit00>(false).Where(x => x.careas == taudit01).OrderBy(x=>x.clinea).ToList();
                var taudit02 = (from a in xoperac
                                group a by new
                                {
                                    a.clinea,                                    
                                }
                       into b
                                select new
                                {
                                    Clinea = b.Key.clinea,
                                    Qtaudi = b.Count()
                                }).ToList();
                foreach (var record in taudit02)
                {
                    var subfol01 = new SubFolder();
                    var subfolders = new ObservableCollection<SubFolder>();
                    var fol01 = new AuditFolder() { FolderName = "Bloque "+record.Clinea , ImageName= "ic_bloque.jpg", AuditCount = record.Qtaudi, FontBold=1 };
                    conta = 1;

                    xoperacs = data.GetList<taudit00>(false).Where(x => x.careas == taudit01 && x.clinea==record.Clinea).OrderByDescending(x=>x.faudit).ToList();
                    var taudit03 = (from a in xoperacs
                                    group a by new
                                    {
                                        a.faudit.Date,
                                    }
                           into b
                                    select new
                                    {
                                        Faudit = b.Key.Date,                                        
                                    }).ToList();

                    foreach (var recorda in taudit03)
                    {
                        int qaudit = data.GetList<taudit00>(false).Where(x => x.careas == taudit01 && x.clinea == record.Clinea && x.faudit.Date==recorda.Faudit.Date).ToList().Count();
                        int qaudia = data.GetList<taudit00>(false).Where(x => x.careas == taudit01 && x.clinea == record.Clinea && x.faudit.Date == recorda.Faudit.Date && x.status=="A").ToList().Count();
                        int qaudid = data.GetList<taudit00>(false).Where(x => x.careas == taudit01 && x.clinea == record.Clinea && x.faudit.Date == recorda.Faudit.Date && x.status == "D").ToList().Count();
                        int qaudie = data.GetList<taudit00>(false).Where(x => x.careas == taudit01 && x.clinea == record.Clinea && x.faudit.Date == recorda.Faudit.Date && x.status == "E").ToList().Count();

                        if ( conta == 1 )
                        { 
                            subfol01 = new SubFolder() { FolderName = recorda.Faudit.ToString("dd-MM-yyyy"), ImageName = "ic_calendar_now.png", AuditCount = qaudit , Clinea=record.Clinea};
                            subfol01.DetFolder = new ObservableCollection<DetFolder>
                            {
                                new DetFolder() { FolderName = "Aprobado", ImageName= "ic_aprobado.png" , AuditCount = qaudia , Careas=taudit01, Status="A", Clinea=record.Clinea, Faudit=recorda.Faudit.ToString("dd-MM-yyyy")},
                                new DetFolder() { FolderName = "Desaprobado", ImageName= "ic_desaprobado.png" , AuditCount = qaudid, Careas=taudit01, Status="D", Clinea=record.Clinea, Faudit=recorda.Faudit.ToString("dd-MM-yyyy") },
                                new DetFolder() { FolderName = "Aprobado Ext.", ImageName= "ic_aprobext.png" , AuditCount = qaudie, Careas=taudit01, Status="E", Clinea=record.Clinea, Faudit=recorda.Faudit.ToString("dd-MM-yyyy") }
                            };
                            subfolders.Add(subfol01);
                        }
                        if (conta > 1 )
                        {
                            subfol01 = new SubFolder() { FolderName = recorda.Faudit.ToString("dd-MM-yyyy"), ImageName = "ic_calendar_past.png", AuditCount = qaudit, Clinea = record.Clinea};
                            subfol01.DetFolder = new ObservableCollection<DetFolder>
                            {
                                new DetFolder() { FolderName = "Aprobado",ImageName= "ic_aprobado.png" , AuditCount = qaudia , Careas=taudit01, Status="A", Clinea=record.Clinea, Faudit=recorda.Faudit.ToString("dd-MM-yyyy")},
                                new DetFolder() { FolderName = "Desaprobado", ImageName= "ic_desaprobado.png" , AuditCount = qaudid , Careas=taudit01, Status="D", Clinea=record.Clinea, Faudit=recorda.Faudit.ToString("dd-MM-yyyy") },
                                new DetFolder() { FolderName = "Aprobado Ext.", ImageName= "ic_aprobext.png", AuditCount = qaudie , Careas=taudit01, Status="E", Clinea=record.Clinea, Faudit=recorda.Faudit.ToString("dd-MM-yyyy")}
                            };
                            subfolders.Add(subfol01);
                        }
                        conta = conta + 1;
                    }
                    fol01.SubFolder = subfolders;
                    folders.Add(fol01);
                }
            }
                            
            return folders;
        }

        private void TreeView_ItemTapped(object sender, Syncfusion.XForms.TreeView.ItemTappedEventArgs e)
        {
            var selaudit = (e.Node.Content) as DetFolder;
            if (selaudit != null)
            {
                //DisplayAlert("Error", selaudit.Careas+ selaudit.Status+ selaudit.Clinea+ selaudit.Faudit, "OK");
                if (selaudit.AuditCount > 0)
                {
                    if (selaudit.Status == "D")
                    {
                        App.Navigator.PushAsync(new CosturaProcesoReauditoriaPage(selaudit.Clinea + selaudit.Status + selaudit.Careas + selaudit.Faudit));                        
                    }
                    else
                    {
                        App.Navigator.PushAsync(new CosturaProcesoDetallePage(selaudit.Clinea + selaudit.Status + selaudit.Careas + selaudit.Faudit));
                    }
                }
                else
                {
                    DisplayAlert("Aviso", "No existen auditorías con estado "+selaudit.FolderName+" en la fecha "+selaudit.Faudit,  "OK");
                }
            }
        }

        private void fab_nuevaauditoria_Clicked(object sender, EventArgs e)
        {
            //App.Navigator.PushAsync(new ImageEditorPage());
            List<taudit00> dataok = new List<taudit00>();
            if (taudit01 == "19") App.Navigator.PushAsync(new CosturaProcesoPage(dataok));
            if (taudit01 == "FC") App.Navigator.PushAsync(new CosturaFinalPage(dataok));
            if (taudit01 == "16") App.Navigator.PushAsync(new AuditoriaCortePage(dataok));
            if (taudit01 == "29") App.Navigator.PushAsync(new AuditoriaBordadoPage(dataok));
        }

        private void tlb_sincroauditoria_Clicked(object sender, EventArgs e)
        {
            GrabaAuditoriaDB();
            //GrabaDefectoDB();            
        }

        async void GrabaAuditoriaDB()
        {
            var ultregis = 0;
            using (var client = new HttpClient())
            {
                using (var data = new DataAccess())
                {
                    var listickets = data.GetList<paudit01>(false).Where(x => x.senvio == "N" && x.careas == taudit01).OrderBy(x=>x.nsecue).ToList();
                    int qlistau = listickets.Count();
                    int xqaudi = 0;
                    using (IProgressDialog fooDialog = UserDialogs.Instance.Progress("Sincronizando...", null, null, true, MaskType.Black))
                    {
                        foreach (var record in listickets)
                        {
                            var responseur = await apiService.GetUltRegistro(record.careas, record.faudit.Date.ToString("yyyy-MM-dd"), record.clinea);
                            if (!responseur.IsSuccess)
                            {
                                ultregis = 0;
                            }
                            else
                            {
                                var dataultreg = (paudit02)responseur.Result;
                                if (dataultreg.clinea != null)
                                {
                                    ultregis = dataultreg.ultreg;
                                }
                            }

                            if (record.flgrau == "S")
                            {
                                var listnsecueref = data.GetList<paudit01>(false).Where(x => x.careas == taudit01 && x.clinea == record.clinea && x.copera == record.copera && x.ctraba == record.ctraba && x.ccarub == record.ccarub && x.nordpr==record.nordpr && x.status == "D").FirstOrDefault();
                                newnsecue = listnsecueref.nseref;
                            }
                            else
                            {
                                newnsecue = 0;
                            }

                            var novoPost = new paudit01
                            {
                                careas = record.careas,
                                faudit = DateTime.Parse(record.faudit.ToShortDateString()),
                                nsecue = ultregis + 1,//record.nsecue,
                                clinea = record.clinea,
                                ctpord = record.ctpord,
                                nordpr = record.nordpr,
                                nordct = record.nordct,
                                cmarbe = record.cmarbe,
                                ccarub = record.ccarub,
                                npieza = record.npieza,
                                cencog = record.cencog,
                                citems = record.citems,
                                ccolor = record.ccolor,
                                npanos = record.npanos,
                                dtalla = record.dtalla,
                                qtotal = record.qtotal,
                                dlotes = record.dlotes,
                                ctraba = record.ctraba,
                                copera = record.copera,
                                cprove = record.cprove,
                                cmaqui = record.cmaqui,
                                cturno = record.cturno,
                                cparti = record.cparti,
                                nordco = record.nordco,
                                npacki = record.npacki,
                                nlotes = record.nlotes,
                                nmuest = record.nmuest,
                                pcierr = record.pcierr,
                                nrecup = record.nrecup,
                                nsegun = record.nsegun,
                                porcen = record.porcen,
                                flgcie = record.flgcie,
                                ndefec = record.ndefec,
                                status = record.status,
                                dobser = record.dobser,
                                flgrau = record.flgrau,
                                nreaud = record.nreaud,
                                caudit = record.caudit,
                                flgext = record.flgext,
                                cliref = record.cliref,
                                fauref = DateTime.Parse(record.fauref.ToShortDateString()),
                                nseref = newnsecue,//record.nseref,
                                cusuar = record.cusuar,
                                fcreac = DateTime.Parse(record.fcreac.ToShortDateString()),
                                fmodif = DateTime.Parse(record.fmodif.ToShortDateString()),
                                nordpo = record.nordpo,
                                fprogr = DateTime.Parse(record.fprogr.ToShortDateString()),
                                caudpr = record.caudpr,
                                drefpr = record.drefpr,
                                ctpaud = record.ctpaud,
                                csuppl = record.csuppl,
                                flgenv = record.flgenv,
                                sanula = record.sanula,
                                ndesap = record.ndesap,
                                sautab = record.sautab,
                            };

                            var json = JsonConvert.SerializeObject(novoPost);
                            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                            var uri = "http://192.168.1.3:7030/api/paudit01";
                            var result = await client.PostAsync(uri, content);

                            if (!result.IsSuccessStatusCode)
                            {
                                await DisplayAlert("Error", "Hubo un error con el Servicio Web, Por favor reintente guardar. " + result.RequestMessage.ToString(), "OK");
                                return;
                            }

                            var resultString = await result.Content.ReadAsStringAsync();
                            var post = JsonConvert.DeserializeObject<paudit01>(resultString);


                            using (var datadef = new DataAccess())
                            {
                                int qdefec = datadef.GetList<pdefec01>(false).Where(x => x.senvio == "N" && x.faudit == record.faudit && x.clinea == record.clinea && x.nsecue == record.nsecue && x.careas == taudit01).Count();
                                if (qdefec > 0)
                                {
                                    var lisdef = datadef.GetList<pdefec01>(false).Where(x => x.senvio == "N" && x.faudit == record.faudit && x.clinea == record.clinea && x.nsecue == record.nsecue && x.careas == taudit01).ToList();
                                    foreach (var recorddef in lisdef)
                                    {
                                        var lisdefimg = datadef.GetList<pdefec10>(false).Where(x => x.faudit == recorddef.faudit && x.clinea == recorddef.clinea && x.nsecue == recorddef.nsecue && x.careas == taudit01 && x.coddef == recorddef.coddef).ToList();
                                        foreach (var recorddefimg in lisdefimg)
                                        {
                                            dimgdef = recorddefimg.defjpg;
                                        }

                                        var novoPostdef = new pdefec01
                                        {
                                            careas = recorddef.careas,
                                            faudit = DateTime.Parse(recorddef.faudit.ToShortDateString()),
                                            nsecue = ultregis + 1,//recorddef.nsecue,
                                            clinea = recorddef.clinea,
                                            codigo = recorddef.codigo,
                                            coddef = recorddef.coddef,
                                            qcanti = recorddef.qcanti,
                                            dobser = recorddef.dobser,
                                            cgrupo = recorddef.cgrupo,
                                            cardef = recorddef.cardef,
                                            imgdef = dimgdef,

                                        };

                                        var jsondef = JsonConvert.SerializeObject(novoPostdef);
                                        var contentdef = new StringContent(jsondef, System.Text.Encoding.UTF8, "application/json");

                                        var uridef = "http://192.168.1.3:7030/api/pdefec01";
                                        var resultdef = await client.PostAsync(uridef, contentdef);

                                        if (!result.IsSuccessStatusCode)
                                        {
                                            await DisplayAlert("Error", "Hubo un error con el Servicio Web, Por favor reintente guardar. " + resultdef.RequestMessage.ToString(), "OK");
                                            return;
                                        }

                                        var resultStringdef = await result.Content.ReadAsStringAsync();
                                        var postdef = JsonConvert.DeserializeObject<pdefec01>(resultStringdef);

                                        using (var dataimg = new DataAccess())
                                        {
                                            int qimgde = data.GetList<pdefec10>(false).Where(x => x.careas == taudit01 && x.faudit == recorddef.faudit && x.clinea == recorddef.clinea && x.nsecue == recorddef.nsecue && x.vimage == true && x.iddefe == recorddef.iddefe).Count();
                                            if (qimgde > 0)
                                            {
                                                var ldefecimg = data.GetList<pdefec10>(false).Where(x => x.careas == taudit01 && x.faudit == recorddef.faudit && x.clinea == recorddef.clinea && x.nsecue == recorddef.nsecue && x.iddefe == recorddef.iddefe).ToList();
                                                foreach (var recordimg in ldefecimg)
                                                {
                                                    var contentimg = new MultipartFormDataContent();
                                                    var path = "/storage/emulated/0/Pictures/Test/" + recordimg.defjpg;

                                                    contentimg.Add(new ByteArrayContent(File.ReadAllBytes(path)), "file", recordimg.defjpg);

                                                    var httpClient = new HttpClient();
                                                    var uploadServiceBaseAddress = "http://192.168.1.3:7030/api/Upload";
                                                    var httpResponseMessage = await httpClient.PostAsync(uploadServiceBaseAddress, contentimg);

                                                    await httpResponseMessage.Content.ReadAsStringAsync();
                                                }
                                            }
                                        }

                                        pdefec01 ndefec = new pdefec01
                                        {
                                            iddefe = recorddef.iddefe,
                                            careas = recorddef.careas,
                                            faudit = DateTime.Parse(recorddef.faudit.ToShortDateString()),
                                            nsecue = recorddef.nsecue,
                                            clinea = recorddef.clinea,
                                            codigo = recorddef.codigo,
                                            coddef = recorddef.coddef,
                                            qcanti = recorddef.qcanti,
                                            dobser = recorddef.dobser,
                                            cgrupo = recorddef.cgrupo,
                                            cardef = recorddef.cardef,
                                            senvio = "S",
                                            imgdef = recorddef.imgdef,
                                        };
                                        datadef.Update(ndefec);
                                    }
                                }
                            }


                            paudit01 nauditoria = new paudit01
                            {
                                idaudi = record.idaudi,
                                careas = record.careas,
                                faudit = record.faudit,
                                nsecue = record.nsecue,
                                clinea = record.clinea,
                                ctpord = record.ctpord,
                                nordpr = record.nordpr,
                                nordct = record.nordct,
                                cmarbe = record.cmarbe,
                                ccarub = record.ccarub,
                                npieza = record.npieza,
                                cencog = record.cencog,
                                citems = record.citems,
                                ccolor = record.ccolor,
                                npanos = record.npanos,
                                dtalla = record.dtalla,
                                qtotal = record.qtotal,
                                dlotes = record.dlotes,
                                ctraba = record.ctraba,
                                copera = record.copera,
                                cprove = record.cprove,
                                cmaqui = record.cmaqui,
                                cturno = record.cturno,
                                cparti = record.cparti,
                                nordco = record.nordco,
                                npacki = record.npacki,
                                nlotes = record.nlotes,
                                nmuest = record.nmuest,
                                pcierr = record.pcierr,
                                nrecup = record.nrecup,
                                nsegun = record.nsegun,
                                porcen = record.porcen,
                                flgcie = record.flgcie,
                                ndefec = record.ndefec,
                                status = record.status,
                                dobser = record.dobser,
                                flgrau = record.flgrau,
                                nreaud = record.nreaud,
                                caudit = record.caudit,
                                flgext = record.flgext,
                                cliref = record.cliref,
                                fauref = record.fauref,
                                nseref = ultregis + 1,
                                cusuar = record.cusuar,
                                fcreac = record.fcreac,
                                fmodif = record.fmodif,
                                nordpo = record.nordpo,
                                fprogr = record.fprogr,
                                caudpr = record.caudpr,
                                drefpr = record.drefpr,
                                ctpaud = record.ctpaud,
                                csuppl = record.csuppl,
                                flgenv = record.flgenv,
                                sanula = record.sanula,
                                ndesap = record.ndesap,
                                sautab = record.sautab,
                                senvio = "S",
                            };
                            data.Update(nauditoria);

                            xqaudi = xqaudi + 1;
                            fooDialog.PercentComplete = xqaudi;
                            fooDialog.Title = xqaudi + " de " + qlistau;
                            await Task.Delay(10);
                        }
                    }
                }
            }
            //await DisplayAlert("Aviso", "Los datos se guardaron de manera satisfactoria.", "OK");
        }

        async void GrabaDefectoDB()
        {
            using (var client = new HttpClient())
            {
                using (var data = new DataAccess())
                {
                    var listickets = data.GetList<pdefec01>(false).Where(x => x.senvio == "N").ToList();
                    foreach (var record in listickets)
                    {
                        var novoPost = new pdefec01
                        {
                            careas = record.careas,
                            faudit = DateTime.Parse(record.faudit.ToShortDateString()),
                            nsecue = record.nsecue,
                            clinea = record.clinea,
                            codigo = record.codigo,
                            coddef = record.coddef,
                            qcanti = record.qcanti,
                            dobser = record.dobser,
                            cgrupo = record.cgrupo,
                            cardef = record.cardef,
                        };

                        var json = JsonConvert.SerializeObject(novoPost);
                        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                        var uri = "http://192.168.1.3:7030/api/pdefec01";
                        var result = await client.PostAsync(uri, content);

                        if (!result.IsSuccessStatusCode)
                        {
                            await DisplayAlert("Error", "Hubo un error con el Servicio Web, Por favor reintente guardar. " + result.RequestMessage.ToString(), "OK");
                            return;
                        }

                        var resultString = await result.Content.ReadAsStringAsync();
                        var post = JsonConvert.DeserializeObject<pdefec01>(resultString);

                        pdefec01 ndefec = new pdefec01
                        {
                            iddefe = record.iddefe,
                            careas = record.careas,
                            faudit = DateTime.Parse(record.faudit.ToShortDateString()),
                            nsecue = record.nsecue,
                            clinea = record.clinea,
                            codigo = record.codigo,
                            coddef = record.coddef,
                            qcanti = record.qcanti,
                            dobser = record.dobser,
                            cgrupo = record.cgrupo,
                            cardef = record.cardef,
                            senvio = "S",
                        };
                        data.Update(ndefec);
                    }
                }
            }
        }

        private async void Tlb_deleteauditoria_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Aviso", "Desea eliminar todas las auditorias", "Si", "No");
            if (result == true)
            {
                using (var data = new DataAccess())
                {
                    //var listickets = data.GetList<paudit01>(false).Where(x => x.senvio == "N").ToList();
                    //int qlistau = listickets.Count();
                    //if (qlistau > 0)
                    //{
                    //    await DisplayAlert("Alerta", "Existen auditorias que aun no ha sincronizado, por favor sincronice antes de eliminar", "OK");
                    //    return;
                    //}
                    data.DeleteAllAuditoria();
                    LoadResumenAuditorias();                    
                }
                await DisplayAlert("Aviso", "Auditorias eliminadas", "OK");

            }
        }

        private async void TreeView_ItemHolding(object sender, Syncfusion.XForms.TreeView.ItemHoldingEventArgs e)
        {
            var selaudit = (e.Node.Content) as SubFolder;
            if (selaudit != null)
            {
                var result = await DisplayAlert("Aviso", "Desea eliminar las auditorias con fecha "+selaudit.FolderName, "Si", "No");
                if (result == true)
                {
                    using (var data = new DataAccess())
                    {
                        var listickets = data.GetList<paudit01>(false).Where(x => x.faudit.ToString("dd-MM-yyyy") == selaudit.FolderName && x.clinea==selaudit.Clinea && x.senvio == "N").ToList();
                        int qlistau = listickets.Count();
                        if (qlistau > 0)
                        {
                            await DisplayAlert("Alerta", "Existen auditorias sin sincronizar, por favor sincronice antes de eliminar", "OK");
                            return;
                        }
                        data.DeleteAuditoria(DateTime.Parse(selaudit.FolderName),selaudit.Clinea);
                        LoadResumenAuditorias();
                    }
                    await DisplayAlert("Aviso", "Auditorias eliminadas", "OK");

                }
            }
        }
    }
}