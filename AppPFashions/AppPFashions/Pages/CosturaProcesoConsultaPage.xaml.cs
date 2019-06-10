using Acr.UserDialogs;
using AppPFashions.Data;
using AppPFashions.Models;
using AppPFashions.Services;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppPFashions.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CosturaProcesoConsultaPage : INotifyPropertyChanged
    {
        private ApiService apiService;
        private DialogService dialogService;
        private AlertService alertService;
        List<caudit00> xoperac;
        string taudit01;
        string dimgdef="";
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
        public CosturaProcesoConsultaPage (string taudit)
		{
            apiService = new ApiService();
            dialogService = new DialogService();
            taudit01 = taudit;         

            InitializeComponent ();
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

        private void Fab_agregarauditoria_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Aviso", "Seleccionado", "OK");
        }

        async void LoadResumenAuditorias()
        {
            try
            {
                using (var data = new DataAccess())
                {                    
                    xoperac = data.GetList<caudit00>(false).Where(x=>x.Careas== taudit01).ToList();
                    listView.ItemsSource = xoperac;

                    #region Metodo_Anterior
                    //var taudit01 = (from a in xoperac
                    //                group a by new
                    //                {
                    //                    a.careas,
                    //                    a.clinea,
                    //                    a.faudit
                    //                }
                    //          into b
                    //                select new
                    //                {
                    //                    nareas = b.Key.careas,
                    //                    nclinea = b.Key.clinea,
                    //                    nfaudit = b.Key.faudit,
                    //                }).ToList();

                    //var sqprime = (from a in xoperac
                    //               where a.flgrau == "N"
                    //               group a by new
                    //               {
                    //                   a.careas,
                    //                   a.clinea,
                    //                   a.faudit
                    //               }
                    //            into b
                    //               select new
                    //               {
                    //                   nareas = b.Key.careas,
                    //                   nclinea = b.Key.clinea,
                    //                   nfaudit = b.Key.faudit,
                    //                   nqprime = b.Count(),
                    //               }).ToList();

                    //var sqrecha = (from a in xoperac
                    //               where a.flgrau == "N" && a.status == "D"
                    //               group a by new
                    //               {
                    //                   a.careas,
                    //                   a.clinea,
                    //                   a.faudit
                    //               }
                    //         into b
                    //               select new
                    //               {
                    //                   nareas = b.Key.careas,
                    //                   nclinea = b.Key.clinea,
                    //                   nfaudit = b.Key.faudit,
                    //                   nqrecha = b.Count(),
                    //               }).ToList();

                    //var sqreaud = (from a in xoperac
                    //               where a.flgrau == "S"
                    //               group a by new
                    //               {
                    //                   a.careas,
                    //                   a.clinea,
                    //                   a.faudit
                    //               }
                    //       into b
                    //               select new
                    //               {
                    //                   nareas = b.Key.careas,
                    //                   nclinea = b.Key.clinea,
                    //                   nfaudit = b.Key.faudit,
                    //                   nqreaud = b.Count(),
                    //               }).ToList();

                    //var sqrerea = (from a in xoperac
                    //               where a.flgrau == "S" && a.status == "D"
                    //               group a by new
                    //               {
                    //                   a.careas,
                    //                   a.clinea,
                    //                   a.faudit
                    //               }
                    //       into b
                    //               select new
                    //               {
                    //                   nareas = b.Key.careas,
                    //                   nclinea = b.Key.clinea,
                    //                   nfaudit = b.Key.faudit,
                    //                   nqrerea = b.Count(),
                    //               }).ToList();

                    //var ftaudit01 = (from a in taudit01
                    //                 join b in sqprime on
                    //                 new { a.nareas, a.nclinea, a.nfaudit }
                    //                 equals
                    //                 new { b.nareas, b.nclinea, b.nfaudit }
                    //                 into result1
                    //                 from newresult in result1.DefaultIfEmpty()
                    //join c in sqrecha on
                    //new { a.nareas, a.nclinea, a.nfaudit }
                    //equals
                    //new { c.nareas, c.nclinea, c.nfaudit }
                    //into result2
                    //from newresult1 in result2.DefaultIfEmpty()
                    //join d in sqreaud on
                    //new { a.nareas, a.nclinea, a.nfaudit }
                    //equals
                    //new { d.nareas, d.nclinea, d.nfaudit }
                    //into result3
                    //from newresult2 in result3.DefaultIfEmpty()
                    //join e in sqrerea on
                    //new { a.nareas, a.nclinea, a.nfaudit }
                    //equals
                    //new { e.nareas, e.nclinea, e.nfaudit }
                    //into result4
                    //from newresult3 in result4.DefaultIfEmpty()
                    //select new paudit10
                    //{
                    //    clinea = a.nclinea,
                    //    qprime = newresult.nqprime,
                    //    //qrecha = newresult1.nqrecha,
                    //    //qreaud = newresult2.nqreaud,
                    //    //qrerea = newresult3.nqrerea
                    //}).ToList();

                    //lsv_resumenauditoria.ItemsSource = xoperac;
                    //(from a in ftaudit01
                    //                                 group a by new
                    //                                 {
                    //                                     a.clinea
                    //                                 }
                    //                                into g
                    //                                 select new
                    //                                 {
                    //                                     fclinea = g.Key.clinea,
                    //                                     fqprime = g.Sum(n => n.qprime),
                    //                                     //fqrecha = g.Sum(n => n.qrecha),
                    //                                     //fqreaud = g.Sum(n => n.qreaud),
                    //                                     //fqrerea = g.Sum(n => n.qrerea),
                    //                                 }).ToList(); 
                    #endregion
                }
    
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", ex.StackTrace, "OK");
            }

            //var response = await apiService.GetAuditoriaResumen<paudit10>("19",DateTime.Parse("2018-11-10"));

            //if (!response.IsSuccess)
            //{
            //    await dialogService.ShowMessage("Error", response.Message);
            //    return;
            //}

            //var opera = (List<paudit10>)response.Result;                        
        }

        private void tgrlsv_defectos_Tapped(object sender, TappedEventArgs e)
        {            
            var sellinea = (e.Parameter) as paudit01;
            //DisplayAlert("Aviso", seldefecto.clinea.ToString(), "OK");
            App.Navigator.PushAsync(new CosturaProcesoDetallePage(sellinea.clinea.ToString()));
        }

        private void fab_nuevaauditoria_Clicked(object sender, EventArgs e)
        {
            //App.Navigator.PushAsync(new ImageEditorPage());
            List<taudit00> dataok = new List<taudit00>();
            if (taudit01=="19") App.Navigator.PushAsync(new CosturaProcesoPage(dataok));
            if (taudit01 == "FC") App.Navigator.PushAsync(new CosturaFinalPage(dataok));
            if (taudit01 == "16") App.Navigator.PushAsync(new AuditoriaCortePage(dataok));
            if (taudit01 == "29") App.Navigator.PushAsync(new AuditoriaBordadoPage(dataok));
        }

        private void lbl_audiaprob_Tapped(object sender, TappedEventArgs e)
        {
            var sellinea = (e.Parameter) as caudit00;
            App.Navigator.PushAsync(new CosturaProcesoDetallePage(sellinea.Clinea.ToString()+"A"+ taudit01));
        }

        private void lbl_audiapext_Tapped(object sender, TappedEventArgs e)
        {
            var sellinea = (e.Parameter) as caudit00;
            App.Navigator.PushAsync(new CosturaProcesoDetallePage(sellinea.Clinea.ToString() + "E"+ taudit01));
        }

        private void lbl_audidesap_Tapped(object sender, TappedEventArgs e)
        {
            var sellinea = (e.Parameter) as caudit00;
            App.Navigator.PushAsync(new CosturaProcesoReauditoriaPage(sellinea.Clinea.ToString() + "D"+taudit01));
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
                    var listickets = data.GetList<paudit01>(false).Where(x => x.senvio == "N" && x.careas == taudit01).ToList();
                    int qlistau = listickets.Count();
                    int xqaudi = 0;
                    using (IProgressDialog fooDialog = UserDialogs.Instance.Progress("Sincronizando...", null, null, true, MaskType.Black))
                    {
                        foreach (var record in listickets)
                        {

                            var responseur = await apiService.GetUltRegistro(record.careas, record.faudit.ToShortDateString(), record.clinea);
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
                                nseref = record.nseref,
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
                                            int qimgde = data.GetList<pdefec10>(false).Where(x => x.careas == taudit01 && x.faudit == recorddef.faudit && x.clinea == recorddef.clinea && x.nsecue == recorddef.nsecue && x.vimage == true && x.iddefe==recorddef.iddefe).Count();
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
                                nseref = record.nseref,
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
                    data.DeleteAllAuditoria();
                    LoadResumenAuditorias();
                }
                    await DisplayAlert("Aviso", "Auditorias eliminadas", "OK");
              
            }            
        }
    }
}