using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace AppPFashions.Models
{
    public class ReporteEficienciaBihorario : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;        
        private ImageSource photo;        
        public ImageSource Photo
        {
            get
            {
                return this.photo;
            }

            set
            {
                this.photo = value;
                this.RaisePropertyChanged("Photo");
            }
        }

        public string ctraba { get; set; }
        public string dtraba { get; set; }
        public string clinea { get; set; }
        public DateTime fproce { get; set; }
        public Int32 qmindi01 { get; set; }
        public Int32 qmindi02 { get; set; }
        public Int32 qmindi03 { get; set; }
        public Int32 qmindi04 { get; set; }
        public Int32 qmindi05 { get; set; }
        public Int32 qmindi06 { get; set; }
        public Int32 pefici01 { get; set; }
        public string imgpefici01 { get; set; }
        public Int32 pefici02 { get; set; }
        public string imgpefici02 { get; set; }
        public Int32 pefici03 { get; set; }
        public string imgpefici03 { get; set; }
        public Int32 pefici04 { get; set; }
        public string imgpefici04 { get; set; }
        public Int32 pefici05 { get; set; }
        public string imgpefici05 { get; set; }
        public Int32 pefici06 { get; set; }
        public string imgpefici06 { get; set; }
        public Int32 tqmindi { get; set; }
        public Int32 tpefici { get; set; }
        public string imgtpefici { get; set; }
        public string linecolor { get; set; }
        public List<LDatos> Data { get; set; }

        private void RaisePropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
    public class LDatos
    {
        public double Performance { get; set; }
    }
}
