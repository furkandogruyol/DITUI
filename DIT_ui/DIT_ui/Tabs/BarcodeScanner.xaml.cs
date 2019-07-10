using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DIT_ui.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace DIT_ui.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarcodeScanner : ContentPage
    {
        public string getProduct = "http://ditwebapp.azurewebsites.net/api/Product/GetProductWithBarcode";
        public string url_last =  "http://ditwebapp.azurewebsites.net/api/ShoppingCart/GetLastCart";
        ZXingScannerPage scanPage;
        public HttpClient _client = new HttpClient();

        public BarcodeScanner()
        {
            InitializeComponent();
        }

        public async Task getScanner()
        {
            var options = new MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                TryHarder = true
            };

            var overlay = new ZXingDefaultOverlay
            {
                ShowFlashButton = false,
                TopText = "Barkodu bu alan içerisinde tutunuz.",
                Opacity = 0.90
            };
            overlay.BindingContext = overlay;

            scanPage = new ZXingScannerPage(options, overlay)
            {
                DefaultOverlayShowFlashButton = true,
                Title = "Barkod Okuyucu"
            };

            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = true;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        var model = await GetProduct(result.Text);

                                    Application.Current.Properties["ProductId"] = model.ProductId;
                                    Application.Current.Properties["ProductWeight"] = model.ProductWeight;
                                    Application.Current.Properties["ProductPrice"] = model.ProductPrice;
                                    Application.Current.Properties["ProductUrl"] = model.ProductUrl;

                        await Navigation.PushAsync(new Detail(model));

                    }
                    catch
                    {
                        await DisplayAlert("Hata", "Belirtilen ürün bulunamamaktadır.", "Geri");
                    }
                });
            };
           await Navigation.PushAsync(scanPage);
           
        }

        private async void Btn_getwithcode_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var model = await GetProduct(tb_barcode.Text);
                Application.Current.Properties["ProductId"] = model.ProductId;
                Application.Current.Properties["ProductWeight"] = model.ProductWeight;
                Application.Current.Properties["ProductPrice"] = model.ProductPrice;
                Application.Current.Properties["ProductUrl"] = model.ProductUrl;
                await Navigation.PushAsync(new Detail(model));
            }
            catch
            {
                await DisplayAlert("Hata", " Belirtilen ürün bulunmamaktadır", "Geri");
            }
        }

        private async void Btn_openscanner_OnClicked(object sender, EventArgs e)
        {
            await getScanner();
        }

        public async Task<Product> GetProduct (string pID)
        {
            var content = await _client.GetStringAsync(getProduct + "/" + pID);
            var pro = JsonConvert.DeserializeObject<Product>(content);

            return pro;
        }

    }
}