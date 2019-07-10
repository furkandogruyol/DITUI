using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DIT_ui.Auth;
using DIT_ui.Menu;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DIT_ui.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        string url_delete = "http://ditwebapp.azurewebsites.net/api/ShoppingCart/DeleteShoppingCartWithCartId";
        public HttpClient _client = new HttpClient();

        public MainPage()
        {
            InitializeComponent();
        }

        private void MainPage_OnCurrentPageChanged(object sender, EventArgs e)
        {
            this.Title = this.CurrentPage.Title;
        }

        private async void fatura_onclick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FaturaGecmisi());
        }

        private async void kampanya_onclick(object sender, EventArgs e)
        {
            //Calısıyor Dokunma!!!!!!!!!!!!
            var action = await DisplayAlert("Kampanya Bildirimleri", "Kampanyalardan anında haberdar olmak ister misiniz?", "Evet", "Hayır");

            if (action)
            {
                //Bildirimleri ac
                AppCenter.Start("d8fdf080-0177-4c28-abbe-7a9f472bb6c0", typeof(Push));
                await Push.SetEnabledAsync(true);
                await DisplayAlert("Bildirim", "Tebrikler! Bundan sonra kampanyalarımızdan anında haberdar olabileceksiniz.", "Tamam");

            }
            else
            {
                //Bildirimleri kapat
                Push.SetEnabledAsync(false);
                await DisplayAlert("Bildirim", "Bundan sonra sizleri bildirimlerimiz ile rahatsız etmeyeceğiz. SÖZ :) ",
                    "Teşekkürlerr");
            }

        }

        private async void biz_onclick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BizeUlasin());
        }

        private async void cikis_onclick(object sender, EventArgs e)
        {
            await cikisyap();
        }

        public async Task cikisyap()
        {
            var action = await DisplayAlert("Çıkış Yap", "Çıkış yapmak istediğinizden emin misiniz?", "Çıkış Yap", "İptal");
            if (action)
            {
                Application.Current.Properties.Remove("UserMail");
                Application.Current.Properties.Remove("Username");

                if (Application.Current.Properties.ContainsKey("CartId"))
                {
                    await _client.DeleteAsync(url_delete + "/" + Application.Current.Properties["CartId"].ToString());
                    Application.Current.Properties.Remove("CartId");
                }
                await Navigation.PopToRootAsync();
            }
            else
            {
            }
        }
        //Geri butonuna basılmıyor.
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }

    }
}