using AppPFashions.Models;
using Syncfusion.SfKanban.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AppPFashions.ViewModels
{
    [Preserve(AllMembers = true)]
    public class KanbanCustomViewModel
    {
        List<taudit00> auditcp;
        List<taudit00> auditcf;
        mtraba00 ldtraba;
        public ObservableCollection<CustomKanbanModel> Cards { get; set; }  

        public KanbanCustomViewModel()
        {                     

            Cards = new ObservableCollection<CustomKanbanModel>();


            auditcp = App.baseDatos.GetList<taudit00>(false).Where(x => x.careas == "19" && x.status == "D" && x.sreaud == "N").OrderBy(x => x.faudit).ToList();
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
                    Dtraba = ldtraba.ctraba + " - " + ldtraba.dtraba,
                    Careas = recordcp.careas,
                    Faudit = recordcp.faudit,
                    Nsecue = recordcp.nsecue,
                });
            }

            auditcf = App.baseDatos.GetList<taudit00>(false).Where(x => x.careas == "FC" && x.status == "D" && x.sreaud == "N").OrderBy(x => x.faudit).ToList();
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
                    Dcolor = recordcf.ccarub + " - " + recordcf.dcarub,
                    Careas = recordcf.careas,
                    Faudit = recordcf.faudit,
                    Nsecue = recordcf.nsecue,
                });
            }
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
