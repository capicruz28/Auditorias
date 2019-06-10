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
    public partial class AuditoriaCortePage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Attributes        
        private DialogService dialogService;
        private AlertService alertService;
        private ApiService apiService;
        private DataService dataService;
        private DataAccess dataAccess;
        private MediaFile _mediaFile;
        List<pdefec10> xoperac;
        string xreaud = "N";
        string nregis;
        string _bloque;
        string sgraud;
        Int32 xidaudi;
        string obsdefecto;
        string obsaudit;
        string saudtot;
        string xcitems;
        #endregion

        public List<string> SectorList { get; set; }
        public List<taudit00> SelBloque { get; set; }


        //public string SelBloque
        //{
        //    get
        //    {
        //        return _bloque;
        //    }
        //    set
        //    {
        //        if (_bloque != value)
        //        {
        //            _bloque = value;
        //            PropertyChanged?.Invoke(this,
        //                                    new PropertyChangedEventArgs(nameof(SelBloque)));
        //        }
        //    }
        //}

        public AuditoriaCortePage(List<taudit00> bloque)
        {
            dialogService = new DialogService();
            alertService = new AlertService();
            apiService = new ApiService();
            dataService = new DataService();
            dataAccess = new DataAccess();
            List<pdefec10> xopera = new List<pdefec10>();
            SectorList = new List<string>
            {
                "01","02","03","04","05","06",
                "07","08","09","10","11","12","SE"
            };

            this.SelBloque = bloque;
            InitializeComponent();

            nregis = "S";
            sgraud = "N";
            pck_bloque.ItemsSource = SectorList;
            if (SelBloque.Count != 0)
            {
                CargaAuditoria();
            }

            using (var data = new DataAccess())
            {
                var ldefec = data.GetList<pdefec10>(false).Where(x => x.svigen == "N").ToList();
                foreach (var record in ldefec)
                {
                    pdefec10 cdefecto = new pdefec10
                    {
                        iddefe = record.iddefe,
                        careas = record.careas,
                        faudit = record.faudit,
                        nsecue = record.nsecue,
                        clinea = record.clinea,
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
                        svigen = record.svigen,
                    };
                    data.Delete(cdefecto);
                }

                var user = data.GetUsuario();
                VariableGlobal.ctraba = user.ctraba;
                VariableGlobal.cusuar = user.cusuar;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            // Begin an asyncronous task on the UI thread because we intend to ask the users permission.
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (pck_bloque.SelectedItem != null)
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


        void CargaAuditoria()
        {
            if (SelBloque.ElementAt(0).status.ToString() == "D" && SelBloque.ElementAt(0).smodif.ToString() == "R")
            {
                if (Int32.Parse(SelBloque.ElementAt(0).nlotes.ToString()) == Int32.Parse(SelBloque.ElementAt(0).nmuest.ToString()))
                {
                    cbxaudittot.IsChecked = true;
                }
                else
                {
                    cbxaudittot.IsChecked = false;
                }
                nregis = "S";
                xreaud = "S";
                pck_bloque.SelectedItem = SelBloque.ElementAt(0).clinea.ToString();
                dpk_fechaauditoria.Date = DateTime.Parse(SelBloque.ElementAt(0).faudit.ToString());
                //ety_ctraba.Text = SelBloque.ElementAt(0).ctraba.ToString();
                //BuscarOperario();
                ety_op.Text = SelBloque.ElementAt(0).nordpr.ToString();
                BuscarOP();
                pck_corte.SelectedItem = SelBloque.ElementAt(0).nordct.ToString();
                ety_lote.Text = SelBloque.ElementAt(0).nlotes.ToString();
                ety_muestra.Text = SelBloque.ElementAt(0).nmuest.ToString();
                ety_observ.Text = SelBloque.ElementAt(0).dobser.ToString();
                srb_audidesaprobado.IsChecked = true;
                //img_operario.IsEnabled = true;
                //ety_ctraba.IsEnabled = true;
                //btn_ctraba.IsEnabled = true;
                ety_op.IsEnabled = true;
                btn_buscarop.IsEnabled = true;
                pck_combo.IsEnabled = true;
                //img_operacion.IsEnabled = true;
                //pck_operacion.IsEnabled = true;
                img_defecto.IsEnabled = true;
                pck_defectos.IsEnabled = true;
                ety_cantdefecto.IsEnabled = true;
                ety_obsdefecto.IsEnabled = true;
                btn_agregardefecto.IsEnabled = true;
                ety_lote.IsEnabled = true;
                ety_muestra.IsEnabled = true;
                using (var data = new DataAccess())
                {
                    var datos = data.GetList<paudit01>(false).Where(a => a.clinea == pck_bloque.SelectedItem.ToString() && a.careas == "16").ToList();
                    if (datos.Count > 0)
                    {
                        var ultsec = data.GetList<paudit01>(false).Where(a => a.clinea == pck_bloque.SelectedItem.ToString() && a.careas == "16").OrderByDescending(x => x.nsecue).First();
                        lbl_nsecue.Text = (ultsec.nsecue + 1).ToString();
                    }
                    else
                    { lbl_nsecue.Text = "1"; }
                }
                using (var data = new DataAccess())
                {
                    var uidreg = data.GetList<paudit01>(false);
                    if (uidreg.Count > 0)
                    {
                        var ultidu = data.GetList<paudit01>(false).OrderByDescending(x => x.idaudi).First();
                        xidaudi = ultidu.idaudi + 1;
                    }
                    else
                    { xidaudi = 1; }
                }
            }
            else
            {
                if (Int32.Parse(SelBloque.ElementAt(0).nlotes.ToString()) == Int32.Parse(SelBloque.ElementAt(0).nmuest.ToString()))
                {
                    cbxaudittot.IsChecked = true;
                }
                else
                {
                    cbxaudittot.IsChecked = false;
                }
                nregis = "N";
                xreaud = "N";
                ety_op.Text = SelBloque.ElementAt(0).nordpr.ToString();
                BuscarOP();
                pck_bloque.SelectedItem = SelBloque.ElementAt(0).clinea.ToString();
                dpk_fechaauditoria.Date = DateTime.Parse(SelBloque.ElementAt(0).faudit.ToString());
                lbl_nsecue.Text = SelBloque.ElementAt(0).nsecue.ToString();
                //pck_corte.SelectedItem = SelBloque.ElementAt(0).nordct.ToString().Trim();
                //pck_pieza.SelectedItem = SelBloque.ElementAt(0).npieza.ToString().Trim()+" - "+ SelBloque.ElementAt(0).dpieza.ToString().Trim();
                lbl_cparti.Text = SelBloque.ElementAt(0).clotei.ToString();
                lbl_tela.Text = SelBloque.ElementAt(0).ditems.ToString();
                lbl_tallas.Text = SelBloque.ElementAt(0).dtalla.ToString();
                lbl_cencog.Text = SelBloque.ElementAt(0).cencog.ToString();
                lbl_qprend.Text = SelBloque.ElementAt(0).qprend.ToString();
                lbl_qpanos.Text = SelBloque.ElementAt(0).npanos.ToString();
                //ety_ctraba.Text = SelBloque.ElementAt(0).ctraba.ToString();
                //BuscarOperario();                  
                ety_lote.Text = SelBloque.ElementAt(0).nlotes.ToString();
                ety_muestra.Text = SelBloque.ElementAt(0).nmuest.ToString();
                ety_observ.Text = SelBloque.ElementAt(0).dobser.ToString();
                if (SelBloque.ElementAt(0).status.ToString() == "A") srb_audiaprobado.IsChecked = true;
                if (SelBloque.ElementAt(0).status.ToString() == "D") srb_audidesaprobado.IsChecked = true;
                if (SelBloque.ElementAt(0).status.ToString() == "E") srb_audiaprobadoext.IsChecked = true;
                using (var data = new DataAccess())
                {
                    xoperac = data.GetList<pdefec10>(false).Where(x => x.clinea == SelBloque.ElementAt(0).clinea.ToString()).ToList();
                    lsv_defectos.ItemsSource = xoperac;
                }
                //img_operario.IsEnabled = true;
                //ety_ctraba.IsEnabled = true;
                //btn_ctraba.IsEnabled = true;
                ety_op.IsEnabled = true;
                btn_buscarop.IsEnabled = true;
                pck_corte.IsEnabled = true;
                pck_combo.IsEnabled = true;
                pck_pieza.IsEnabled = true;
                //img_operacion.IsEnabled = true;
                //pck_operacion.IsEnabled = true;
                img_defecto.IsEnabled = true;
                pck_defectos.IsEnabled = true;
                ety_cantdefecto.IsEnabled = true;
                ety_obsdefecto.IsEnabled = true;
                btn_agregardefecto.IsEnabled = true;
                ety_lote.IsEnabled = true;
                ety_muestra.IsEnabled = true;
            }
        }

        public void AgregarNuevaAuditoria()
        {
            //if (pck_bloque.SelectedItem == null)
            //{
            //    DisplayAlert("Aviso", "Debe seleccionar un bloque", "OK");
            //    return;
            //}
            using (var data = new DataAccess())
            {
                var datos = data.GetList<paudit01>(false).Where(a =>  a.careas == "16").ToList();
                if (datos.Count > 0)
                {
                    var ultsec = data.GetList<paudit01>(false).Where(a => a.careas == "16").OrderByDescending(x => x.nsecue).First();
                    lbl_nsecue.Text = (ultsec.nsecue + 1).ToString();
                }
                else
                { lbl_nsecue.Text = "1"; }
            }
            using (var data = new DataAccess())
            {
                var uidreg = data.GetList<paudit01>(false);
                if (uidreg.Count > 0)
                {
                    var ultidu = data.GetList<paudit01>(false).OrderByDescending(x => x.idaudi).First();
                    xidaudi = ultidu.idaudi + 1;
                }
                else
                { xidaudi = 1; }
            }
            //img_operario.IsEnabled = true;
            //ety_ctraba.IsEnabled = true;
            //btn_ctraba.IsEnabled = true;
            ety_op.IsEnabled = true;
            btn_buscarop.IsEnabled = true;
            pck_corte.IsEnabled = true;
            pck_combo.IsEnabled = true;
            pck_pieza.IsEnabled = true;
            //img_operacion.IsEnabled = true;
            //pck_operacion.IsEnabled = true;
            img_defecto.IsEnabled = true;
            pck_defectos.IsEnabled = true;
            ety_cantdefecto.IsEnabled = true;
            ety_obsdefecto.IsEnabled = true;
            btn_agregardefecto.IsEnabled = true;
            ety_lote.IsEnabled = true;
            ety_muestra.IsEnabled = true;
            ety_observ.IsEnabled = true;
        }
        private void btn_agregarauditoria_Clicked(object sender, EventArgs e)
        {
            AgregarNuevaAuditoria();
        }

        void BuscarOperario()
        {
            //if (string.IsNullOrEmpty(ety_ctraba.Text))
            //{
            //    DisplayAlert("Error", "Ingrese un código de trabajador.", "OK");
            //    ety_ctraba.Focus();
            //    return;
            //}

            //if (ety_ctraba.Text.Length == 3)
            //{
            //    ety_ctraba.Text = "O00" + ety_ctraba.Text;
            //}

            //if (ety_ctraba.Text.Length == 4)
            //{
            //    ety_ctraba.Text = "O0" + ety_ctraba.Text;
            //}

            //if (ety_ctraba.Text.Length == 5)
            //{
            //    ety_ctraba.Text = "O" + ety_ctraba.Text;
            //}

            //using (var datos = new DataAccess())
            //{
            //    var xctraba = datos.GetOperario(ety_ctraba.Text);
            //    if (xctraba == null)
            //    {
            //        DisplayAlert("Error", "El código de trabajador no existe o no esta activo", "OK");
            //        return;
            //    }
            //    lbl_dtraba.Text = xctraba.dtraba;
            //}
        }
        private void btn_ctraba_Clicked(object sender, EventArgs e)
        {
            BuscarOperario();
        }

        async void BuscarOP()
        {
            if (string.IsNullOrEmpty(ety_op.Text))
            {
                await DisplayAlert("Error", "Ingrese un número de OP", "OK");
                ety_op.Focus();
                return;
            }
            var response = await apiService.GetOP<ordprod>(ety_op.Text);
            if (!response.IsSuccess)
            {
                await DisplayAlert("Error", response.Message, "OK");
                ety_op.Focus();
                return;
            }

            //*** Llena Picker Corte ***//
            pck_corte.Items.Clear();
            var dataop = (List<ordprod>)response.Result;
            foreach (var record in dataop.OrderBy(x => x.nordct))
            {
                lbl_descliente.Text = record.dclien;
                pck_corte.Items.Add(record.nordct.Trim());
            }
            //pck_corte.ItemsSource = dataop;

            //*** Llena Picker Combo ***//
            pck_combo.Items.Clear();
            var datos = (from dat in dataop
                         group dat by new { dat.ccarub, dat.dcarub } into g
                         select new { ccarub = g.Key.ccarub, dcarub = g.Key.dcarub });
            foreach (var record in datos)
            {
                pck_combo.Items.Add(record.ccarub.Trim() + " - " + record.dcarub.Trim());
            }

            if (SelBloque.Count != 0)
            {
                pck_corte.SelectedItem = SelBloque.ElementAt(0).nordct.ToString().Trim();
                pck_combo.SelectedItem = SelBloque.ElementAt(0).ccarub.ToString().Trim() + " - " + SelBloque.ElementAt(0).dcarub.ToString().Trim();                
                //pck_pieza.SelectedItem = SelBloque.ElementAt(0).npieza.ToString().Trim() + " - " + SelBloque.ElementAt(0).dpieza.ToString().Trim();
            }

            //*** Llena Picker Operación ***//
            //pck_operacion.Items.Clear();
            //using (var data = new DataAccess())
            //{
            //    var xopera = data.GetList<topera01>(false);
            //    //pck_operacion.DataSource = xoperac;

            //    if (xopera == null)
            //    {
            //        await dialogService.ShowMessage("Error", "No existen registros");
            //        return;
            //    }
            //    foreach (var record in xopera)
            //    {
            //        pck_operacion.Items.Add(record.cclave.Trim() + " - " + record.descri.Trim());
            //        //pck_operacion.Items.Add(record.dopera);
            //        //pck_operacion.DisplayMemberPath = "dopera";
            //    }
            //}

            //if (SelBloque.Count != 0)
            //{
            //    pck_operacion.SelectedItem = SelBloque.ElementAt(0).copera.ToString().Trim() + " - " + SelBloque.ElementAt(0).dopera.ToString().Trim();
            //    //pck_operacion.SelectedItem = SelBloque.ElementAt(0).dopera.ToString();
            //}

            //*** Llena Picker Defectos ***//
            //pck_defectos.Items.Clear();
            using (var data = new DataAccess())
            {
                //var xdefec = data.GetList<mdefec00>(false).Where(x => x.csecci != "20" && x.csecci != "FI" && x.csecci != "32").OrderBy(a=>a.coddef);
                var xdefec = data.GetList<mdefec00>(false).Where(x => x.csecci == "16").OrderBy(a => a.coddef);
                //pck_defectos.DataSource = xdefec;

                if (xdefec == null)
                {
                    await dialogService.ShowMessage("Error", "No existen registros");
                    return;
                }
                foreach (var record in xdefec)
                {
                    pck_defectos.Items.Add(record.coddef.Trim() + " - " + record.descri.Trim());
                    //pck_defectos.DisplayMemberPath = "ddefec";
                    //pck_defectos.SelectedValuePath = "descri";
                }
            }          
        }
        private void btn_buscarop_Clicked(object sender, EventArgs e)
        {
            BuscarOP();
        }

        private async void img_operarios_Tapped(object sender, EventArgs e)
        {
            var result = await alertService.ShowMessage("Aviso", "Desea sincronizar la lista de trabajadores");
            if (result == true)
            {
                dataAccess.DeleteOperarios();
                var response = await apiService.Operarios<mtraba00>();

                if (!response.IsSuccess)
                {
                    await dialogService.ShowMessage("Error", response.Message);
                    return;
                }

                var opera = (List<mtraba00>)response.Result;
                int xqtraba = opera.Count();
                int xtraba = 0;

                using (IProgressDialog fooDialog = UserDialogs.Instance.Progress("Sincronizando...", null, null, true, MaskType.Black))
                {
                    foreach (var record in opera)
                    {
                        mtraba00 operario = new mtraba00
                        {
                            ctraba = record.ctraba,
                            dtraba = record.dtraba,
                            ccargo = record.ccargo,
                            dcargo = record.dcargo,
                            xsecci = record.xsecci,
                            clinea = record.clinea
                        };
                        dataService.InsertOperario(operario);
                        xtraba = xtraba + 1;
                        fooDialog.PercentComplete = xtraba;
                        fooDialog.Title = xtraba + " de " + xqtraba;
                        await Task.Delay(10);
                    }
                }
            }
            else // if it's equal to Cancel
            {
                return; // just return to the page and do nothing.
            }
        }

        private async void img_operaciones_Tapped(object sender, EventArgs e)
        {
            var result = await alertService.ShowMessage("Aviso", "Desea sincronizar la lista de operaciones");
            if (result == true)
            {
                dataAccess.DeleteOperaciones();
                var response = await apiService.Operaciones<topera01>();

                if (!response.IsSuccess)
                {
                    await dialogService.ShowMessage("Error", response.Message);
                    return;
                }

                var response01 = await apiService.Aql<ttcmue00>();
                if (!response01.IsSuccess)
                {
                    await dialogService.ShowMessage("Error", response01.Message);
                    return;
                }
                var opera01 = (List<ttcmue00>)response01.Result;

                var opera = (List<topera01>)response.Result;
                int xqtraba = opera.Count();
                int xtraba = 0;

                using (IProgressDialog fooDialog = UserDialogs.Instance.Progress("Sincronizando...", null, null, true, MaskType.Black))
                {
                    foreach (var record in opera)
                    {
                        topera01 operario = new topera01
                        {
                            cclave = record.cclave,
                            descri = record.descri,
                            cgrupo = record.cgrupo,
                            dopera = record.dopera,
                        };
                        dataService.InsertOperacion(operario);
                        xtraba = xtraba + 1;
                        fooDialog.PercentComplete = xtraba;
                        fooDialog.Title = xtraba + " de " + xqtraba;
                        await Task.Delay(10);
                    }
                    foreach (var record in opera01)
                    {
                        ttcmue00 muestra = new ttcmue00
                        {
                            codmue = record.codmue,
                            ntanli = record.ntanli,
                            ntanlf = record.ntanlf,
                            ntanmu = record.ntanmu,
                            nivaql = record.nivaql,
                        };
                        dataService.InsertAql(muestra);
                    }
                }
            }
            else // if it's equal to Cancel
            {
                return; // just return to the page and do nothing.
            }
        }

        private async void img_defectos_Tapped(object sender, EventArgs e)
        {
            var result = await alertService.ShowMessage("Aviso", "Desea sincronizar la lista de defectos");
            if (result == true)
            {
                dataAccess.DeleteDefectos();
                var response = await apiService.Defectos<mdefec00>();

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

            if (string.IsNullOrEmpty(ety_cantdefecto.Text))
            {
                DisplayAlert("Error", "Ingrese un número de OP", "OK");
                ety_cantdefecto.Focus();
                return;
            }

            try
            {
                using (var data = new DataAccess())
                {
                    #region pdefec01
                    //pdefec01 ndefecto = new pdefec01
                    //{
                    //    careas = "19",
                    //    faudit = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                    //    nsecue = Int32.Parse(lbl_nsecue.Text.ToString()),
                    //    clinea = lbl_bloquedef.Text,
                    //    codigo = "02",
                    //    coddef = pck_defectos.SelectedItem.ToString().Substring(0, 2),
                    //    qcanti = Int32.Parse(ety_cantdefecto.Text.ToString()),
                    //    dobser = ety_obsdefecto.Text.ToString(),
                    //    cgrupo = "N",
                    //    cardef = "19",

                    //};
                    //data.Insert(ndefecto); 
                    #endregion

                    int qdefec = data.GetList<pdefec10>(false).Where(x => x.careas == "16" && x.clinea == pck_bloque.SelectedItem.ToString() && x.nsecue == Int32.Parse(lbl_nsecue.Text.ToString()) && x.coddef == pck_defectos.SelectedItem.ToString().Substring(0, 2) && x.faudit.ToShortDateString() == dpk_fechaauditoria.Date.ToShortDateString()).Count();
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
                        careas = "16",
                        faudit = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                        nsecue = Int32.Parse(lbl_nsecue.Text.ToString()),
                        clinea = lbl_bloquedef.Text,
                        codigo = "01",
                        coddef = pck_defectos.SelectedItem.ToString().Substring(0, 2),
                        qcanti = Int32.Parse(ety_cantdefecto.Text.ToString()),
                        dobser = obsdefecto,//ety_obsdefecto.Text = null ?? "",
                        cgrupo = "N",
                        cardef = "16",
                        descri = pck_defectos.SelectedItem.ToString().Substring(5, (pck_defectos.SelectedItem.ToString().Length - 5)),//pck_defectos.SelectedValue.ToString()
                        defjpg = "",
                        vimage = false,
                        vphoto = true,
                        svigen = "N",
                    };
                    data.Insert(cdefecto);

                    xoperac = data.GetList<pdefec10>(false);
                    lsv_defectos.ItemsSource = xoperac.Where(x => x.clinea == pck_bloque.SelectedItem.ToString() && x.nsecue == Int32.Parse(lbl_nsecue.Text.ToString()) && x.careas == "16");
                    pck_defectos.SelectedItem = null;
                    ety_obsdefecto.Text = "";
                    ety_cantdefecto.Text = "";
                }
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
            pck_corte.SelectedItem = null;
            pck_combo.SelectedItem = null;
            pck_pieza.SelectedItem = null;
            lbl_cparti.Text = "";
            lbl_tela.Text = "";
            lbl_cencog.Text = "";
            lbl_tallas.Text = "";
            lbl_qprend.Text = "";
            lbl_qpanos.Text = "";
            ety_lote.Text = "";
            ety_muestra.Text = "";
            ety_observ.Text = "";
            lbl_bloquedef.Text = "";
            lbl_fechadef.Text = "";
            pck_defectos.SelectedItem = null;
            ety_obsdefecto.Text = "";
            ety_cantdefecto.Text = "";
            if (xoperac != null)
            {
                xoperac.Clear();
                lsv_defectos.ItemsSource = xoperac;
            }
            btn_buscarop.IsEnabled = false;            
            ety_op.IsEnabled = false;
            pck_corte.IsEnabled = false;
            pck_combo.IsEnabled = false;
            pck_pieza.IsEnabled = false;            
            ety_lote.IsEnabled = false;
            ety_muestra.IsEnabled = false;
            ety_observ.IsEnabled = false;
            pck_defectos.IsEnabled = false;
            ety_obsdefecto.IsEnabled = false;
            ety_cantdefecto.IsEnabled = false;
            btn_agregardefecto.IsEnabled = false;
            srb_audiaprobado.IsChecked = true;
            srb_audiaprobadoext.IsChecked = false;
            srb_audidesaprobado.IsChecked = false;
        }
        private void AddAuditoria()
        {
            //if (string.IsNullOrEmpty(ety_ctraba.Text))
            //{
            //    DisplayAlert("Error", "Ingrese un código de trabajador.", "OK");
            //    ety_ctraba.Focus();
            //    return;
            //}

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

            //if (pck_operacion.SelectedItem == null)
            //{
            //    DisplayAlert("Aviso", "Debe seleccionar la operación", "OK");
            //    return;
            //}

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

            string sapaud = "";
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

            try
            {
                if (nregis == "S")
                {
                    using (var data = new DataAccess())
                    {
                        if (string.IsNullOrEmpty(ety_cantdefecto.Text))
                        {
                            qdefectos = 0;
                        }
                        else
                        {
                            qdefectos = Int32.Parse(ety_cantdefecto.Text);
                        }
                        paudit01 nauditoria = new paudit01
                        {
                            idaudi = xidaudi,
                            careas = "16",
                            faudit = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                            nsecue = Int32.Parse(lbl_nsecue.Text.ToString()),
                            clinea = lbl_bloquedef.Text,
                            ctpord = "OP",
                            nordpr = ety_op.Text.ToString(),
                            nordct = pck_corte.SelectedItem.ToString().Trim(),
                            cmarbe = "",
                            ccarub = pck_combo.SelectedItem.ToString().Substring(0, pck_combo.SelectedItem.ToString().IndexOf("-") - 1),
                            npieza = Int32.Parse(pck_pieza.SelectedItem.ToString().Substring(0,1)),
                            cencog = lbl_cencog.Text,
                            citems = "",
                            ccolor = "",
                            npanos = Int32.Parse(lbl_qpanos.Text),
                            dtalla = lbl_tallas.Text,
                            qtotal = Int32.Parse(lbl_qprend.Text),
                            dlotes = "",
                            ctraba = "",// ety_ctraba.Text.ToString(),
                            copera = "",//pck_operacion.SelectedItem.ToString().Substring(0, pck_operacion.SelectedItem.ToString().IndexOf("-") - 1),
                            cprove = "",
                            cmaqui = "",
                            cturno = "",
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
                            flgrau = xreaud,
                            nreaud = 0,
                            caudit = VariableGlobal.ctraba,
                            flgext = "N",
                            cliref = "",
                            fauref = DateTime.Parse("1900-01-01 00:00:00"),
                            nseref = 0,
                            cusuar = VariableGlobal.cusuar,
                            fcreac = DateTime.Parse("1900-01-01 00:00:00"),
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
                        data.Insert(nauditoria);

                        taudit00 auditoria = new taudit00
                        {
                            idaudi = xidaudi,
                            careas = "16",
                            faudit = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                            nsecue = Int32.Parse(lbl_nsecue.Text.ToString()),
                            clinea = lbl_bloquedef.Text.ToString().Trim(),
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
                            nordct = pck_corte.SelectedItem.ToString().Trim(),
                            npieza = Int32.Parse(pck_pieza.SelectedItem.ToString().Substring(0, 1)),
                            dpieza = pck_pieza.SelectedItem.ToString().Substring(pck_pieza.SelectedItem.ToString().IndexOf("-") + 1, pck_pieza.SelectedItem.ToString().Length - (pck_pieza.SelectedItem.ToString().IndexOf("-") + 1)).Trim(),
                            clotei = lbl_cparti.Text,
                            citems = xcitems,
                            ditems = lbl_tela.Text,
                            cencog = lbl_cencog.Text,
                            dtalla = lbl_tallas.Text,
                            qprend = Int32.Parse(lbl_qprend.Text),
                            npanos = Int32.Parse(lbl_qpanos.Text),
                        };
                        data.Insert(auditoria);

                        //int qaudita = 0; int qauditx = 0; int qauditd = 0;

                        //if (sapaud == "A")
                        //{
                        //    var datos = data.GetList<caudit00>(false).Where(a => a.Clinea == pck_bloque.SelectedItem.ToString() && a.Careas == "16").ToList();
                        //    if (datos.Count > 0)
                        //    {
                        //        var ultsec = data.GetList<caudit00>(false).Where(a => a.Clinea == pck_bloque.SelectedItem.ToString() && a.Careas == "16").First();
                        //        qaudita = ultsec.Qprime + 1;
                        //        caudit00 cauditoria = new caudit00
                        //        {
                        //            idcaudit = ultsec.idcaudit,
                        //            Clinea = lbl_bloquedef.Text.ToString().Trim(),
                        //            IsVisible = false,
                        //            Qprime = qaudita,
                        //            Qapext = ultsec.Qapext,
                        //            Qrecha = ultsec.Qrecha,
                        //            Daudit = qaudita + ultsec.Qapext + ultsec.Qrecha + " Auditoria(s)",
                        //            Careas = "16",
                        //        };
                        //        data.Update(cauditoria);
                        //    }
                        //    else
                        //    {
                        //        qaudita = 1;
                        //        caudit00 cauditoria = new caudit00
                        //        {
                        //            Clinea = lbl_bloquedef.Text.ToString().Trim(),
                        //            IsVisible = false,
                        //            Qprime = qaudita,
                        //            Qapext = qauditx,
                        //            Qrecha = qauditd,
                        //            Daudit = "1 Auditoria(s)",
                        //            Careas = "16",
                        //        };
                        //        data.Insert(cauditoria);
                        //    }
                        //}
                        //if (sapaud == "E")
                        //{
                        //    var datos = data.GetList<caudit00>(false).Where(a => a.Clinea == pck_bloque.SelectedItem.ToString() && a.Careas == "16").ToList();
                        //    if (datos.Count > 0)
                        //    {
                        //        var ultsec = data.GetList<caudit00>(false).Where(a => a.Clinea == pck_bloque.SelectedItem.ToString() && a.Careas == "16").First();
                        //        qauditx = ultsec.Qapext + 1;
                        //        caudit00 cauditoria = new caudit00
                        //        {
                        //            idcaudit = ultsec.idcaudit,
                        //            Clinea = lbl_bloquedef.Text.ToString().Trim(),
                        //            IsVisible = false,
                        //            Qprime = ultsec.Qprime,
                        //            Qapext = qauditx,
                        //            Qrecha = ultsec.Qrecha,
                        //            Daudit = qauditx + ultsec.Qprime + ultsec.Qrecha + " Auditoria(s)",
                        //            Careas = "16",
                        //        };
                        //        data.Update(cauditoria);
                        //    }
                        //    else
                        //    {
                        //        qauditx = 1;
                        //        caudit00 cauditoria = new caudit00
                        //        {
                        //            Clinea = lbl_bloquedef.Text.ToString().Trim(),
                        //            IsVisible = false,
                        //            Qprime = qaudita,
                        //            Qapext = qauditx,
                        //            Qrecha = qauditd,
                        //            Daudit = "1 Auditoria(s)",
                        //            Careas = "16",
                        //        };
                        //        data.Insert(cauditoria);
                        //    }
                        //}
                        //if (sapaud == "D")
                        //{
                        //    var datos = data.GetList<caudit00>(false).Where(a => a.Clinea == pck_bloque.SelectedItem.ToString() && a.Careas == "16").ToList();
                        //    if (datos.Count > 0)
                        //    {
                        //        var ultsec = data.GetList<caudit00>(false).Where(a => a.Clinea == pck_bloque.SelectedItem.ToString() && a.Careas == "16").First();
                        //        qauditd = ultsec.Qrecha + 1;
                        //        caudit00 cauditoria = new caudit00
                        //        {
                        //            idcaudit = ultsec.idcaudit,
                        //            Clinea = lbl_bloquedef.Text.ToString().Trim(),
                        //            IsVisible = false,
                        //            Qprime = ultsec.Qprime,
                        //            Qapext = ultsec.Qapext,
                        //            Qrecha = qauditd,
                        //            Daudit = qauditd + ultsec.Qprime + ultsec.Qapext + " Auditoria(s)",
                        //            Careas = "16",
                        //        };
                        //        data.Update(cauditoria);
                        //    }
                        //    else
                        //    {
                        //        qauditd = 1;
                        //        caudit00 cauditoria = new caudit00
                        //        {
                        //            Clinea = lbl_bloquedef.Text.ToString().Trim(),
                        //            IsVisible = false,
                        //            Qprime = qaudita,
                        //            Qapext = qauditx,
                        //            Qrecha = qauditd,
                        //            Daudit = "1 Auditoria(s)",
                        //            Careas = "16",
                        //        };
                        //        data.Insert(cauditoria);
                        //    }
                        //}


                        var ldefec = data.GetList<pdefec10>(false).Where(x => x.svigen == "N").ToList();
                        foreach (var record in ldefec)
                        {
                            pdefec01 ndefecto = new pdefec01
                            {
                                careas = record.careas,
                                faudit = record.faudit,
                                nsecue = record.nsecue,
                                clinea = record.clinea,
                                codigo = record.codigo,
                                coddef = record.coddef,
                                qcanti = record.qcanti,
                                dobser = record.dobser,
                                cgrupo = record.cgrupo,
                                cardef = record.cardef,
                                senvio = "N",

                            };
                            data.Insert(ndefecto);

                            pdefec10 cdefecto = new pdefec10
                            {
                                iddefe = record.iddefe,
                                careas = record.careas,
                                faudit = record.faudit,
                                nsecue = record.nsecue,
                                clinea = record.clinea,
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
                            data.Update(cdefecto);
                        }
                    }
                }
                else
                {
                    using (var data = new DataAccess())
                    {
                        paudit01 nauditoria = new paudit01
                        {
                            idaudi = SelBloque.ElementAt(0).idaudi,
                            careas = "16",
                            faudit = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                            nsecue = Int32.Parse(lbl_nsecue.Text.ToString()),
                            clinea = lbl_bloquedef.Text,
                            ctpord = "OP",
                            nordpr = ety_op.Text.ToString(),
                            nordct = pck_corte.SelectedItem.ToString().Trim(),
                            cmarbe = "",
                            ccarub = pck_combo.SelectedItem.ToString().Substring(0, pck_combo.SelectedItem.ToString().IndexOf("-") - 1),
                            npieza = Int32.Parse(pck_pieza.SelectedItem.ToString().Substring(0, 1)),
                            cencog = lbl_cencog.Text,
                            citems = "",
                            ccolor = "",
                            npanos = Int32.Parse(lbl_qpanos.Text),
                            dtalla = lbl_tallas.Text,
                            qtotal = Int32.Parse(lbl_qprend.Text),
                            dlotes = "",
                            ctraba = "",//ety_ctraba.Text.ToString(),
                            copera = "",//pck_operacion.SelectedItem.ToString().Substring(0, pck_operacion.SelectedItem.ToString().IndexOf("-") - 1),
                            cprove = "",
                            cmaqui = "",
                            cturno = "",
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
                            flgext = "",
                            cliref = "",
                            fauref = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                            nseref = 0,
                            cusuar = "",
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
                        data.Update(nauditoria);

                        taudit00 auditoria = new taudit00
                        {
                            idaudi = SelBloque.ElementAt(0).idaudi,
                            careas = "16",
                            faudit = DateTime.Parse(dpk_fechaauditoria.Date.ToString()),
                            nsecue = Int32.Parse(lbl_nsecue.Text.ToString()),
                            clinea = lbl_bloquedef.Text.ToString().Trim(),
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
                            nordct = pck_corte.SelectedItem.ToString().Trim(),
                            npieza = Int32.Parse(pck_pieza.SelectedItem.ToString().Substring(0, 1)),
                            dpieza = pck_pieza.SelectedItem.ToString().Substring(pck_pieza.SelectedItem.ToString().IndexOf("-") + 1, pck_pieza.SelectedItem.ToString().Length - (pck_pieza.SelectedItem.ToString().IndexOf("-") + 1)).Trim(),
                            clotei = lbl_cparti.Text,
                            citems = xcitems,
                            ditems = lbl_tela.Text,
                            cencog = lbl_cencog.Text,
                            dtalla = lbl_tallas.Text,
                            qprend = Int32.Parse(lbl_qprend.Text),
                            npanos = Int32.Parse(lbl_qpanos.Text),
                        };
                        data.Update(auditoria);
                    }
                }

                DisplayAlert("Aviso", "Los datos se guardaron de manera satisfactoria.", "OK");
                LimpiaAuditoria();
                sgraud = "S";
            }
            catch (Exception ex)
            {
                DisplayAlert("Aviso", ex.StackTrace, "OK");
            }
        }

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
                Name = "16" + dpk_fechaauditoria.Date.Year.ToString() + dpk_fechaauditoria.Date.Month.ToString() + dpk_fechaauditoria.Date.Day.ToString() + lbl_bloquedef.Text + lbl_nsecue.Text + seldefecto.coddef.ToString() + ".jpg",
            });

            if (file == null)
                return;
            string desfot = "16" + dpk_fechaauditoria.Date.Year.ToString() + dpk_fechaauditoria.Date.Month.ToString() + dpk_fechaauditoria.Date.Day.ToString() + lbl_bloquedef.Text + lbl_nsecue.Text + seldefecto.coddef.ToString() + ".jpg"; //pck_defectos.SelectedItem.ToString().Substring(0, pck_defectos.SelectedItem.ToString().IndexOf("-") - 1) + ".jpg";
            //await DisplayAlert("File Location", file.Path, "OK");

            //img_defecto.Source = ImageSource.FromStream(() =>
            //{
            //    var stream = file.GetStream();
            //    file.Dispose();
            //    return stream;
            //});

            try
            {
                using (var data = new DataAccess())
                {
                    pdefec10 cdefecto = new pdefec10
                    {
                        iddefe = seldefecto.iddefe,
                        careas = "16",
                        faudit = dpk_fechaauditoria.Date,
                        nsecue = Int32.Parse(lbl_nsecue.Text),
                        clinea = pck_bloque.SelectedItem.ToString(),
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
                    data.Update(cdefecto);

                    xoperac = data.GetList<pdefec10>(false);
                    lsv_defectos.ItemsSource = xoperac.Where(x => x.clinea == pck_bloque.SelectedItem.ToString() && x.nsecue == Int32.Parse(lbl_nsecue.Text.ToString()) && x.careas == "16");
                }
                //await DisplayAlert("Aviso", "La imagen se guardo de manera satisfactoria.", "OK");
                using (var data = new DataAccess())
                {
                    xoperac = data.GetList<pdefec10>(false);
                    lsv_defectos.ItemsSource = xoperac.Where(x => x.clinea == pck_bloque.SelectedItem.ToString() && x.nsecue == Int32.Parse(lbl_nsecue.Text.ToString()) && x.careas == "16");
                }

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
            int lvindex = xoperac.IndexOf(e.SelectedItem as pdefec10);
            //DisplayAlert("Seleccionado", xoperac.ElementAt(lvindex).iddefe.ToString(), "OK");
            //xiddefe = xoperac.ElementAt(lvindex).iddefe;
            //xnameimg= xoperac.ElementAt(lvindex).defjpg.ToString();
        }

        private async void img_deletejpg_Tapped(object sender, TappedEventArgs e)
        {
            var result = await alertService.ShowMessage("Aviso", "Desea eliminar la imagen.");
            if (result == true)
            {
                var seldefecto = (e.Parameter) as pdefec10;
                try
                {
                    using (var data = new DataAccess())
                    {
                        pdefec10 cdefecto = new pdefec10
                        {
                            iddefe = seldefecto.iddefe,
                            careas = "16",
                            faudit = dpk_fechaauditoria.Date,
                            clinea = pck_bloque.SelectedItem.ToString(),
                            coddef = seldefecto.coddef,//pck_defectos.SelectedItem.ToString().Substring(0, 2),
                            descri = seldefecto.descri,//pck_defectos.SelectedItem.ToString().Substring(5, (pck_defectos.SelectedItem.ToString().Length - 5)),
                            defjpg = "",
                            vimage = false,
                            vphoto = true,
                        };
                        data.Update(cdefecto);

                        xoperac = data.GetList<pdefec10>(false);
                        lsv_defectos.ItemsSource = xoperac;
                    }
                    await DisplayAlert("Aviso", "La imagen se elimino de manera satisfactoria.", "OK");
                    using (var data = new DataAccess())
                    {
                        xoperac = data.GetList<pdefec10>(false);
                        lsv_defectos.ItemsSource = xoperac;
                    }
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
                using (var data = new DataAccess())
                {
                    var xlotes = data.GetList<ttcmue00>(false).Where(x => Int32.Parse(ety_lote.Text) >= x.ntanli && Int32.Parse(ety_lote.Text) <= x.ntanlf && x.nivaql.Trim() == "2.5");
                    foreach (var record in xlotes)
                    {
                        ety_muestra.Text = record.ntanmu.ToString();
                    }
                }

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

            using (var data = new DataAccess())
            {
                var xlotes = data.GetList<ttcmue00>(false).Where(x => Int32.Parse(ety_lote.Text) >= x.ntanli && Int32.Parse(ety_lote.Text) <= x.ntanlf && x.nivaql.Trim() == "2.5");
                foreach (var record in xlotes)
                {
                    ety_muestra.Text = record.ntanmu.ToString();
                }
            }

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

        private void Pck_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if( pck_combo.SelectedItem != null )
            { 
                CargarPiezas();
            }
        }

        private async void CargarPiezas()
        {
            var response = await apiService.GetPiezas<ppzxes00>(ety_op.Text,pck_combo.SelectedItem.ToString().Substring(0,3));
            if (!response.IsSuccess)
            {
                await DisplayAlert("Error", response.Message, "OK");                
                return;
            }

            //*** Llena Picker Piezas ***//
            pck_pieza.Items.Clear();
            var datapieza = (List<ppzxes00>)response.Result;
            foreach (var record in datapieza.OrderBy(x => x.npieza))
            {                
                pck_pieza.Items.Add(record.npieza.Trim()+" - "+record.dpieza.Trim());                
            }

            if (SelBloque.Count != 0)
            {
                pck_pieza.SelectedItem = SelBloque.ElementAt(0).npieza.ToString().Trim() + " - " + SelBloque.ElementAt(0).dpieza.ToString().Trim();
            }

        }

        private async void CargaCorte()
        {
            var response = await apiService.GetCorte<pcorte00>(ety_op.Text, pck_corte.SelectedItem.ToString(), pck_pieza.SelectedItem.ToString().Substring(0,1));
            if (!response.IsSuccess)
            {
                await DisplayAlert("Error", "No existe datos para la pieza "+ pck_pieza.SelectedItem.ToString().Substring(0, 1), "OK");
                lbl_cparti.Text = "";
                lbl_tela.Text = "";
                lbl_cencog.Text = "";
                lbl_tallas.Text = "";
                lbl_qprend.Text = "";
                lbl_qpanos.Text = "";
                pck_bloque.SelectedItem = null;
                return;
            }

            var datacorte = (List<pcorte00>)response.Result;
            foreach (var record in datacorte.OrderBy(x => x.npieza))
            {
                lbl_cparti.Text = record.clotei;
                lbl_tela.Text = record.ditems;
                lbl_cencog.Text = record.cencog;
                lbl_tallas.Text = record.dtalla;
                lbl_qprend.Text = record.qprend.ToString();
                lbl_qpanos.Text = record.npanos.ToString();
                pck_bloque.SelectedItem = record.nmodul;
                lbl_bloquedef.Text = pck_bloque.SelectedItem.ToString();
                lbl_fechadef.Text = dpk_fechaauditoria.Date.ToString("dd - MMM - yyyy");
                xcitems = record.citems;
            }
        }

        private void Pck_pieza_SelectedIndexChanged(object sender, EventArgs e)
        {
            if( pck_pieza.SelectedIndex != -1 )
            { 
                CargaCorte();
            }
        }

        private void Pck_corte_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pck_corte.SelectedIndex != -1)
            {
                CargaColorCorte();
            }
        }

        private async void CargaColorCorte()
        {
            var response = await apiService.GetCorteColor<padcor00>(ety_op.Text, pck_corte.SelectedItem.ToString());
            if (!response.IsSuccess)
            {
                await DisplayAlert("Error", response.Message, "OK");
                return;
            }

            var datacorte = (List<padcor00>)response.Result;
            foreach (var record in datacorte)
            {
                pck_combo.SelectedItem = record.ccarub.Trim() + " - " + record.dcarub.Trim();
            }
        }

    }
}