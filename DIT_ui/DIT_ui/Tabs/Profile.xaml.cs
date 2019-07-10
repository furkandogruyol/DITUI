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
	public partial class Profile : ContentPage
    {
        string url = "http://ditwebapp.azurewebsites.net/api/ShoppingCart/GetLastCart";
        string url_delete = "http://ditwebapp.azurewebsites.net/api/ShoppingCart/DeleteShoppingCartWithCartId";
        public HttpClient _client = new HttpClient();

        public Profile ()
		{
		    InitializeComponent();

		    if (Application.Current.Properties.ContainsKey("CartId"))
		    {
		        btn_start.IsEnabled = false;
		        btn_iptal.IsVisible = true;
		        btn_iptal.IsEnabled = true;
		        lb_sepet.IsVisible = true;
		    }
		    else
		    {
		        btn_start.IsEnabled = true;
		        btn_iptal.IsVisible = false;
		        btn_iptal.IsEnabled = false;
		        lb_sepet.IsVisible = false;
		    }

            lb_user.Text = Application.Current.Properties["Username"].ToString();
		}

	    private async void Btn_start_OnClicked(object sender, EventArgs e)
        {
            var lastCart = await GetCartId();
            var lastCartId = lastCart.cartId;
         
            Application.Current.Properties["CartId"] = ++lastCartId;
            await Navigation.PushAsync(new BarcodeScanner());
            btn_iptal.IsVisible = true;
            btn_iptal.IsEnabled = true;
            lb_sepet.IsVisible = true;

        }

        public async Task<KullanıcıSepeti> GetCartId()
        {
            var content = await _client.GetStringAsync(url);
            var cart = JsonConvert.DeserializeObject<KullanıcıSepeti>(content);
            return cart;
        }

        private async void Btn_iptal_OnClicked(object sender, EventArgs e)
        {
            var action = await DisplayAlert("Alışveriş İptal", "Alışverişi iptal etmek istediğinizden emin misiniz?", "Evet", "Hayır");
            if (action)
            {
                await _client.DeleteAsync(url_delete + "/" + Application.Current.Properties["CartId"].ToString());
                Application.Current.Properties.Remove("CartId");
                btn_iptal.IsVisible = false;
                btn_iptal.IsEnabled = false;
                btn_start.IsEnabled = true;
                lb_sepet.IsVisible = false;
            }
           
        }
    }
}