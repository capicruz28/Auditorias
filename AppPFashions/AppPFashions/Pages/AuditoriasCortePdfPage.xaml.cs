using AppPFashions.Interfaces;
using AppPFashions.Models;
using AppPFashions.Services;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppPFashions.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AuditoriasCortePdfPage : ContentPage
	{
        IDownloader downloader = DependencyService.Get<IDownloader>();
        public ObservableCollection<AuditFolder> Folders { get; set; }

        public ObservableCollection<SubFolder> SubFolders { get; set; }
        string dclien, nordpr;

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
        public AuditoriasCortePdfPage ()
		{
            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService();
            InitializeComponent();            
            BindingContext = this;
        }

        async void GenerateItems()
        {
            DependencyService.Get<IDownloader>().Show("Descargando");

            var response = await apiService.GetAuditoriaCortePdf<AuditoriaCorte>(ety_op.Text);
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Aviso", response.Message);
                DependencyService.Get<IDownloader>().Hide();
                return;
            }
            var fichas = (List<AuditoriaCorte>)response.Result;

            //********** INICIO DESCARGA DE ARCHIVOS DESDE FTP **********//            
            foreach (var recordf in fichas)
            {
                string fileName = "16"+recordf.faudit.Year.ToString()+recordf.faudit.ToString("MM")+recordf.faudit.ToString("dd")+recordf.clinea+recordf.nsecue+recordf.nordpr+recordf.nordct;
                IFolder rootFolder = await FileSystem.Current.GetFolderFromPathAsync("/storage/emulated/0/Fichas/");
                ExistenceCheckResult folderexist = await rootFolder.CheckExistsAsync(fileName);
                if (folderexist == ExistenceCheckResult.FileExists)
                {
                    IFile file = await rootFolder.GetFileAsync(fileName+".pdf");
                    await file.DeleteAsync();
                }

                string rutapdf = "ftp://192.168.2.55/" + fileName+".pdf";
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
                                 a.nordct,                                 
                             }
                         into b
                             select new
                             {
                                 Dficha = b.Key.nordct,                                 
                                 Qficha = b.Count()
                             }).ToList();

            foreach (var record in fichascab)
            {
                var subfol01 = new SubFolder();
                var subfolders = new ObservableCollection<SubFolder>();

                var detfol01 = new DetFolder();
                var detfolders = new ObservableCollection<DetFolder>();
                var fol01 = new AuditFolder() { FolderName = record.Dficha, ImageName = "ic_folder.png", AuditCount = record.Qficha, FontBold = 1 };

                var fichasdet = fichas.Where(x => x.nordct == record.Dficha).GroupBy(g=>g.dcarub).Select(s=>s.First()).ToList();
                foreach (var records in fichasdet)
                {                                        
                    
                    
                    subfol01 = new SubFolder() { FolderName = records.dcarub, ImageName = "ic_subfolder.png"};

                    var fichasfile = fichas.Where(x => x.nordct == record.Dficha && x.ccarub==records.ccarub).ToList();
                    foreach (var recordf in fichasfile)
                    {
                        string fileImage = "ic_check.png";
                        string fileName = "16" + recordf.faudit.Year.ToString() + recordf.faudit.ToString("MM") + recordf.faudit.ToString("dd") + recordf.clinea + recordf.nsecue + recordf.nordpr + recordf.nordct + ".pdf";
                        detfol01 = new DetFolder() { FolderName = fileName,  ImageName = "ic_pdf.png", SubImageName = fileImage };

                        detfolders.Add(detfol01);
                        
                    }
                    subfol01.DetFolder = detfolders;
                    subfolders.Add(subfol01);
                }

                fol01.SubFolder = subfolders;
                folders.Add(fol01);
            }

            Folders = folders;
            treeView.ItemsSource = Folders;

            DependencyService.Get<IDownloader>().Hide();

            ety_op.Text = "";
            
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
            string filename = "";
            try
            {
                var selficha = (e.Node.Content) as DetFolder;
                if (selficha != null)
                {
                    filename = selficha.FolderName ;

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