using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DIT_ui.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DIT_ui.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingDetail : ContentPage
    {
        private string url_update =
            "http://ditwebapp.azurewebsites.net/api/ShoppingCart/UpdateProductCountWithShoppingCartId";

        private string ur_product = "http://ditwebapp.azurewebsites.net/api/Product/GetProducts";
        public ObservableCollection<Product> SepetListe;

        HttpClient _client = new HttpClient();
        public KullanıcıSepeti ks;

        public ShoppingDetail(KullanıcıSepeti model)
        {
            InitializeComponent();
            btn_back.IsEnabled = true;
            btn_update.IsEnabled = true;
            img_ürün.Source = model.productUrl;
            lb_adi.Text = model.productName;
            tb_miktar.Text = model.productAmount.ToString();
            lb_gram.Text += model.productWeight.ToString();
            lb_fiyat.Text += model.productPrice.ToString();
            ks = model;
        }


        private async void Btn_update_OnClicked(object sender, EventArgs e)
        {

            btn_back.IsEnabled = false;
            btn_update.IsEnabled = false;
            KullanıcıSepeti updateCart = new KullanıcıSepeti();
            Product p1 = new Product();
            try
            {
                var content = await _client.GetStringAsync(ur_product);
                var list = JsonConvert.DeserializeObject<List<Product>>(content);
                SepetListe = new ObservableCollection<Product>(list);
            }
            catch (Exception ecc)
            {
                await DisplayAlert("Hata", "İnternet bağlantını kontrol edip uygulamaya bir daha giriş yap!", "Tamam");
            }

            try
            {
                updateCart.productUrl = ks.productUrl;
                //Bunlar yanlıs birim fiyatı üzerinden işlem yapılması gerekiyor.
                updateCart.productWeight = SepetListe.Where(p => p.ProductId == ks.productId).First().ProductWeight;
                updateCart.productPrice = SepetListe.Where(p => p.ProductId == ks.productId).First().ProductPrice;
                updateCart.productId = ks.productId;
                updateCart.userId = ks.userId;
                updateCart.cartId = ks.cartId;
                //alttaki ikisi yanlıs alınıyor.
                updateCart.shoppingCartId = ks.shoppingCartId;
                updateCart.shoppingDate = ks.shoppingDate;
                updateCart.productAmount = Int32.Parse(tb_miktar.Text);

                var content = JsonConvert.SerializeObject(updateCart);
                var result = await _client.PutAsync(url_update + "/" + updateCart.shoppingCartId,
                    new StringContent(content, Encoding.UTF8, "application/json"));
                await Navigation.PopAsync();

            }
            catch (Exception ex)
            {

                await DisplayAlert("Hata", ex.Message, "Tamam");
            }

        }

        private async void Btn_back_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}