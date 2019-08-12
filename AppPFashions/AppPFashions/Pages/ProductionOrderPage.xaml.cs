using AppPFashions.Data;
using AppPFashions.Interfaces;
using AppPFashions.Models;
using AppPFashions.Services;
using PCLStorage;
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
	public partial class ProductionOrderPage : INotifyPropertyChanged
    {
        IDownloader downloader = DependencyService.Get<IDownloader>();
        public ObservableCollection<AuditFolder> Folders { get; set; }

        public ObservableCollection<SubFolder> SubFolders { get; set; }
        string dclien,nordpr;

        public string Dclien
        {
            get
            {
                return dclien;
            }
            set
            {
                if (dclien != value)
                {
                    dclien = value;
                    OnPropertyChanged("Dclien");
                }
            }
        }
        public string Nordpr
        {
            get
            {
                return nordpr;
            }
            set
            {
                if (nordpr != value)
                {
                    nordpr = value;
                    OnPropertyChanged("Nordpr");
                }
            }
        }

        private ApiService apiService;
        private DialogService dialogService;
        private AlertService alertService;
        private DataService dataService;

        public ProductionOrderPage ()
		{
            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService();
            InitializeComponent ();
            //downloader.OnFileDownloaded += OnFileDownloaded;
            BindingContext = this;            
        }

        async void GenerateItems()
        {
            DependencyService.Get<IDownloader>().Show("Descargando");
         
            var response = await apiService.GetFichasPdf<OrdenProduccion>(ety_op.Text);
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Aviso", response.Message);
                DependencyService.Get<IDownloader>().Hide();
                return;
            }
            var fichas = (List<OrdenProduccion>)response.Result;

            //********** INICIO DESCARGA DE ARCHIVOS DESDE FTP **********//            
            foreach (var recordf in fichas)
            {
                string fileName = recordf.drutaf + ".pdf";
                IFolder rootFolder = await FileSystem.Current.GetFolderFromPathAsync("/storage/emulated/0/Fichas/");
                ExistenceCheckResult folderexist = await rootFolder.CheckExistsAsync(fileName);
                if (folderexist == ExistenceCheckResult.FileExists)
                {
                    IFile file = await rootFolder.GetFileAsync(fileName);
                    await file.DeleteAsync();
                }

                string rutapdf = "ftp://192.168.2.55/" + recordf.drutaf + ".pdf";
                downloader.DownloadFile(rutapdf, "Fichas");
            }            
            //********** FIN DESCARGA DE ARCHIVOS DESDE FTP **********//

            foreach (var recordc in fichas)
            {
                Dclien = recordc.dclien;
                Nordpr = recordc.nordpr;
            }

            var folders = new ObservableCollection<AuditFolder>();

            treeView.Nodes.Clear();

            var fichascab = (from a in fichas
                            group a by new
                            {
                                a.cdaxes,
                                a.dficha,                                
                            }
                         into b
                            select new
                            {
                                Dficha = b.Key.dficha,
                                Cdaxes = b.Key.cdaxes,
                                Qficha = b.Count()
                            }).ToList();

            foreach (var record in fichascab)
            {
                var subfol01 = new SubFolder();
                var subfolders = new ObservableCollection<SubFolder>();
                var fol01 = new AuditFolder() { FolderName = record.Dficha, ImageName = "ic_folder.png", AuditCount = record.Qficha, FontBold = 1 };

                var fichasdet = fichas.Where(x => x.cdaxes == record.Cdaxes).ToList();
                foreach (var records in fichasdet)
                {
                    string fileName = records.drutaf + ".pdf";
                    string fileImage = "ic_check.png";
                    //FileInfo fi = new FileInfo("/storage/emulated/0/Fichas/" + fileName);
                    //if (fi.Length == 0){ fileImage = "ic_cancel.png"; }
                    //else{ fileImage = "ic_check.png"; }

                    if (string.IsNullOrEmpty(records.dcolor))
                    {                                               
                        subfol01 = new SubFolder() { FolderName = records.drutaf, SubFolderName=records.drutaf , ImageName = "ic_pdf.png" ,SubImageName = fileImage };
                    }
                    else
                    {
                        subfol01 = new SubFolder() { FolderName = records.dcolor, SubFolderName = records.drutaf , ImageName = "ic_pdf.png", SubImageName = fileImage };
                    }
                    subfolders.Add(subfol01);
                }

                fol01.SubFolder = subfolders;
                folders.Add(fol01);
            }

            Folders = folders;
            treeView.ItemsSource = Folders;

            //********** INICIO INSERTA EN SQLITE MOBILE **********//
            //using (var data = new DataAccess())
            //{
            //    var dataop = data.GetOP(ety_op.Text);

            //    if (dataop.Count == 0)
            //    {
            //        dataService.Save(fichas);
            //    }
            //    else
            //    {
            //        foreach (var recordf in fichas)
            //        {
            //            string fileName = recordf.drutaf + ".pdf";
            //            IFolder rootFolder = await FileSystem.Current.GetFolderFromPathAsync("/storage/emulated/0/Fichas/");
            //            ExistenceCheckResult folderexist = await rootFolder.CheckExistsAsync(fileName);
            //            if (folderexist == ExistenceCheckResult.FileExists)
            //            {
            //                IFile file = await rootFolder.GetFileAsync(fileName);
            //                await file.DeleteAsync();
            //            }
            //        }
            //        data.DeleteFichaOP(ety_op.Text);                             
            //        dataService.Save(fichas);
            //    }
            //}
            //********** FIN INSERTA EN SQLITE MOBILE **********//
            DependencyService.Get<IDownloader>().Hide();

            ety_op.Text = "";            
            //await DisplayAlert("Aviso", "Archivos descargados satisfactoriamente", "OK");
        }

        private void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                DisplayAlert("Aviso", "File Saved Successfully", "Close");
            }
            else
            {
                DisplayAlert("Aviso", "Error while saving the file", "Close");
            }
        }

        private async void TreeView_ItemTapped(object sender, Syncfusion.XForms.TreeView.ItemTappedEventArgs e)
        {
            string filename="";
            try
            {       
                var selficha = (e.Node.Content) as SubFolder;
                if (selficha != null)
                {                    
                    filename = selficha.SubFolderName+".pdf";                    

                    IFolder rootFolder = await FileSystem.Current.GetFolderFromPathAsync("/storage/emulated/0/Fichas/");
                    ExistenceCheckResult folderexist = await rootFolder.CheckExistsAsync(filename);
                    if (folderexist == ExistenceCheckResult.FileExists)
                    {
                        DependencyService.Get<IFileManager>().OpenFile("/storage/emulated/0/Fichas/" + filename);
                    }
                }                
            }
            catch (Exception ex)
            {                
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void Btn_buscar_op_Clicked(object sender, EventArgs e)
        {            
            GenerateItems();          
        }
    }
}