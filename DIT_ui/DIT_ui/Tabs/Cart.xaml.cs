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
using ZXing.Net.Mobile.Forms;

namespace DIT_ui.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cart : ContentPage
    {
        string url = "http://ditwebapp.azurewebsites.net/api/ShoppingCart/GetShoppingCartWithCartId";
        string url_price = "http://ditwebapp.azurewebsites.net/api/ShoppingCart/GetTotalPriceWithCartId";
        string url_delete = "http://ditwebapp.azurewebsites.net/api/ShoppingCart/DeleteProductWithShoppingCartId";
        public HttpClient _client = new HttpClient();
        public ObservableCollection<KullanıcıSepeti> SepetListe;
        public ObservableCollection<TutarObje> _prices;

        public Cart()
        {
            InitializeComponent();

            if (Application.Current.Properties.ContainsKey("CartId"))
            {
                GetList(Application.Current.Properties["CartId"].ToString());
                btn_ok.IsEnabled = true;
                btn_addCart.IsEnabled = true;
                GetPrice(Application.Current.Properties["CartId"].ToString());

            }
            else
            {
                lb_tutar.Text = "Sepetinizde ürün bulunmuyor.";
                lv_ürün.ItemsSource = null;
                btn_ok.IsEnabled = false;
                btn_addCart.IsEnabled = false;
            }

            lv_ürün.RefreshCommand = new Command(async () =>
            {
                if (Application.Current.Properties.ContainsKey("CartId"))
                {
                    await GetList(Application.Current.Properties["CartId"].ToString());
                    await GetPrice(Application.Current.Properties["CartId"].ToString());
                    lv_ürün.IsRefreshing = false;
                }
                else
                {
                    lb_tutar.Text = "Sepetinizde ürün bulunmuyor.";
                    lv_ürün.ItemsSource = null;
                    btn_ok.IsEnabled = false;
                    btn_addCart.IsEnabled = false;
                    lv_ürün.IsRefreshing = false;
                }

            });

          
        }
        
      
        

        private async void Btn_ok_OnClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Alışverişi Tamamla", "Alışverişinizi tamamlamak üzeresiniz. Tamam derseniz geri dönemeyeceksiniz!", "Tamam","Geri Dön");
            if (result)
            {
                await Navigation.PushAsync(new CartDone());
            }
            
        }

        private async void Btn_addCart_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BarcodeScanner());
        }


        private async void Btn_remove_OnClicked(object sender, EventArgs e)
        {
            var shoppingId = int.Parse((sender as Button).CommandParameter.ToString());
            await ürünüSil(shoppingId);
        }


        private void Btn_update_OnClicked(object sender, EventArgs e)
        {
            this.DisplayAlert("Güncelle", "Miktar Giriniz", "Güncelle");
        }

        public async Task GetList(string cId)
        {
            try
            {
                var content = await _client.GetStringAsync(url + "/" + cId);
                var list = JsonConvert.DeserializeObject<List<KullanıcıSepeti>>(content);
                SepetListe = new ObservableCollection<KullanıcıSepeti>(list);
                lv_ürün.ItemsSource = SepetListe;
            }
            catch (Exception e)
            {
                await DisplayAlert("Hata", "İnternet bağlantını kontrol edip uygulamaya bir daha giriş yap!", "Tamam");
            }

        }

      
        IEnumerable<KullanıcıSepeti> GetCart(string search = null)
        {

            if (String.IsNullOrWhiteSpace(search))
            {
                return SepetListe;
            }
            return SepetListe.Where(k => k.productName.ToString().Contains(search));
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            lv_ürün.ItemsSource = GetCart(e.NewTextValue);
        }

        public async Task refreshListAsync()
        {
            try
            {
                var content = await _client.GetStringAsync(url + "/" + Application.Current.Properties["CartId"]);
                var list = JsonConvert.DeserializeObject<List<KullanıcıSepeti>>(content);
                SepetListe = new ObservableCollection<KullanıcıSepeti>(list);
                lv_ürün.ItemsSource = SepetListe;
            }
            catch (Exception e)
            {
                await DisplayAlert("Hata", "İnternet bağlantını kontrol edip uygulamaya bir daha giriş yap!", "Tamam");
            }
        }

       
        public async Task GetPrice(string cıd)
        {
            var content = await _client.GetStringAsync(url_price + "/" + cıd);
            var price = JsonConvert.DeserializeObject<List<TutarObje>>(content);
            _prices = new ObservableCollection<TutarObje>(price);
            TutarObje tutar = _prices.First();
            lb_tutar.BindingContext = tutar.doubleObject;

        }


        public async Task ürünüSil(int sId)
        {
            var action = await DisplayAlert("Sil", " Ürünü listenizden kaldırmak istediğinizden emin misiniz?", "Kaldır", "İptal");
            if (action)
            {
                await _client.DeleteAsync(url_delete + "/" +sId);
                

                if (Application.Current.Properties.ContainsKey("CartId"))
                {
                    await refreshListAsync();
                }
                else
                {
                    lb_tutar.Text = "Boş";
                    lv_ürün.ItemsSource = null;
                }
            }
            else
            {

            }
        }


        private async void Lv_ürün_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as KullanıcıSepeti;
            Application.Current.Properties["ShoppingCartId"] = item.shoppingCartId.ToString();
            Application.Current.Properties["ShoppingDate"] = item.shoppingDate.ToString();
            await Navigation.PushAsync(new ShoppingDetail(item));

        }

      

    }
}