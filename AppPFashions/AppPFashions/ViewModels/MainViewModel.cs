using AppPFashions.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;


// CLASE CONTENEDORA DEL MENU LATERAL

namespace AppPFashions.ViewModels
{    
    public class MainViewModel
    {
        #region Properties
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public ObservableCollection<MenuItemViewModel> MenuUser { get; set; }
        public ObservableCollection<Auditorias> ListaAuditorias { get; set; }
        public LoginViewModel NewLogin { get; set; }
        public CosturaProcesoViewModel CosProVieMod { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            Menu = new ObservableCollection<MenuItemViewModel>();
            MenuUser = new ObservableCollection<MenuItemViewModel>();
            ListaAuditorias = new ObservableCollection<Auditorias>();
            NewLogin = new LoginViewModel();
            CosProVieMod = new CosturaProcesoViewModel();
            LoadMenu();
            LoadMenuUser();
            LoadAuditorias();
        }
        #endregion

        #region Methods
        private void LoadMenu()
        {
            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_home_pf.png",
                PageName = "HomePage",
                Title = "Página principal",  
                
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_fichas_pf.png",
                PageName = "ProductionOrderPage",
                Title = "Orden de producción",
                
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_fichas.png",
                PageName = "RecordsPage",
                Title = "Fichas técnicas",

            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_user.png",
                PageName = "LoginPage",
                Title = "Cuenta",

            });
        }

        private void LoadMenuUser()
        {
            MenuUser.Add(new MenuItemViewModel
            {
                Icon = "ic_home_pf.png",
                PageName = "UserPage",
                Title = "Página principal",

            });

            MenuUser.Add(new MenuItemViewModel
            {
                Icon = "ic_ccalidad.png",
                PageName = "QualityPage",
                Title = "Control de calidad",

            });

            MenuUser.Add(new MenuItemViewModel
            {
                Icon = "ic_costura.png",
                PageName = "SewingPage",
                Title = "Costura",

            });

            MenuUser.Add(new MenuItemViewModel
            {
                Icon = "ic_acabado.png",
                PageName = "FinishesPage",
                Title = "Acabados",

            });

            MenuUser.Add(new MenuItemViewModel
            {
                Icon = "ic_csesion.png",
                PageName = "LogoutPage",
                Title = "Cerrar Sesión",

            });
        }

        private void LoadAuditorias()
        {
            ListaAuditorias.Add(new Auditorias
            {
                AuditoriaImage = "corte.png",
                AuditoriaName = "Corte",
                AuditoriaPage = "AuditoriaCortePage",

            });
            ListaAuditorias.Add(new Auditorias
            {
                AuditoriaImage = "costurap.png",
                AuditoriaName = "Costura Proceso",
                AuditoriaPage = "CosturaProcesoPage",

            });
            ListaAuditorias.Add(new Auditorias
            {
                AuditoriaImage = "costuraf.png",
                AuditoriaName = "Costura Final",
                AuditoriaPage = "CosturaFinalPage",

            });
            ListaAuditorias.Add(new Auditorias
            {
                AuditoriaImage = "servicios.png",
                AuditoriaName = "Servicios",
                AuditoriaPage = "AuditoriaServiciosPage",

            });
            ListaAuditorias.Add(new Auditorias
            {
                AuditoriaImage = "bordado.png",
                AuditoriaName = "Bordado",
                AuditoriaPage = "AuditoriaBordadoPage",

            });
            ListaAuditorias.Add(new Auditorias
            {
                AuditoriaImage = "estampado.png",
                AuditoriaName = "Estampado",
                AuditoriaPage = "AuditoriaEstampadoPage",

            });
            ListaAuditorias.Add(new Auditorias
            {
                AuditoriaImage = "transfer.png",
                AuditoriaName = "Transfer",

            });
            ListaAuditorias.Add(new Auditorias
            {
                AuditoriaImage = "acabadoi.png",
                AuditoriaName = "Ingreso Acabado",

            });
            ListaAuditorias.Add(new Auditorias
            {
                AuditoriaImage = "acabadop.png",
                AuditoriaName = "Acabado Proceso",

            });
            ListaAuditorias.Add(new Auditorias
            {
                AuditoriaImage = "acabadov.png",
                AuditoriaName = "Acabado Vaporizado",

            });
            ListaAuditorias.Add(new Auditorias
            {
                AuditoriaImage = "acabadof.png",
                AuditoriaName = "Acabado Final",

            });
            ListaAuditorias.Add(new Auditorias
            {
                AuditoriaImage = "segundas.png",
                AuditoriaName = "Segundas",

            });
            ListaAuditorias.Add(new Auditorias
            {
                AuditoriaImage = "lavanderia.png",
                AuditoriaName = "Lavanderia",

            });
            ListaAuditorias.Add(new Auditorias
            {
                AuditoriaImage = "auditoriaf.png",
                AuditoriaName = "Final",

            });
        }
        #endregion
    }
}
