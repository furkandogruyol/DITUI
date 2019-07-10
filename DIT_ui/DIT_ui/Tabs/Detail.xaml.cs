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

namespace DIT_ui.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Detail : ContentPage
    {
        string url = "http://ditwebapp.azurewebsites.net/api/ShoppingCart/CreateShoppingCart";
        private HttpClient _client = new HttpClient();
        private Product p1;

        public Detail(Product model)
        {
            InitializeComponent();
            img_ürün.Source = model.ProductUrl;
            lb_adi.Text += model.ProductName;
            tb_miktar.Text = "1";
            lb_gram.Text += model.ProductWeight.ToString();
            lb_fiyat.Text += model.ProductPrice.ToString();
            p1 = model;
            btn_back.IsEnabled = true;
            btn_addToCart.IsEnabled = true;
        }



        public async void Btn_addToCart_OnClicked(object sender, EventArgs e)
        {
            Cart c1 = new Cart();
            await createCartObjAsync();
            await c1.refreshListAsync();
            await Navigation.PushAsync(new MainPage());
        }

        public async Task createCartObjAsync()
        {

            KullanıcıSepeti cart = new KullanıcıSepeti();
            int userId = Int32.Parse(Application.Current.Properties["UserId"].ToString());
            long productId = long.Parse(Application.Current.Properties["ProductId"].ToString());

            cart.productId = productId;
            cart.userId = userId;
            cart.shoppingDate = DateTime.Now;
            cart.cartId = Int32.Parse(Application.Current.Properties["CartId"].ToString());
            cart.productAmount = Int32.Parse(tb_miktar.Text);
            cart.productUrl = p1.ProductUrl;
            //bu kısım değişti miktar ile carpmıltım
            cart.productPrice = Convert.ToDouble(Application.Current.Properties["ProductPrice"]);
            cart.productWeight = Convert.ToDouble(Application.Current.Properties["ProductWeight"]);


            try
            {
                btn_back.IsEnabled = false;
                btn_addToCart.IsEnabled = false;
                var content = JsonConvert.SerializeObject(cart);

                var result = await _client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));

            }
            catch (Exception exception)
            {
                await DisplayAlert(exception.Message.ToString(), "Sorun", "Tamam");
            }
        }

        private async void Btn_back_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BarcodeScanner());
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
    }
}