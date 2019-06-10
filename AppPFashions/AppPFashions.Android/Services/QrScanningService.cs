using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppPFashions.Services;
using Xamarin.Forms;
using ZXing.Mobile;

[assembly:Dependency(typeof(AppPFashions.Droid.Services.QrScanningService))]

namespace AppPFashions.Droid.Services
{
    public class QrScanningService : IQrScanningService
    {
        public async Task<string> ScanAsync()
        {
            var optionsDefault = new MobileBarcodeScanningOptions();
            var optionsCustom = new MobileBarcodeScanningOptions();

            var scanner = new MobileBarcodeScanner()
            {
                TopText = "",
                BottomText = "",
            };


            var scanResult = await scanner.Scan(optionsCustom);
            return scanResult.Text;
        }
    }
}