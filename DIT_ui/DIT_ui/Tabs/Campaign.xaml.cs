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
	public partial class Campaign : ContentPage
	{
	    string url = "http://ditwebapp.azurewebsites.net/api/Campaign/GetCampaign";
	    public HttpClient _client = new HttpClient();
	    public ObservableCollection<KampanyaModel> KampanyaListt;


        public Campaign ()
		{
			InitializeComponent ();
            GetList();
		    lv_kampanya.RefreshCommand = new Command(() =>
		    {
		        GetList();
		        lv_kampanya.IsRefreshing = false;
		    });
        }

	   public  async void GetList()
	    {
	        try
	        {
	            var content = await _client.GetStringAsync(url);
	            var camps = JsonConvert.DeserializeObject<List<KampanyaModel>>(content);
	            KampanyaListt = new ObservableCollection<KampanyaModel>(camps);
	            lv_kampanya.ItemsSource = KampanyaListt;
            }
	        catch (Exception e)
	        {
	            await DisplayAlert("Hata", "İnternet bağlantını kontrol edip uygulamaya bir daha giriş yap!", "Tamam");
	        }
	    
	    }

        //GetKampanyalar yap  ordan verileri cek
	    IEnumerable<KampanyaModel> GetKampanyalar(string search = null)
	    {
	        //Api den verileri cekip liste haline getir sonrada burada göster.
	       
	        if (String.IsNullOrWhiteSpace(search))
	        {
	            return KampanyaListt;
	        }

	        return KampanyaListt.Where(k => k.productName.Contains(search));
	    }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            lv_kampanya.ItemsSource = GetKampanyalar(e.NewTextValue);
            
        }

       

    }
}