using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace AppPFashions.Models
{
    public class ReporteEficienciaDestajeroDetalle : INotifyPropertyChanged
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
        public string cbihor { get; set; }
        public double pefici { get; set; }        

        private void RaisePropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
