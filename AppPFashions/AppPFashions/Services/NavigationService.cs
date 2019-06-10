using AppPFashions.Models;
using AppPFashions.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

// SERVICIO DE NAVEGACION
namespace AppPFashions.Services
{
    public class NavigationService
    {
        #region Attributes
        private DataService dataService;
        #endregion

        public NavigationService()
        {
            dataService = new DataService();
        }

        public async Task Navigate(string pageName)
        {
            App.Master.IsPresented = false;   
            switch (pageName)
            {
                case "HomePage":
                    await App.Navigator.PushAsync(new HomePage());
                    break;
                case "RecordsPage":
                    await App.Navigator.PushAsync(new RecordsPage());
                    break;
                case "ProductionOrderPage":
                    await App.Navigator.PushAsync(new ProductionOrderPage());
                    break;
                case "LoginPage":
                    await App.Navigator.PushAsync(new LoginPage());
                    break;
                case "UserPage":
                    await App.Navigator.PushAsync(new UserPage());
                    break;
                case "LogoutPage":
                    Logout();
                    break;
            }
        }

        public async Task NavigateUser(string pageName)
        {
            App.UserMaster.IsPresented = false;
            switch (pageName)
            {
                case "UserPage":
                    await App.Navigator.PushAsync(new UserPage());
                    break;
                case "QualityPage":
                    await App.Navigator.PushAsync(new QualityPage());
                    break;
                case "SewingPage":
                    await App.Navigator.PushAsync(new SewingPage());
                    break;
                case "FinishesPage":
                    await App.Navigator.PushAsync(new FinishesPage());
                    break;  
                case "CutPage":
                    await App.Navigator.PushAsync(new CutPage());
                    break;
                case "LogoutPage":
                    Logout();
                    break;
                case "AuditoriaCortePage":
                    await App.Navigator.PushAsync(new CosturaProcesoConsultaPage("16"));
                    break;
                case "CosturaProcesoPage":
                    //await App.Navigator.PushAsync(new CosturaProcesoConsultaPage("19"));
                    await App.Navigator.PushAsync(new ResumenAuditoriasPage("19"));
                    break;
                case "CosturaFinalPage":
                    //await App.Navigator.PushAsync(new CosturaProcesoConsultaPage("FC"));
                    await App.Navigator.PushAsync(new ResumenAuditoriasPage("FC"));
                    break;
                case "AuditoriaBordadoPage":
                    await App.Navigator.PushAsync(new CosturaProcesoConsultaPage("29"));
                    break;
            }
        }

        private void Logout()
        {
            App.CurrentUser.IsRemembered = false;
            dataService.UpdateUser(App.CurrentUser);
            App.Current.MainPage = new MasterPage();
        }

        internal void SetMainPage(Usuario user)
        {
            App.CurrentUser = user;
            App.Current.MainPage = new MasterUserPage();
        }
    }
}
