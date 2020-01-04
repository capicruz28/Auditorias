using Acr.UserDialogs;
using AppPFashions.Data;
using AppPFashions.Interfaces;
using AppPFashions.Models;
using AppPFashions.Services;
using PCLStorage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppPFashions.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuditoriaTransferPage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Attributes        
        private DialogService dialogService;
        private AlertService alertService;
        private ApiService apiService;
        private DataService dataService;
        private DataAccess dataAccess;
        string estadoReauditoria = "N";
        string xcoddef, xdedef;
        string xcliref = "";
        DateTime xfauref = DateTime.Parse("1900-01-01 00:00:00");
        Int32 xnseref = 0;
        int xqaprob, xqdesap, xqaprex;
        string sapaud = "";
        string nuevaAuditoria;
        string _bloque;
        string sgraud;
        Int32 xidaudi;
        string obsdefecto;
        string obsaudit;
        string saudtot;
        string xcitems, cmaquina, xcturno, xcmaquina;
        #endregion

        public List<string> listMaquina { get; set; }
        public List<string> listTurno { get; set; }
        public List<taudit00> listaCargaAuditoria { get; set; }
        public List<pdefec10> listaCargaDefectos { get; set; }

        public AuditoriaTransferPage(List<taudit00> listaAuditoria)
        {
            dialogService = new DialogService();
            alertService = new AlertService();
            apiService = new ApiService();
            dataService = new DataService();
            List<pdefec10> xopera = new List<pdefec10>();
            listMaquina = new List<string>
            {
                "","01 MAQUINA SR","02 MAQUINA MHM","03 MAQUINA MANUAL","04 MAQUINA DESARROLLO","05 MAQUINA 01","06 MAQUINA 02","07 MAQUINA 03"
            };

            listTurno = new List<string>
            {
                "M MAÑANA","N NOCHE"
            };

            this.listaCargaAuditoria = listaAuditoria;
            InitializeComponent();

            nuevaAuditoria = "S";
            sgraud = "N";
            pck_maquina.ItemsSource = listMaquina;
            pck_turno.ItemsSource = listTurno;
            if (listaCargaAuditoria.Count != 0)
            {
                CargaAuditoria();
            }

            App.baseDatos.DeleteDefectoTemp();
            var user = App.baseDatos.GetUsuario();
            VariableGlobal.ctraba = user.ctraba;
            VariableGlobal.cusuar = user.cusuar;
        }


        #region ControlRetroceso
        protected override bool OnBackButtonPressed()
        {
            // Begin an asyncronous task on the UI thread because we intend to ask the users permission.
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (ety_op.Text != null)
                {
                    if (await DisplayAlert("Alerta", "Estas seguro que quiere salir sin guardar los cambios", "Si", "No"))
                    {
                        base.OnBackButtonPressed();
                        await App.Navigator.PopAsync();
                    }
                }
                else
                {
                    base.OnBackButtonPressed();
                    await App.Navigator.PopAsync();
                }
            });
            return true;
        }
        #endregion

        #region CargaAuditoria
        void CargaAuditoria()
        {
            if (listaCargaAuditoria.ElementAt(0).cturno.ToString().Trim() == "M") { xcturno = "M MAÑANA"; }
            if (listaCargaAuditoria.ElementAt(0).cturno.ToString().Trim() == "N") { xcturno = "N NOCHE"; }
            if (string.IsNullOrEmpty(listaCargaAuditoria.ElementAt(0).cmaqui.ToString().Trim())) { xcmaquina = ""; }
            else { xcmaquina = listaCargaAuditoria.ElementAt(0).cmaqui.ToString().Trim() + " MAQUINA"; }

            //********** REAUDITORIA **********//
            if (listaCargaAuditoria.ElementAt(0).status.ToString() == "D" && listaCargaAuditoria.ElementAt(0).smodif.ToString() == "R")
            {
                if (Int32.Parse(listaCargaAuditoria.ElementAt(0).nlotes.ToString()) == Int32.Parse(listaCargaAuditoria.ElementAt(0).nmuest.ToString()))
                {
                    cbxaudittot.IsChecked = true;
                }
                else
                {
                    cbxaudittot.IsChecked = false;
                }
                nuevaAuditoria = "S";
                estadoReauditoria = "S";
                xcliref = listaCargaAuditoria.ElementAt(0).clinea.ToString();
                xfauref = DateTime.Parse(listaCargaAuditoria.ElementAt(0).faudit.ToString());
                xnseref = Int32.Parse(listaCargaAuditoria.ElementAt(0).nsecue.ToString());

                dpk_fechaauditoria.Date = DateTime.Parse(DateTime.Now.ToShortDateString());//DateTime.Parse(listaCargaAuditoria.ElementAt(0).faudit.ToString());
                ety_op.Text = listaCargaAuditoria.ElementAt(0).nordpr.ToString();
                BuscarOP();
                ety_corte.Text = listaCargaAuditoria.ElementAt(0).nordct.ToString();
                ety_lote.Text = listaCargaAuditoria.ElementAt(0).nlotes.ToString();
                ety_muestra.Text = listaCargaAuditoria.ElementAt(0).nmuest.ToString();
                ety_observ.Text = listaCargaAuditoria.ElementAt(0).dobser.ToString();
                if (xcmaquina == "") { pck_maquina.SelectedItem = null; } else { pck_maquina.SelectedItem = xcmaquina; }                
                pck_turno.SelectedItem = xcturno;
                srb_audidesaprobado.IsChecked = true;
                ety_op.IsEnabled = false;
                btn_buscarop.IsEnabled = false;
                pck_combo.IsEnabled = false;
                img_defecto.IsEnabled = false;
                pck_defectos.IsEnabled = false;
                ety_cantdefecto.IsEnabled = false;
                ety_obsdefecto.IsEnabled = false;
                btn_agregardefecto.IsEnabled = false;
                btn_guardarauditoria.IsEnabled = false;
                ety_lote.IsEnabled = false;
                ety_muestra.IsEnabled = false;
                btn_agregarauditoria.IsEnabled = false;
                btn_guardarauditoria.IsEnabled = true;
                cbxaudittot.IsEnabled = false;

                var datosSecuencia = App.baseDatos.GetList<paudit01>(false).Where(a => a.faudit.ToShortDateString() == dpk_fechaauditoria.Date.ToShortDateString() && a.careas == "31").ToList();
                if (datosSecuencia.Count > 0)
                {
                    var ultimaAuditoria = App.baseDatos.GetList<paudit01>(false).Where(a => a.faudit.ToShortDateString() == dpk_fechaauditoria.Date.ToShortDateString() && a.careas == "31").OrderByDescending(x => x.nsecue).First();
                    lbl_nsecue.Text = (ultimaAuditoria.nsecue + 1).ToString();
                }
                else
                { lbl_nsecue.Text = "1"; }

                var datosId = App.baseDatos.GetList<paudit01>(false);
                if (datosId.Count > 0)
                {
                    var ultimoIdAuditoria = App.baseDatos.GetList<paudit01>(false).OrderByDescending(x => x.idaudi).First();
                    xidaudi = ultimoIdAuditoria.idaudi + 1;
                }
                else
                { xidaudi = 1; }

            }
            //********** EDITAR AUDITORIA **********//
            else
            {
                if (Int32.Parse(listaCargaAuditoria.ElementAt(0).nlotes.ToString()) == Int32.Parse(listaCargaAuditoria.ElementAt(0).nmuest.ToString()))
                    if (Int32.Parse(listaCargaAuditoria.ElementAt(0).nlotes.ToString()) == Int32.Parse(listaCargaAuditoria.ElementAt(0).nmuest.ToString()))
                    {
                        cbxaudittot.IsChecked = true;
                    }
                    else
                    {
                        cbxaudittot.IsChecked = false;
                    }
                nuevaAuditoria = "N";
                estadoReauditoria = "N";
                ety_op.Text = listaCargaAuditoria.ElementAt(0).nordpr.ToString();
                BuscarOP();
                dpk_fechaauditoria.Date = DateTime.Parse(listaCargaAuditoria.ElementAt(0).faudit.ToString());
                lbl_nsecue.Text = listaCargaAuditoria.ElementAt(0).nsecue.ToString();
                ety_lote.Text = listaCargaAuditoria.ElementAt(0).nlotes.ToString();
                ety_muestra.Text = listaCargaAuditoria.ElementAt(0).nmuest.ToString();
                ety_observ.Text = listaCargaAuditoria.ElementAt(0).dobser.ToString();
                pck_maquina.SelectedItem = xcmaquina;
                pck_turno.SelectedItem = xcturno;
                if (listaCargaAuditoria.ElementAt(0).status.ToString() == "A") srb_audiaprobado.IsChecked = true;
                if (listaCargaAuditoria.ElementAt(0).status.ToString() == "D") srb_audidesaprobado.IsChecked = true;
                if (listaCargaAuditoria.ElementAt(0).status.ToString() == "E") srb_audiaprobadoext.IsChecked = true;
                //using (var data = new DataAccess())
                //{
                listaCargaDefectos = App.baseDatos.GetList<pdefec10>(false).Where(x => x.clinea == listaCargaAuditoria.ElementAt(0).clinea.ToString() && x.faudit.Date == listaCargaAuditoria.ElementAt(0).faudit.Date && x.nsecue == listaCargaAuditoria.ElementAt(0).nsecue).ToList();
                lsv_defectos.ItemsSource = listaCargaDefectos;
                //}
                ety_op.IsEnabled = true;
                btn_buscarop.IsEnabled = true;
                ety_corte.IsEnabled = true;
                pck_combo.IsEnabled = true;
                img_defecto.IsEnabled = true;
                pck_defectos.IsEnabled = true;
                ety_cantdefecto.IsEnabled = true;
                ety_obsdefecto.IsEnabled = true;
                btn_agregardefecto.IsEnabled = true;
                btn_guardarauditoria.IsEnabled = true;
                ety_lote.IsEnabled = true;
                ety_muestra.IsEnabled = true;
                pck_maquina.IsEnabled = true;
                pck_turno.IsEnabled = true;
            }
        }
        #endregion

        #region NuevaAuditoria
        public void AgregarNuevaAuditoria()
        {
            //using (var data = new DataAccess())
            //{
            var datosSecuencia = App.baseDatos.GetList<paudit01>(false).Where(a => a.careas == "31" && a.faudit.ToShortDateString() == dpk_fechaauditoria.Date.ToShortDateString()).ToList();
            if (datosSecuencia.Count > 0)
            {
                var ultimaAuditoria = App.baseDatos.GetList<paudit01>(false).Where(a => a.careas == "31" && a.faudit.ToShortDateString() == dpk_fechaauditoria.Date.ToShortDateString()).OrderByDescending(x => x.nsecue).First();
                lbl_nsecue.Text = (ultimaAuditoria.nsecue + 1).ToString();
            }
            else
            { lbl_nsecue.Text = "1"; }
            //}
            //using (var data = new DataAccess())
            //{
            var datosId = App.baseDatos.GetList<paudit01>(false);
            if (datosId.Count > 0)
            {
                var ultimoIdAuditoria = App.baseDatos.GetList<paudit01>(false).OrderByDescending(x => x.idaudi).First();
                xidaudi = ultimoIdAuditoria.idaudi + 1;
            }
            else
            { xidaudi = 1; }
            //}
            ety_op.IsEnabled = true;
            btn_buscarop.IsEnabled = true;
            ety_corte.IsEnabled = true;
            pck_combo.IsEnabled = true;
            pck_maquina.IsEnabled = true;
            pck_turno.IsEnabled = true;
            img_defecto.IsEnabled = true;
            pck_defectos.IsEnabled = true;
            ety_cantdefecto.IsEnabled = true;
            ety_obsdefecto.IsEnabled = true;
            btn_agregardefecto.IsEnabled = true;
            btn_guardarauditoria.IsEnabled = true;
            ety_lote.IsEnabled = true;
            ety_muestra.IsEnabled = true;
            ety_observ.IsEnabled = true;
            btn_agregarauditoria.IsEnabled = false;
        }
        #endregion
        private void btn_agregarauditoria_Clicked(object sender, EventArgs e)
        {
            //DisplayAlert("Error", "Ingrese un número de OP", DateTime.Now.ToString());
            AgregarNuevaAuditoria();
        }

        void BuscarOperario()
        {

        }
        private void btn_ctraba_Clicked(object sender, EventArgs e)
        {
            BuscarOperario();
        }


        #region BuscarOP
        async void BuscarOP()
        {
            if (string.IsNullOrEmpty(ety_op.Text))
            {
                await DisplayAlert("Error", "Ingrese un número de OP", "OK");
                ety_op.Focus();
                return;
            }

            DependencyService.Get<IDownloader>().Show("Cargando");
            var response = await apiService.GetOP<ordprod>(ety_op.Text);
            if (!response.IsSuccess)
            {
                await DisplayAlert("Error", response.Message, "OK");
                ety_op.Focus();
                DependencyService.Get<IDownloader>().Hide();
                return;
            }

            DependencyService.Get<IDownloader>().Hide();

            var dataop = (List<ordprod>)response.Result;
            foreach (var record in dataop.OrderBy(x => x.nordct))
            {
                lbl_descliente.Text = record.dclien;
            }

            //*** Llena Picker Combo ***//
            pck_combo.Items.Clear();
            var datos = (from dat in dataop
                         group dat by new { dat.ccarub, dat.dcarub } into g
                         select new { ccarub = g.Key.ccarub, dcarub = g.Key.dcarub });
            foreach (var record in datos)
            {
                pck_combo.Items.Add(record.ccarub.Trim() + " - " + record.dcarub.Trim());
            }

            if (listaCargaAuditoria.Count != 0)
            {
                ety_corte.Text = listaCargaAuditoria.ElementAt(0).nordct.ToString().Trim();
                pck_combo.SelectedItem = listaCargaAuditoria.ElementAt(0).ccarub.ToString().Trim() + " - " + listaCargaAuditoria.ElementAt(0).dcarub.ToString().Trim();
                //pck_pieza.SelectedItem = listaCargaAuditoria.ElementAt(0).npieza.ToString().Trim() + " - " + listaCargaAuditoria.ElementAt(0).dpieza.ToString().Trim();
            }

            //*** Llena Picker Defectos ***//            
            //using (var data = new DataAccess())
            //{
            var xdefec = App.baseDatos.GetList<mdefec00>(false).Where(x => x.csecci == "31");

            if (xdefec == null)
            {
                await dialogService.ShowMessage("Error", "No existen registros");
                return;
            }
            pck_defectos.DataSource = xdefec.OrderBy(x => x.coddef).ToList();
            //}    
        }
        #endregion

        private void btn_buscarop_Clicked(object sender, EventArgs e)
        {
            BuscarOP();
        }

        private async void img_defectos_Tapped(object sender, EventArgs e)
        {
            var result = await alertService.ShowMessage("Aviso", "Desea sincronizar la lista de defectos");
            if (result == true)
            {                
                App.baseDatos.DeleteDefectos("31");
                var response = await apiService.Defectos<mdefec00>("31");

                if (!response.IsSuccess)
                {
                    await dialogService.ShowMessage("Error", response.Message);
                    return;
                }

                var opera = (List<mdefec00>)response.Result;
                int xqtraba = opera.Count();
                int xtraba = 0;

                using (IProgressDialog fooDialog = UserDialogs.Instance.Progress("Sincronizando...", null, null, true, MaskType.Black))
                {
                    foreach (var record in opera)
                    {
                        mdefec00 defecto = new mdefec00
                        {
                            coddef = record.coddef,
                            descri = record.descri,
                            dgrupo = record.dgrupo,
                            codigo = record.codigo,
                            dapare = record.dapare,
                            csecci = record.csecci,
                            ddefec = record.ddefec,
                        };
                        dataService.InsertDefecto(defecto);
                        xtraba = xtraba + 1;
                        fooDialog.PercentComplete = xtraba;
                        fooDialog.Title = xtraba + " de " + xqtraba;
                        await Task.Delay(10);
                    }
                }

                var xdefec = App.baseDatos.GetList<mdefec00>(false).Where(x => x.csecci == "31");

                if (xdefec == null)
                {
                    await dialogService.ShowMessage("Error", "No existen registros");
                    return;
                }
                pck_defectos.DataSource = xdefec.OrderBy(x => x.coddef).ToList();

            }
            else // if it's equal to Cancel
            {
                return; // just return to the page and do nothing.
            }
        }

        private void AddDefecto()
        {
            if (pck_defectos.SelectedItem == null)
            {
                DisplayAlert("Aviso", "Debe ingresar una cantidad de defectos", "OK");
                return;
            }

            if (Int32.Parse(ety_cantdefecto.Value.ToString()) == 0)
            {
                DisplayAlert("Aviso", "Catidad de defectos tiene que ser mayor a 0", "OK");
                ety_cantdefecto.Focus();
                return;
            }

            try
            {
                int qdefec = App.baseDatos.GetList<pdefec10>(false).Where(x => x.careas == "31" && x.nsecue == Int32.Parse(lbl_nsecue.Text.ToString()) && x.coddef == pck_defectos.SelectedItem.ToString().Substring(0, 2) && x.faudit.ToShortDateString() == dpk_fechaauditoria.Date.ToShortDateString()).Count();
                if (qdefec > 0)
                {
                    DisplayAlert("Aviso", "Defecto ya fue registrado", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(ety_obsdefecto.Text))
                {
                    obsdefecto = "";
                }
                else
                {
                    obsdefecto = ety_obsdefecto.Text;
                }
                pdefec10 cdefecto = new pdefec10
                {
                    careas = "31",
                    faudit = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                    nsecue = Int32.Parse(lbl_nsecue.Text.ToString()),
                    clinea = lbl_bloquedef.Text,
                    codigo = "01",
                    coddef = xcoddef,//pck_defectos.SelectedItem.ToString().Substring(0, 2),
                    qcanti = Int32.Parse(ety_cantdefecto.Value.ToString()),
                    dobser = obsdefecto,//ety_obsdefecto.Text = null ?? "",
                    cgrupo = "N",
                    cardef = "31",
                    descri = xdedef,//pck_defectos.SelectedItem.ToString().Substring(5, (pck_defectos.SelectedItem.ToString().Length - 5)),//pck_defectos.SelectedValue.ToString()
                    defjpg = "",
                    vimage = false,
                    vphoto = true,
                    svigen = "N",
                };
                App.baseDatos.Insert(cdefecto);

                listaCargaDefectos = App.baseDatos.GetList<pdefec10>(false);
                lsv_defectos.ItemsSource = listaCargaDefectos.Where(x => x.nsecue == Int32.Parse(lbl_nsecue.Text.ToString()) && x.careas == "31" && x.faudit.ToShortDateString() == dpk_fechaauditoria.Date.ToShortDateString());
                pck_defectos.SelectedItem = null;
                ety_obsdefecto.Text = "";
                ety_cantdefecto.Value = 0;
                //}
                //DisplayAlert("Aviso", "Los datos se guardaron de manera satisfactoria.","OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Aviso", ex.StackTrace, "OK");
            }
        }

        private void LimpiaAuditoria()
        {
            lbl_nsecue.Text = "";
            ety_op.Text = "";
            lbl_descliente.Text = "";
            ety_corte.Text = null;
            pck_combo.SelectedItem = null;
            ety_lote.Text = "";
            ety_muestra.Text = "";
            ety_observ.Text = "";
            lbl_bloquedef.Text = "";
            lbl_fechadef.Text = "";
            pck_defectos.SelectedItem = null;
            pck_maquina.SelectedItem = null;
            pck_turno.SelectedItem = null;
            ety_obsdefecto.Text = "";
            ety_cantdefecto.Value = 0;
            if (listaCargaDefectos != null)
            {
                listaCargaDefectos.Clear();
                lsv_defectos.ItemsSource = listaCargaDefectos;
            }
            btn_buscarop.IsEnabled = false;
            ety_op.IsEnabled = false;
            ety_corte.IsEnabled = false;
            pck_combo.IsEnabled = false;
            pck_maquina.IsEnabled = false;
            pck_turno.IsEnabled = false;
            ety_lote.IsEnabled = false;
            ety_muestra.IsEnabled = false;
            ety_observ.IsEnabled = false;
            pck_defectos.IsEnabled = false;
            ety_obsdefecto.IsEnabled = false;
            ety_cantdefecto.IsEnabled = false;
            btn_agregardefecto.IsEnabled = false;
            btn_guardarauditoria.IsEnabled = false;
            srb_audiaprobado.IsChecked = true;
            srb_audiaprobadoext.IsChecked = false;
            srb_audidesaprobado.IsChecked = false;
            btn_agregarauditoria.IsEnabled = true;
        }

        #region ResumenAuditoria
        void ActualizaResumenAuditoria()
        {
            if (pck_maquina.SelectedItem!=null)
            {
                cmaquina = pck_maquina.SelectedItem.ToString().Substring(0, 2);                
            }
            else
            {
                cmaquina = "";
            }
            //using (var data = new DataAccess())
            //{
            xqaprob = 0;
            xqdesap = 0;
            xqaprex = 0;
            var getresaudit = App.baseDatos.GetResaudit("31", DateTime.Parse(dpk_fechaauditoria.Date.ToString()), cmaquina);
            if (nuevaAuditoria == "S")
            {
                if (getresaudit != null)
                {
                    xqaprob = getresaudit.qaprob;
                    xqdesap = getresaudit.qdesap;
                    xqaprex = getresaudit.qaprex;

                    raudit00 resaudit = new raudit00
                    {
                        idraud = getresaudit.idraud,
                        careas = "31",
                        faudit = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                        clinea = cmaquina,
                        qaprob = xqaprob + Int32.Parse(sapaud == "A" ? "1" : "0"),
                        qdesap = xqdesap + Int32.Parse(sapaud == "D" ? "1" : "0"),
                        qaprex = xqaprex + Int32.Parse(sapaud == "E" ? "1" : "0"),
                    };
                    App.baseDatos.Update(resaudit);
                }
                else
                {
                    raudit00 resaudit = new raudit00
                    {
                        careas = "31",
                        faudit = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                        clinea = cmaquina,
                        qaprob = xqaprob + Int32.Parse(sapaud == "A" ? "1" : "0"),
                        qdesap = xqdesap + Int32.Parse(sapaud == "D" ? "1" : "0"),
                        qaprex = xqaprex + Int32.Parse(sapaud == "E" ? "1" : "0"),
                    };
                    App.baseDatos.Insert(resaudit);
                }
            }
            //}
        }
        #endregion

        #region GuardaAuditoria
        private void AddAuditoria()
        {
            try
            {
                if (string.IsNullOrEmpty(ety_op.Text))
                {
                    DisplayAlert("Error", "Ingrese un número de OP", "OK");
                    ety_op.Focus();
                    return;
                }

                if (pck_combo.SelectedItem == null)
                {
                    DisplayAlert("Aviso", "Debe seleccionar el color del combo", "OK");
                    return;
                }

                if (string.IsNullOrEmpty(ety_corte.Text))
                {
                    DisplayAlert("Error", "Ingrese un número de Corte/Marbete", "OK");
                    ety_op.Focus();
                    return;
                }

                if (pck_turno.SelectedItem == null)
                {
                    DisplayAlert("Aviso", "Debe seleccionar un turno", "OK");
                    return;
                }

                if (string.IsNullOrEmpty(ety_lote.Text))
                {
                    DisplayAlert("Error", "Ingrese la cantidad del lote", "OK");
                    ety_lote.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(ety_muestra.Text))
                {
                    DisplayAlert("Error", "Ingrese la cantidad de la muestra", "OK");
                    ety_muestra.Focus();
                    return;
                }
                //using (var data = new DataAccess())
                //{
                int qdefec = App.baseDatos.GetList<pdefec10>(false).Where(x => x.careas == "31" && x.nsecue == Int32.Parse(lbl_nsecue.Text.ToString()) && x.coddef == xcoddef && x.faudit.ToShortDateString() == dpk_fechaauditoria.Date.ToShortDateString()).Count();
                if (pck_defectos.SelectedItem != null && qdefec == 0)
                {
                    DisplayAlert("Aviso", "Aun no agrego el defecto seleccionado", "OK");
                    return;
                }
                //}

                Int32 qdefectos;
                if (srb_audiaprobado.IsChecked == true) sapaud = "A";
                if (srb_audiaprobadoext.IsChecked == true) sapaud = "E";
                if (srb_audidesaprobado.IsChecked == true) sapaud = "D";

                if (string.IsNullOrWhiteSpace(ety_observ.Text))
                {
                    obsaudit = "";
                }
                else
                {
                    obsaudit = ety_observ.Text;
                }

                if (pck_maquina.SelectedItem != null )
                {
                    cmaquina = pck_maquina.SelectedItem.ToString().Substring(0, 2);                    
                }
                else
                {
                    cmaquina = "";
                }

     
                if (nuevaAuditoria == "S")
                {
                    //using (var data = new DataAccess())
                    //{
                    qdefectos = App.baseDatos.GetList<pdefec10>(false).Where(x => x.careas == "31" && x.nsecue == Int32.Parse(lbl_nsecue.Text.ToString()) && x.faudit.ToShortDateString() == dpk_fechaauditoria.Date.ToShortDateString()).Sum(x => x.qcanti);

                    paudit01 nauditoria = new paudit01
                    {
                        idaudi = xidaudi,
                        careas = "31",
                        faudit = DateTime.Parse(dpk_fechaauditoria.Date.ToShortDateString().Trim()),
                        nsecue = Int32.Parse(lbl_nsecue.Text.ToString().Trim()),
                        clinea = "",
                        ctpord = "OP",
                        nordpr = ety_op.Text.ToString().Trim(),
                        nordct = ety_corte.Text.ToString().Trim(),
                        cmarbe = "",
                        ccarub = pck_combo.SelectedItem.ToString().Substring(0, pck_combo.SelectedItem.ToString().IndexOf("-") - 1),
                        npieza = 0,
                        cencog = "",
                        citems = "",
                        ccolor = "",
                        npanos = 0,
                        dtalla = "",
                        qtotal = 0,
                        dlotes = "",
                        ctraba = "",// ety_ctraba.Text.ToString(),
                        copera = "",//pck_operacion.SelectedItem.ToString().Substring(0, pck_operacion.SelectedItem.ToString().IndexOf("-") - 1),
                        cprove = "",
                        cmaqui = cmaquina,
                        cturno = pck_turno.SelectedItem.ToString().Substring(0, 1),
                        cparti = "",
                        nordco = "",
                        npacki = 0,
                        nlotes = Int32.Parse(ety_lote.Text.ToString()),
                        nmuest = Int32.Parse(ety_muestra.Text.ToString()),
                        pcierr = 0,
                        nrecup = 0,
                        nsegun = 0,
                        porcen = 0,
                        flgcie = saudtot,
                        ndefec = qdefectos,
                        status = sapaud,
                        dobser = obsaudit,//ety_observ.Text = null ?? "",
                        flgrau = estadoReauditoria,
                        nreaud = 0,
                        caudit = VariableGlobal.ctraba,
                        flgext = "N",
                        cliref = xcliref,
                        fauref = xfauref,
                        nseref = xnseref,
                        cusuar = VariableGlobal.cusuar,
                        fcreac = DateTime.Now,
                        fmodif = DateTime.Parse("1900-01-01 00:00:00"),
                        nordpo = 0,
                        fprogr = DateTime.Parse("1900-01-01 00:00:00"),
                        caudpr = "",
                        drefpr = "",
                        ctpaud = "PR",
                        csuppl = "",
                        flgenv = "",
                        sanula = "N",
                        ndesap = 0,
                        sautab = "S",
                        senvio = "N",
                    };
                    App.baseDatos.Insert(nauditoria);

                    taudit00 auditoria = new taudit00
                    {
                        idaudi = xidaudi,
                        careas = "31",
                        faudit = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                        nsecue = Int32.Parse(lbl_nsecue.Text.ToString()),
                        clinea = "",
                        nordpr = ety_op.Text.ToString().Trim(),
                        ccarub = pck_combo.SelectedItem.ToString().Substring(0, pck_combo.SelectedItem.ToString().IndexOf("-") - 1).Trim(),
                        dcarub = pck_combo.SelectedItem.ToString().Substring(pck_combo.SelectedItem.ToString().IndexOf("-") + 1, pck_combo.SelectedItem.ToString().Length - (pck_combo.SelectedItem.ToString().IndexOf("-") + 1)).Trim(),
                        ctraba = "",// ety_ctraba.Text.ToString().Trim(),
                        copera = "",//pck_operacion.SelectedItem.ToString().Substring(0, pck_operacion.SelectedItem.ToString().IndexOf("-") - 1).Trim(),
                        dopera = "",//pck_operacion.SelectedItem.ToString().Substring(pck_operacion.SelectedItem.ToString().IndexOf("-") + 1, pck_operacion.SelectedItem.ToString().Length - (pck_operacion.SelectedItem.ToString().IndexOf("-") + 1)).Trim(),
                        dclien = lbl_descliente.Text.ToString().Trim(),
                        nlotes = Int32.Parse(ety_lote.Text.ToString()),
                        nmuest = Int32.Parse(ety_muestra.Text.ToString()),
                        status = sapaud,
                        dobser = obsaudit,//ety_observ.Text,
                        nordct = ety_corte.Text.ToString().Trim(),
                        npieza = 0,
                        dpieza = "",
                        clotei = "",
                        citems = "",
                        ditems = "",
                        cencog = "",
                        dtalla = "",
                        qprend = 0,
                        npanos = 0,
                        sreaud = "N",
                        ndefec = qdefectos,
                        nseref = 0,
                        cmaqui = cmaquina,
                        cturno = pck_turno.SelectedItem.ToString().Substring(0, 1)
                    };
                    App.baseDatos.Insert(auditoria);

                    if (estadoReauditoria == "S")
                    {
                        taudit00 rauditoria = new taudit00
                        {
                            idaudi = Int32.Parse(listaCargaAuditoria.ElementAt(0).idaudi.ToString()),
                            careas = listaCargaAuditoria.ElementAt(0).careas.ToString(),
                            faudit = DateTime.Parse(listaCargaAuditoria.ElementAt(0).faudit.ToString()),
                            nsecue = Int32.Parse(listaCargaAuditoria.ElementAt(0).nsecue.ToString()),
                            clinea = listaCargaAuditoria.ElementAt(0).clinea.ToString(),
                            nordpr = listaCargaAuditoria.ElementAt(0).nordpr.ToString(),
                            ccarub = listaCargaAuditoria.ElementAt(0).ccarub.ToString(),
                            dcarub = listaCargaAuditoria.ElementAt(0).dcarub.ToString(),
                            ctraba = listaCargaAuditoria.ElementAt(0).ctraba.ToString(),
                            copera = listaCargaAuditoria.ElementAt(0).copera.ToString(),
                            dopera = listaCargaAuditoria.ElementAt(0).dopera.ToString(),
                            dclien = listaCargaAuditoria.ElementAt(0).dclien.ToString(),
                            nlotes = Int32.Parse(listaCargaAuditoria.ElementAt(0).nlotes.ToString()),
                            nmuest = Int32.Parse(listaCargaAuditoria.ElementAt(0).nmuest.ToString()),
                            status = listaCargaAuditoria.ElementAt(0).status.ToString(),
                            dobser = listaCargaAuditoria.ElementAt(0).dobser.ToString(),
                            nordct = listaCargaAuditoria.ElementAt(0).nordct.ToString(),
                            npieza = Int32.Parse(listaCargaAuditoria.ElementAt(0).npieza.ToString()),
                            dpieza = listaCargaAuditoria.ElementAt(0).dpieza.ToString(),
                            clotei = listaCargaAuditoria.ElementAt(0).clotei.ToString(),
                            citems = listaCargaAuditoria.ElementAt(0).citems.ToString(),
                            ditems = listaCargaAuditoria.ElementAt(0).ditems.ToString(),
                            cencog = listaCargaAuditoria.ElementAt(0).cencog.ToString(),
                            dtalla = listaCargaAuditoria.ElementAt(0).dtalla.ToString(),
                            qprend = Int32.Parse(listaCargaAuditoria.ElementAt(0).qprend.ToString()),
                            npanos = Int32.Parse(listaCargaAuditoria.ElementAt(0).npanos.ToString()),
                            sreaud = "S",
                            ndefec = Int32.Parse(listaCargaAuditoria.ElementAt(0).ndefec.ToString()),
                            nseref = 0,
                            cmaqui = listaCargaAuditoria.ElementAt(0).cmaqui.ToString(),
                            cturno = listaCargaAuditoria.ElementAt(0).cturno.ToString()
                        };
                        App.baseDatos.Update(rauditoria);
                    }

                    var ldefec = App.baseDatos.GetList<pdefec10>(false).Where(x => x.svigen == "N").ToList();
                    foreach (var record in ldefec)
                    {
                        pdefec01 ndefecto = new pdefec01
                        {
                            careas = record.careas,
                            faudit = record.faudit,
                            nsecue = record.nsecue,
                            clinea = "",
                            codigo = record.codigo,
                            coddef = record.coddef,
                            qcanti = record.qcanti,
                            dobser = record.dobser,
                            cgrupo = record.cgrupo,
                            cardef = record.cardef,
                            senvio = "N",

                        };
                        App.baseDatos.Insert(ndefecto);

                        pdefec10 cdefecto = new pdefec10
                        {
                            iddefe = record.iddefe,
                            careas = record.careas,
                            faudit = record.faudit,
                            nsecue = record.nsecue,
                            clinea = "",
                            codigo = record.codigo,
                            coddef = record.coddef,
                            qcanti = record.qcanti,
                            dobser = record.dobser,
                            cgrupo = record.cgrupo,
                            cardef = record.cardef,
                            descri = record.descri,
                            defjpg = record.defjpg,
                            vimage = record.vimage,
                            vphoto = record.vphoto,
                            svigen = "S",
                        };
                        App.baseDatos.Update(cdefecto);
                    }
                    //}
                }
                else
                {
                    qdefectos = App.baseDatos.GetList<pdefec10>(false).Where(x => x.careas == "31" && x.nsecue == Int32.Parse(lbl_nsecue.Text.ToString()) && x.faudit.ToShortDateString() == dpk_fechaauditoria.Date.ToShortDateString()).Sum(x => x.qcanti);
                    paudit01 nauditoria = new paudit01
                    {
                        idaudi = listaCargaAuditoria.ElementAt(0).idaudi,
                        careas = "31",
                        faudit = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                        nsecue = Int32.Parse(lbl_nsecue.Text.ToString()),
                        clinea = "",
                        ctpord = "OP",
                        nordpr = ety_op.Text.ToString(),
                        nordct = ety_corte.Text.ToString().Trim(),
                        cmarbe = "",
                        ccarub = pck_combo.SelectedItem.ToString().Substring(0, pck_combo.SelectedItem.ToString().IndexOf("-") - 1),
                        npieza = 0,
                        cencog = "",
                        citems = "",
                        ccolor = "",
                        npanos = 0,
                        dtalla = "",
                        qtotal = 0,
                        dlotes = "",
                        ctraba = "",//ety_ctraba.Text.ToString(),
                        copera = "",//pck_operacion.SelectedItem.ToString().Substring(0, pck_operacion.SelectedItem.ToString().IndexOf("-") - 1),
                        cprove = "",
                        cmaqui = cmaquina,
                        cturno = pck_turno.SelectedItem.ToString().Substring(0, 1),
                        cparti = "",
                        nordco = "",
                        npacki = 0,
                        nlotes = Int32.Parse(ety_lote.Text.ToString()),
                        nmuest = Int32.Parse(ety_muestra.Text.ToString()),
                        pcierr = 0,
                        nrecup = 0,
                        nsegun = 0,
                        porcen = 0,
                        flgcie = saudtot,
                        ndefec = 0,
                        status = sapaud,
                        dobser = obsaudit,//ety_observ.Text.ToString(),
                        flgrau = "N",
                        nreaud = 0,
                        caudit = "",
                        flgext = "N",
                        cliref = xcliref,
                        fauref = xfauref,
                        nseref = xnseref,
                        cusuar = VariableGlobal.cusuar,
                        fcreac = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                        fmodif = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                        nordpo = 0,
                        fprogr = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                        caudpr = "",
                        drefpr = "",
                        ctpaud = "PR",
                        csuppl = "",
                        flgenv = "",
                        sanula = "N",
                        ndesap = 0,
                        sautab = "S",
                        senvio = "N",
                    };
                    App.baseDatos.Update(nauditoria);

                    taudit00 auditoria = new taudit00
                    {
                        idaudi = listaCargaAuditoria.ElementAt(0).idaudi,
                        careas = "31",
                        faudit = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                        nsecue = Int32.Parse(lbl_nsecue.Text.ToString()),
                        clinea = "",
                        nordpr = ety_op.Text.ToString().Trim(),
                        ccarub = pck_combo.SelectedItem.ToString().Substring(0, pck_combo.SelectedItem.ToString().IndexOf("-") - 1).Trim(),
                        dcarub = pck_combo.SelectedItem.ToString().Substring(pck_combo.SelectedItem.ToString().IndexOf("-") + 1, pck_combo.SelectedItem.ToString().Length - (pck_combo.SelectedItem.ToString().IndexOf("-") + 1)).Trim(),
                        ctraba = "",//ety_ctraba.Text.ToString().Trim(),
                        copera = "",//pck_operacion.SelectedItem.ToString().Substring(0, pck_operacion.SelectedItem.ToString().IndexOf("-") - 1).Trim(),
                        dopera = "",//pck_operacion.SelectedItem.ToString().Substring(pck_operacion.SelectedItem.ToString().IndexOf("-") + 1, pck_operacion.SelectedItem.ToString().Length - (pck_operacion.SelectedItem.ToString().IndexOf("-") + 1)).Trim(),
                        dclien = lbl_descliente.Text.ToString().Trim(),
                        nlotes = Int32.Parse(ety_lote.Text.ToString()),
                        nmuest = Int32.Parse(ety_muestra.Text.ToString()),
                        status = sapaud,
                        dobser = obsaudit,//ety_observ.Text,
                        nordct = ety_corte.Text.ToString().Trim(),
                        npieza = 0,
                        dpieza = "",
                        clotei = "",
                        citems = "",
                        ditems = "",
                        cencog = "",
                        dtalla = "",
                        qprend = 0,
                        npanos = 0,
                        sreaud = listaCargaAuditoria.ElementAt(0).sreaud,
                        ndefec = qdefectos,
                        cmaqui = cmaquina,
                        cturno = pck_turno.SelectedItem.ToString().Substring(0, 1)
                    };
                    App.baseDatos.Update(auditoria);
                    //}
                }

                DisplayAlert("Aviso", "Los datos se guardaron de manera satisfactoria.", "OK");
                ActualizaResumenAuditoria();
                LimpiaAuditoria();
                sgraud = "S";
            }
            catch (Exception ex)
            {
                DisplayAlert("Aviso", ex.StackTrace, "OK");
            }
        }
        #endregion

        private void btn_agregardefecto_Clicked(object sender, EventArgs e)
        {
            AddDefecto();
        }

        private async void img_capturarfoto_Tapped(object sender, TappedEventArgs e)
        {
            var seldefecto = (e.Parameter) as pdefec10;

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Test",
                SaveToAlbum = true,
                CompressionQuality = 75,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Rear,
                Name = "31" + dpk_fechaauditoria.Date.Year.ToString() + dpk_fechaauditoria.Date.Month.ToString() + dpk_fechaauditoria.Date.Day.ToString() + lbl_bloquedef.Text + lbl_nsecue.Text + seldefecto.coddef.ToString() + ".jpg",
            });

            if (file == null)
                return;
            string desfot = file.Path.Substring(82, file.Path.Length - 82);

            try
            {
                pdefec10 cdefecto = new pdefec10
                {
                    iddefe = seldefecto.iddefe,
                    careas = "31",
                    faudit = dpk_fechaauditoria.Date,
                    nsecue = Int32.Parse(lbl_nsecue.Text),
                    clinea = "",
                    codigo = seldefecto.codigo,
                    coddef = seldefecto.coddef,//pck_defectos.SelectedItem.ToString().Substring(0, 2),
                    qcanti = seldefecto.qcanti,
                    dobser = seldefecto.dobser,
                    cgrupo = seldefecto.cgrupo,
                    cardef = seldefecto.cardef,
                    descri = seldefecto.descri,//pck_defectos.SelectedItem.ToString().Substring(5, (pck_defectos.SelectedItem.ToString().Length - 5)),
                    defjpg = desfot.ToString(),
                    vimage = true,
                    vphoto = false,
                    svigen = seldefecto.svigen,
                };
                App.baseDatos.Update(cdefecto);

                listaCargaDefectos = App.baseDatos.GetList<pdefec10>(false);
                lsv_defectos.ItemsSource = listaCargaDefectos.Where(x => x.nsecue == Int32.Parse(lbl_nsecue.Text.ToString()) && x.careas == "31");
                //}
                //await DisplayAlert("Aviso", "La imagen se guardo de manera satisfactoria.", "OK");
                //using (var data = new DataAccess())
                //{
                listaCargaDefectos = App.baseDatos.GetList<pdefec10>(false);
                lsv_defectos.ItemsSource = listaCargaDefectos.Where(x => x.nsecue == Int32.Parse(lbl_nsecue.Text.ToString()) && x.careas == "31");
                //}

                var result = await alertService.ShowMessage("Aviso", "Desea editar la imagen.");
                if (result == true)
                {
                    await App.Navigator.PushAsync(new ImageEditorPage(desfot));
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", ex.StackTrace, "OK");
            }
        }

        private void lsv_defectos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            int lvindex = listaCargaDefectos.IndexOf(e.SelectedItem as pdefec10);
        }

        private async void img_deletejpg_Tapped(object sender, TappedEventArgs e)
        {
            var result = await alertService.ShowMessage("Aviso", "Desea eliminar la imagen.");
            if (result == true)
            {
                var seldefecto = (e.Parameter) as pdefec10;
                string filename = seldefecto.defjpg;
                try
                {
                    //using (var data = new DataAccess())
                    //{
                    pdefec10 cdefecto = new pdefec10
                    {
                        iddefe = seldefecto.iddefe,
                        careas = "31",
                        faudit = dpk_fechaauditoria.Date,
                        nsecue = seldefecto.nsecue,
                        clinea = "",
                        codigo = seldefecto.codigo,
                        coddef = seldefecto.coddef,//pck_defectos.SelectedItem.ToString().Substring(0, 2),
                        qcanti = seldefecto.qcanti,
                        dobser = seldefecto.dobser,
                        cgrupo = seldefecto.cgrupo,
                        cardef = seldefecto.cardef,
                        descri = seldefecto.descri,//pck_defectos.SelectedItem.ToString().Substring(5, (pck_defectos.SelectedItem.ToString().Length - 5)),
                        defjpg = "",
                        vimage = false,
                        vphoto = true,
                        svigen = seldefecto.svigen,
                    };
                    App.baseDatos.Update(cdefecto);

                    //listaCargaDefectos = data.GetList<pdefec10>(false);
                    listaCargaDefectos = App.baseDatos.GetList<pdefec10>(false).Where(x => x.careas == seldefecto.careas && x.clinea == seldefecto.clinea && x.nsecue == seldefecto.nsecue && x.faudit == seldefecto.faudit).ToList();
                    lsv_defectos.ItemsSource = listaCargaDefectos;
                    //}

                    IFolder rootFolder = await FileSystem.Current.GetFolderFromPathAsync("/storage/emulated/0/Pictures/Test/");
                    ExistenceCheckResult folderexist = await rootFolder.CheckExistsAsync(filename);
                    if (folderexist == ExistenceCheckResult.FileExists)
                    {
                        DependencyService.Get<IFileManager>().DeleteFile("/storage/emulated/0/Pictures/test/" + filename);
                    }

                    await DisplayAlert("Aviso", "La imagen se elimino de manera satisfactoria.", "OK");

                    //using (var data = new DataAccess())
                    //{
                    //    listaCargaDefectos = data.GetList<pdefec10>(false);
                    //    lsv_defectos.ItemsSource = listaCargaDefectos;
                    //}
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Aviso", ex.StackTrace, "OK");
                }
            }
            else
            {
                return;
            }
        }



        private async void img_viewjpg_Tapped(object sender, TappedEventArgs e)
        {
            var seldefecto = (e.Parameter) as pdefec10;

            string filename = seldefecto.defjpg;
            IFolder rootFolder = await FileSystem.Current.GetFolderFromPathAsync("/storage/emulated/0/Pictures/Test/");
            ExistenceCheckResult folderexist = await rootFolder.CheckExistsAsync(filename);
            if (folderexist == ExistenceCheckResult.FileExists)
            {
                DependencyService.Get<IFileManager>().OpenFile("/storage/emulated/0/Pictures/test/" + filename);
            }
        }

        private void btn_guardarauditoria_Clicked(object sender, EventArgs e)
        {
            AddAuditoria();
        }

        private void fab_nuevaauditoria_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Aviso", "Seleccionado", "OK");
        }
        private void ety_lote_Unfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(ety_lote.Text))
            {
                //using (var data = new DataAccess())
                //{
                var xlotes = App.baseDatos.GetList<ttcmue00>(false).Where(x => Int32.Parse(ety_lote.Text) >= x.ntanli && Int32.Parse(ety_lote.Text) <= x.ntanlf && x.nivaql.Trim() == "2.5");
                foreach (var record in xlotes)
                {
                    ety_muestra.Text = record.ntanmu.ToString();
                }
                //}

                if (cbxaudittot.IsChecked == true)
                {
                    ety_muestra.Text = ety_lote.Text;
                }
            }
        }

        private void ety_ctraba_Unfocused(object sender, FocusEventArgs e)
        {
            //BuscarOperario();
        }

        private void ety_op_Unfocused(object sender, FocusEventArgs e)
        {
            //BuscarOP();
        }

        private void Cbxaudittot_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(ety_lote.Text))
            {
                DisplayAlert("Error", "Ingrese la cantidad del lote", "OK");
                ety_lote.Focus();
                return;
            }

            //using (var data = new DataAccess())
            //{
            var xlotes = App.baseDatos.GetList<ttcmue00>(false).Where(x => Int32.Parse(ety_lote.Text) >= x.ntanli && Int32.Parse(ety_lote.Text) <= x.ntanlf && x.nivaql.Trim() == "2.5");
            foreach (var record in xlotes)
            {
                ety_muestra.Text = record.ntanmu.ToString();
            }
            //}

            if (cbxaudittot.IsChecked == true)
            {
                ety_muestra.Text = ety_lote.Text;
                saudtot = "S";
            }
            else
            {
                saudtot = "N";
            }
        }

        //private void Pck_combo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (pck_combo.SelectedItem != null)
        //    {
        //        CargarPiezas();
        //    }
        //}

        //private async void CargarPiezas()
        //{
        //    var response = await apiService.GetPiezas<ppzxes00>(ety_op.Text, pck_combo.SelectedItem.ToString().Substring(0, pck_combo.SelectedItem.ToString().IndexOf("-") - 1));
        //    if (!response.IsSuccess)
        //    {
        //        await DisplayAlert("Error", response.Message, "OK");
        //        return;
        //    }

        //    //*** Llena Picker Piezas ***//
        //    pck_pieza.Items.Clear();
        //    var datapieza = (List<ppzxes00>)response.Result;
        //    foreach (var record in datapieza.OrderBy(x => x.npieza))
        //    {
        //        pck_pieza.Items.Add(record.npieza.Trim() + " - " + record.dpieza.Trim());
        //    }

        //    if (listaCargaAuditoria.Count != 0)
        //    {
        //        pck_pieza.SelectedItem = listaCargaAuditoria.ElementAt(0).npieza.ToString().Trim() + " - " + listaCargaAuditoria.ElementAt(0).dpieza.ToString().Trim();
        //    }

        //}

        //private async void CargaCorte()
        //{
        //    var response = await apiService.GetCorte<pcorte00>(ety_op.Text, pck_corte.SelectedItem.ToString(), pck_pieza.SelectedItem.ToString().Substring(0, 1));
        //    if (!response.IsSuccess)
        //    {
        //        await DisplayAlert("Error", "No existe datos para la pieza " + pck_pieza.SelectedItem.ToString().Substring(0, 1), "OK");
        //        lbl_cparti.Text = "";
        //        lbl_tela.Text = "";
        //        lbl_cencog.Text = "";
        //        lbl_tallas.Text = "";
        //        lbl_qprend.Text = "";
        //        lbl_qpanos.Text = "";
        //        pck_bloque.SelectedItem = null;
        //        return;
        //    }

        //    var datacorte = (List<pcorte00>)response.Result;
        //    foreach (var record in datacorte.OrderBy(x => x.npieza))
        //    {
        //        lbl_cparti.Text = record.clotei;
        //        lbl_tela.Text = record.ditems;
        //        lbl_cencog.Text = record.cencog;
        //        lbl_tallas.Text = record.dtalla;
        //        lbl_qprend.Text = record.qprend.ToString();
        //        lbl_qpanos.Text = record.npanos.ToString();
        //        pck_bloque.SelectedItem = record.nmodul;
        //        lbl_bloquedef.Text = pck_bloque.SelectedItem.ToString();
        //        lbl_fechadef.Text = dpk_fechaauditoria.Date.ToString("dd - MMM - yyyy");
        //        xcitems = record.citems;
        //    }
        //}

        //private void Pck_pieza_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (pck_pieza.SelectedIndex != -1)
        //    {
        //        CargaCorte();
        //    }
        //}

        //private void Pck_corte_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (pck_corte.SelectedIndex != -1)
        //    {
        //        CargaColorCorte();
        //    }
        //}

        private void Dpk_fechaauditoria_DateSelected(object sender, DateChangedEventArgs e)
        {
            var datosSecuencia = App.baseDatos.GetList<paudit01>(false).Where(a => a.careas == "31" && a.faudit.ToShortDateString() == dpk_fechaauditoria.Date.ToShortDateString()).ToList();
            if (datosSecuencia.Count > 0)
            {
                var ultimaAuditoria = App.baseDatos.GetList<paudit01>(false).Where(a => a.careas == "31" && a.faudit.ToShortDateString() == dpk_fechaauditoria.Date.ToShortDateString()).OrderByDescending(x => x.nsecue).First();
                lbl_nsecue.Text = (ultimaAuditoria.nsecue + 1).ToString();
            }
            else
            { lbl_nsecue.Text = "1"; }
        }

        //private async void CargaColorCorte()
        //{
        //    var response = await apiService.GetCorteColor<padcor00>(ety_op.Text, pck_corte.SelectedItem.ToString());
        //    if (!response.IsSuccess)
        //    {
        //        await DisplayAlert("Error", response.Message, "OK");
        //        return;
        //    }

        //    var datacorte = (List<padcor00>)response.Result;
        //    foreach (var record in datacorte)
        //    {
        //        pck_combo.SelectedItem = record.ccarub.Trim() + " - " + record.dcarub.Trim();
        //    }
        //}

        private void Tlb_viewpdf_Clicked(object sender, EventArgs e)
        {
            App.Navigator.PushAsync(new ProductionOrderPage());
        }

        private void Pck_defectos_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var seledefecto = (e.Value) as mdefec00;
            xcoddef = seledefecto.coddef.ToString().Trim();
            xdedef = seledefecto.ddefec.ToString().Trim();
        }


    }
}