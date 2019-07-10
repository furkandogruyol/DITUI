using DIT_ui.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DIT_ui.Auth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgetPass : ContentPage
    {

        string url = "http://ditwebapp.azurewebsites.net/api/Users/GetUser";
        public HttpClient _client = new HttpClient();
        private ObservableCollection<User> _users;

        public ForgetPass()
        {
            InitializeComponent();
        }

        private async void Btn_pass_OnClicked(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tb_mail.Text) || String.IsNullOrWhiteSpace(tb_phone.Text))
            {
                await DisplayAlert("Hata","Doldurulması zorunlu alanları doldurup bir daha deneyin.","Tamam");
            }
            else
            {
                //ac_mail.IsRunning = true;
                //ac_mail.IsEnabled = true;
                //ac_mail.IsVisible = true;

                tb_mail.IsEnabled = false;
                tb_phone.IsEnabled = false;
                btn_pass.IsEnabled = false;

                var content = await _client.GetStringAsync(url);
                var users = JsonConvert.DeserializeObject<List<User>>(content);
                _users = new ObservableCollection<User>(users);

                var pass = _users.Where(u => u.userEmail == tb_mail.Text).First().userPassword.ToString();
                if (_users.Where(u => u.userEmail == tb_mail.Text).Any())
                {
                    var userPhone = _users.Where(u => u.userEmail == tb_mail.Text).First().userPhone.ToString();


                    if (userPhone == tb_phone.Text)
                    {
                        try
                        {
                            MailMessage mail = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                            mail.From = new MailAddress("tfapphub@gmail.com");


                            mail.To.Add(tb_mail.Text);
                            mail.Subject = "Şifremi Unuttum";


                            mail.Body = tb_mail.Text + " mail adresine kayıtlı kullanıcının şifresi: " + pass;

                            SmtpServer.Port = 587;
                            SmtpServer.Host = "smtp.gmail.com";
                            SmtpServer.EnableSsl = true;
                            SmtpServer.UseDefaultCredentials = false;
                            SmtpServer.Credentials = new System.Net.NetworkCredential("tfapphub@gmail.com", "selamcukulatam");

                            SmtpServer.Send(mail);

                          
                            tb_mail.IsEnabled = true;
                            tb_phone.IsEnabled = true;
                            btn_pass.IsEnabled = true;
                            //ac_mail.IsRunning = false;
                            //ac_mail.IsEnabled = false;
                            //ac_mail.IsVisible = false;

                            await DisplayAlert("OK", "Mailinize şifrenizi yolladık kontrol edip giriş yapabilirsiniz. Keyifli alışverişler dileriz.", "Tamam");

                        }
                        catch (Exception ex)
                        {
                          await  DisplayAlert("Ooops", "Malesef şuanda bu servisimiz kullanıma kapalıdır. Lütfen daha sonra tekrar deneyiniz.", "Tamam :(");
                        
                            tb_mail.IsEnabled = true;
                            tb_phone.IsEnabled = true;
                            btn_pass.IsEnabled = true;
                            //ac_mail.IsRunning = false;
                            //ac_mail.IsEnabled = false;
                            //ac_mail.IsVisible = false;

                        }
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Verilen telefon numarası sistemde kayıtlı mail adresi ile eşleşmiyor!",
                            "Tamam");
                       
                        tb_mail.IsEnabled = true;
                        tb_phone.IsEnabled = true;
                        btn_pass.IsEnabled = true;
                        //ac_mail.IsRunning = false;
                        //ac_mail.IsEnabled = false;
                        //ac_mail.IsVisible = false;

                    }


                }
                else
                {
                    await DisplayAlert("Hata", "Bu mail adresi sistemimizde kayıtlı değil. Lütfen sistemde kayıtlı olan mail adresinizi giriniz.", "Tamam");
                    
                    tb_mail.IsEnabled = true;
                    tb_phone.IsEnabled = true;
                    btn_pass.IsEnabled = true;
                    //ac_mail.IsRunning = false;
                    //ac_mail.IsEnabled = false;
                    //ac_mail.IsVisible = false;

                }

            }
        }


    }
}