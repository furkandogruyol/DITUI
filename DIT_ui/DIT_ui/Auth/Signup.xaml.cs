using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DIT_ui.Models;
using DIT_ui.Tabs;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DIT_ui.Auth
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Signup : ContentPage
	{
	    string url = "http://ditwebapp.azurewebsites.net//api/Users/PostUser";

        public HttpClient _client = new HttpClient();
	    private ObservableCollection<User> _users;
	    private User us;

        private string emailReg =
	        "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&\'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$";

        public Signup ()
		{
			InitializeComponent ();
		}

	    private async void kaydol_onclick(object sender, EventArgs e)
	    {
	        btn_sign.IsEnabled = false;
            if (String.IsNullOrWhiteSpace(tb_mail.Text)|| String.IsNullOrWhiteSpace(tb_pw.Text) || String.IsNullOrWhiteSpace(tb_adr.Text) || String.IsNullOrWhiteSpace(tb_tel.Text) || String.IsNullOrWhiteSpace(tb_user.Text))
            {
                await DisplayAlert("Hata", "Doldurulması gereken alanları doldurup bir daha deneyiniz.", "Tamam");
                btn_sign.IsEnabled = true;
            }
            else
            {
                await tamam_onclick();
                btn_sign.IsEnabled = true;
            }
	      
	    }

	    public async Task tamam_onclick()
	    {
            //Numarayı stringten int e cevir
	        int phone = 0;
	        Int32.TryParse(tb_tel.Text, out phone);
            //UserId kısmını readonly yapacagız.
	        Random rnd = new Random();
	        int uid = rnd.Next(1, 99999);
	        us = new User()
	        {
	            userEmail = tb_mail.Text,
	            userPassword = tb_pw.Text,
	            userName = tb_user.Text,
	            userAddress = tb_adr.Text,
	            userPhone = phone,
	            userId = uid
	        };

	        try
	        {
	            var content = JsonConvert.SerializeObject(us);
                //Değiştirdim
	            await _client.PostAsync(url, new StringContent(content, Encoding.UTF8,"application/json"));
                
	            Application.Current.Properties["UserMail"] = tb_mail.Text;
	            Application.Current.Properties["Username"] = tb_user.Text;
	            
                await Application.Current.SavePropertiesAsync();
                
                await Navigation.PushAsync(new MainPage());
	            btn_sign.IsEnabled = true;
	        }
	        catch (Exception e)
	        {
	            await DisplayAlert("Ooops","İnternete bağlı oluğ olmadığınızı kontrol edip tekrar deneyin","Tamam");
	            btn_sign.IsEnabled = true;
	        }
	       
            
	    }

	    private void Tb_mail_OnCompleted(object sender, EventArgs e)
	    {
            //email uygun formatta değil ise butona bastırma
	        if (!Regex.IsMatch(tb_mail.Text, emailReg))
	        {
	            btn_sign.IsEnabled = false;
	        }
            else
            {
                btn_sign.IsEnabled = true;
            }
        }
	}
}