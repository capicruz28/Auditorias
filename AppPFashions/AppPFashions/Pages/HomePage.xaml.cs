using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppPFashions.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{            
			InitializeComponent ();
            BindingContext = this;
            if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
            {

                ImageCollection.Add(new RotatorModel("Confeccion.jpg"));
                ImageCollection.Add(new RotatorModel("Tejeduria.jpg"));
                ImageCollection.Add(new RotatorModel("Tintoreria.jpg"));
                ImageCollection.Add(new RotatorModel("Rama.jpg"));
                ImageCollection.Add(new RotatorModel("Lavanderias.jpg"));                
            }
            //var ImageCollection = new List<RotatorModel> {
            //new RotatorModel ("Confeccion.jpg"),
            //new RotatorModel ("Tejeduria.jpg"),
            //new RotatorModel ("Tintoreria.jpg"),
            //new RotatorModel ("Rama.jpg"),
            //new RotatorModel ("Lavanderias.jpg"),           
            //};

            //rotator.ItemsSource = ImageCollection;
        }

        private List<RotatorModel> imageCollection = new List<RotatorModel>();
        public List<RotatorModel> ImageCollection
        {
            get { return imageCollection; }
            set { imageCollection = value; }
        }
    }

    public class RotatorModel
    {
        public RotatorModel(string imageString)
        {
            Image = imageString;
        }
        private String _image;
        public String Image
        {
            get { return _image; }
            set { _image = value; }
        }
    }

}