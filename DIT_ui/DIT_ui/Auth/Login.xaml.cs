using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DIT_ui.Models;
using DIT_ui.Tabs;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DIT_ui.Auth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        string url = "http://ditwebapp.azurewebsites.net/api/Users/GetUser";
        public HttpClient _client = new HttpClient();
        private ObservableCollection<User> _users;

      
        public Login()
        {
            InitializeComponent();
          
        }


        private async void Btn_login_OnClicked(object sender, EventArgs e)
        {
            btn_login.IsEnabled = false;
            if (String.IsNullOrWhiteSpace(tb_mail.Text)|| String.IsNullOrWhiteSpace(tb_pw.Text))
            {
                await DisplayAlert("Hata", "Doldurulması gereken alanları doldurup bir daha deneyin.", "Tamam");
                btn_login.IsEnabled = true;
            }
            else
            {
                try
                {
                    var content = await _client.GetStringAsync(url);
                    var users = JsonConvert.DeserializeObject<List<User>>(content);
                    _users = new ObservableCollection<User>(users);
                    var userT = tb_mail.Text.ToString().Trim();
                    if (_users.Where(u=> u.userEmail == userT).Any() == false)
                    {
                        await DisplayAlert("Hata", "Bu mail adresi sistemimizde kayıtlı görünmüyor lütfen tekrar deneyin.", "Tamam");
                        btn_login.IsEnabled = true;
                        tb_mail.Text = "";
                        tb_pw.Text = "";
                    }
                    else
                    {
                        string pass = _users.Where(u => u.userEmail == userT).First().userPassword.ToString();

                        if (pass == tb_pw.Text)
                        {
                            Application.Current.Properties["UserMail"] = _users.First(u => u.userEmail == userT).userEmail;
                            Application.Current.Properties["Username"] = _users.First(u => u.userEmail == userT).userName;
                            Application.Current.Properties["UserId"] = _users.First(u => u.userEmail == userT).userId;
                            
                            await Application.Current.SavePropertiesAsync();

                            await Navigation.PushAsync(new MainPage());
                        }
                        else
                        {
                            await DisplayAlert("Hata", "Şifreniz hatalı görünüyor lütfen tekrar deneyin.", "Tamam");
                            btn_login.IsEnabled = true;
                            tb_pw.Text = "";
                        }
                    }
                   
                }
                catch (Exception ex)
                {
                    await DisplayAlert(ex.Message, "İnternetiniz mi kapalı kontrol edip tekrar deneyin.", "Tamam");
                    btn_login.IsEnabled = true;
                }
               
            }

        }

        private async void Btn_kayit_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Signup());
        }

        private async void Btn_sifre_OnClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new ForgetPass());
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }

    }
}