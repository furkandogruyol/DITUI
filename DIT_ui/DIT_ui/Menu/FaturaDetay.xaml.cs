using DIT_ui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DIT_ui.Menu
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FaturaDetay : ContentPage
	{
	    KullanıcıSepeti smail = new KullanıcıSepeti();
		public FaturaDetay (object ft)
		{
			InitializeComponent ();

		    ft_tarih.Text += (ft as KullanıcıSepeti).shoppingDate.ToString();
		    ft_aciklama.Text += (ft as KullanıcıSepeti).productName.ToString();
		    ft_tutar.Text += (ft as KullanıcıSepeti).productPrice.ToString();
            smail=ft as KullanıcıSepeti;
		}


	    private async void Btn_mail_OnClicked(object sender, EventArgs e)
	    {
           try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("tfapphub@gmail.com");

                /************** Bu kısım kullanıcıdan alınacak *****************/
                string m = Application.Current.Properties["UserMail"].ToString();
                mail.To.Add(m);
                mail.Subject = "DIT Fatura Geçmişi";

                /**************** Bu kısımda faturanın içeriği olacak **********/
                mail.Body = smail.shoppingDate.ToString()+" Tarihli "+smail.productPrice.ToString()+" tutarındaki alışverişinizin ayrıntıları; "+smail.productName.ToString()+" "+smail.productAmount+" adet";
                

                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("tfapphub@gmail.com", "selamcukulatam");

                SmtpServer.Send(mail);
                await DisplayAlert("OK", "Mailinize alışveriş özetinizi yolladık.Bir sıkıntı olursa bizle iletişime geçmekten çekinmeyin. İyi günler.", "Tamam");


            }
            catch (Exception ex)
            {
               await DisplayAlert("Ooops", "Malesef şuanda bu servisimiz kullanıma kapalıdır. Lütfen daha sonra tekrar deneyiniz.", "Tamam :(");
            }
        }
    }
}