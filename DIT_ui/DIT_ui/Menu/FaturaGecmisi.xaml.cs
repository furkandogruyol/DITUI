using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DIT_ui.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace DIT_ui.Menu
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FaturaGecmisi : ContentPage
	{
        HttpClient _client = new HttpClient();
        public string url= "http://ditwebapp.azurewebsites.net/api/ShoppingCart/GetShoppingList/";
	    public ObservableCollection<KullanıcıSepeti> Sepet;

        public FaturaGecmisi ()
		{
			InitializeComponent ();
		    GetList(Application.Current.Properties["UserId"].ToString());

		}

	    private async void Lv_fatura_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
	    {
	     
	        await Navigation.PushAsync(new FaturaDetay(e.SelectedItem));
            
        }

	    public async Task GetList(string uid)
	    {
	        try
	        {
	            var content = await _client.GetStringAsync(url + uid);
	            var list = JsonConvert.DeserializeObject<List<KullanıcıSepeti>>(content);
	            Sepet = new ObservableCollection<KullanıcıSepeti>(list);
	            lv_fatura.ItemsSource = Sepet;
	        }
	        catch (Exception e)
	        {
	            await DisplayAlert("Hata", "İnternet bağlantını kontrol edip uygulamaya bir daha giriş yap!", "Tamam");
	        }

	    }
    }
}