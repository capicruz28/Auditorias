using Acr.UserDialogs;
using AppPFashions.Data;
using AppPFashions.Models;
using AppPFashions.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppPFashions.ViewModels
{
    public class CosturaProcesoViewModel :  INotifyPropertyChanged
    {
        #region Attributes
        private DialogService dialogService;
        private AlertService alertService;
        private ApiService apiService;
        private DataService dataService;
        private DataAccess dataAccess;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        ICommand tapCommand;
        ICommand tapCommandOperaciones;
        ICommand tapCommandDefectos;

        string _ctraba;
        string _dtraba;
        string _nordpr;
        string _dclien;
        string _bloque;
        string _defecto;
        Int32 _cantdefecto;
        DateTime _fecha;
        private bool isRefreshing;

        //private ObservableCollection<string> _drivers;
        public ObservableCollection<string> ListCorte { get; set; }
        public ObservableCollection<string> ListCombo { get; set; }
        public ObservableCollection<string> ListOperacion { get; set; }
        public ObservableCollection<string> ListDefecto { get; set; }

        private ObservableCollection<topera01> orderInfo;
        ObservableCollection<pdefec10> listaudi;
        //public ObservableCollection<topera01> OrderInfoCollection
        //{
        //    get { return orderInfo; }
        //    set { this.orderInfo = value; }
        //}
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<pdefec10> ListaAuditoria
        {
            get
            {
                return listaudi;
            }
            set
            {
                listaudi = value;
                OnPropertyChanged();
            }
        }

        public string SelBloque
        {
            get
            {
                return _bloque;
            }
            set
            {
                if (_bloque != value)
                {
                    _bloque = value;
                    PropertyChanged?.Invoke(this,
                                            new PropertyChangedEventArgs(nameof(SelBloque)));
                }
            }
        }

        public DateTime SelFecha
        {
            get
            {
                return _fecha;
            }
            set
            {
                if (_fecha != value)
                {
                    _fecha = value;
                    PropertyChanged?.Invoke(this,
                                            new PropertyChangedEventArgs(nameof(SelFecha)));
                }
            }
        }

        public string SelDefecto
        {
            get
            {
                return _defecto;
            }
            set
            {
                if (_defecto != value)
                {
                    _defecto = value;
                    PropertyChanged?.Invoke(this,
                                            new PropertyChangedEventArgs(nameof(SelDefecto)));
                }
            }
        }

        public Int32 SelCantDefecto
        {
            get
            {
                return _cantdefecto;
            }
            set
            {
                if (_cantdefecto != value)
                {
                    _cantdefecto = value;
                    PropertyChanged?.Invoke(this,
                                            new PropertyChangedEventArgs(nameof(SelCantDefecto)));
                }
            }
        }
        public ObservableCollection<topera01> OrderInfoCollection
        {
            get
            {
                return orderInfo;
            }
            set
            {
                orderInfo = value;
                OnPropertyChanged();
            }
        }


        private void GenerateOrders()
        {
            this.IsRefreshing = true;
            using (var data = new DataAccess())
            {
                var xoperac = data.GetList<pdefec10>(false);
                listaudi = new ObservableCollection<pdefec10>(xoperac);          
            }
            this.IsRefreshing = false;
        }


        public CosturaProcesoViewModel()
        {
            tapCommand = new Command(OnTapped);
            tapCommandOperaciones = new Command(OnTappedOperaciones);
            tapCommandDefectos = new Command(OnTappedDefectos);
            dialogService = new DialogService();
            alertService = new AlertService();
            apiService = new ApiService();
            dataService = new DataService();
            dataAccess = new DataAccess();
            ListCorte = new ObservableCollection<string>();
            ListCombo = new ObservableCollection<string>();
            ListOperacion = new ObservableCollection<string>();
            ListDefecto = new ObservableCollection<string>();
            orderInfo = new ObservableCollection<topera01>();
            SelFecha = DateTime.Now;
            this.GenerateOrders();
            SectorList = new List<string>              
            {
                "01","02","03","04","05","06",
                "07","08","09","10","11","12","SE"
            };
        }
        public ICommand SincroOperarios
        {
            get { return tapCommand; }
        }

        public ICommand SincroOperaciones
        {
            get { return tapCommandOperaciones; }
        }

        public ICommand SincroDefectos
        {
            get { return tapCommandDefectos; }
        }
        async void OnTapped(object s)
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
                int xtraba=0;

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
                        fooDialog.Title =  xtraba + " de " + xqtraba;                        
                        await Task.Delay(10);
                    }
                }                                
            }
            else // if it's equal to Cancel
            {
                return; // just return to the page and do nothing.
            }

            //await dialogService.ShowMessage("Image", "Tapped");
        }

        async void OnTappedOperaciones(object s)
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
      
                        };
                        dataService.InsertOperacion(operario);
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

            //await dialogService.ShowMessage("Image", "Tapped");
        }

        async void OnTappedDefectos(object s)
        {
            var result = await alertService.ShowMessage("Aviso", "Desea sincronizar la lista de defectos");
            if (result == true)
            {
                dataAccess.DeleteDefectos();
                var response = await apiService.Defectos<mdefec00>(Ctraba);

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
            //await dialogService.ShowMessage("Image", "Tapped");
        }

        public ICommand BuscarOperarioCommand
        {
            get
            {
                return new RelayCommand(BusOperario);
            }
        }

        public ICommand BuscarOP
        {
            get
            {
                return new RelayCommand(BusOP);
            }
        }

        public ICommand GuardarDefecto
        {
            get
            {
                return new RelayCommand(AddDefecto);
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GenerateOrders);
            }
        }

        private async void BusOperario()
        {
            if (string.IsNullOrEmpty(Ctraba))
            {
                await dialogService.ShowMessage("Error", "Ingrese un código de trabajador.");
                return;
            }

            if (Ctraba.Length == 3)
            {
                Ctraba = "O00" + Ctraba;
            }

            if (Ctraba.Length == 4)
            {
                Ctraba = "O0" + Ctraba;
            }

            if (Ctraba.Length == 5)
            {
                Ctraba = "O" + Ctraba;
            }

            //var scanner = DependencyService.Get<IQrScanningService>();
            //var result = await scanner.ScanAsync();
            //if (result != null)
            //{
            //    Ctraba = result;
            //}
            using (var datos = new DataAccess())
            {
                var xctraba = datos.GetOperario(Ctraba);
                if (xctraba == null)
                {
                    await dialogService.ShowMessage("Error", "No código de trabajador no existe o no esta activo");
                    return;
                }
                Dtraba = xctraba.dtraba;
            }
            
        }

        private async void BusOP()
        {
            if (string.IsNullOrEmpty(Nordpr))
            {
                await dialogService.ShowMessage("Error", "Ingrese un número de OP");
                return;
            }    

            var response = await apiService.GetOP<ordprod>(Nordpr);
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            var dataop = (List<ordprod>)response.Result;
            foreach (var record in dataop.OrderBy(x=>x.nordct))
            {
                Dclien = record.dclien;
                ListCorte.Add(record.nordct);
                //ListCombo.Add(record.ccarub+" "+record.dcarub);
            }
            var datos = (from dat in dataop
                         group dat by new { dat.ccarub, dat.dcarub } into g
                         select new { ccarub = g.Key.ccarub, dcarub = g.Key.dcarub });
            foreach (var record in datos)
            {                                
                ListCombo.Add(record.ccarub + " - " + record.dcarub);
            }

            using (var data = new DataAccess())
            {
                var xoperac = data.GetList<topera01>(false);                     
                if (xoperac == null)
                {
                    await dialogService.ShowMessage("Error", "No existen registros");
                    return;
                }
                foreach (var record in xoperac)
                {
                    ListOperacion.Add(record.cclave + " - " + record.descri);
                }
            }

            using (var data = new DataAccess())
            {
                var xdefec = data.GetList<mdefec00>(false).Where(x=>x.csecci=="19");
                if (xdefec == null)
                {
                    await dialogService.ShowMessage("Error", "No existen registros");
                    return;
                }
                foreach (var record in xdefec)
                {
                    ListDefecto.Add(record.coddef + " - " + record.descri);
                }
            }
        }

        private async void AddDefecto()
        {
            using (var data = new DataAccess())
            {
                pdefec01 ndefecto = new pdefec01
                {
                    careas = "19",
                    faudit = SelFecha,
                    nsecue = 1,
                    clinea = SelBloque,
                    codigo = "02",
                    coddef = SelDefecto.Substring(0, 2),
                    qcanti = SelCantDefecto,
                    dobser = "",
                    cgrupo = "N",
                    cardef = "19",

                };
                data.Insert(ndefecto);

                pdefec10 cdefecto = new pdefec10
                {
                    careas = "19",
                    faudit = SelFecha,
                    clinea = SelBloque,
                    coddef = SelDefecto.Substring(0, 2),
                    descri = SelDefecto.Substring(5, (SelDefecto.Length - 5)),
                };
                data.Insert(cdefecto);

            }
            await dialogService.ShowMessage("Aviso", "Los datos se guardaron de manera satisfactoria.");            
        }

        public string Dtraba
        {
            get
            {
                return _dtraba;
            }
            set
            {
                if (_dtraba != value)
                {
                    _dtraba = value;
                    PropertyChanged?.Invoke(this,
                                            new PropertyChangedEventArgs(nameof(Dtraba)));
                }
            }
        }

        public string Ctraba
        {
            get
            {
                return _ctraba;
            }
            set
            {
                if (_ctraba != value)
                {
                    _ctraba = value;
                    PropertyChanged?.Invoke(this,
                                            new PropertyChangedEventArgs(nameof(Ctraba)));
                }
            }
        }

        public string Nordpr
        {
            get
            {
                return _nordpr;
            }
            set
            {
                if (_nordpr != value)
                {
                    _nordpr = value;
                    PropertyChanged?.Invoke(this,
                                            new PropertyChangedEventArgs(nameof(Nordpr)));
                }
            }
        }

        public string Dclien
        {
            get
            {
                return _dclien;
            }
            set
            {
                if (_dclien != value)
                {
                    _dclien = value;
                    PropertyChanged?.Invoke(this,
                                            new PropertyChangedEventArgs(nameof(Dclien)));
                }
            }
        }

        public List<string> SectorList { get; set; }
    
    }
}
