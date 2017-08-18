using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarians.GoogleLogin.Interface;
using Xamarin.Forms;

namespace Sample
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var result = await DependencyService.Get<IGoogleLogin>().SignIn();
            if(result.IsSuccess)
            {
                imgProfile.Source = result.Image;
                var name = result.Name;
                await DisplayAlert("", "Account Name -" + name, "Ok");
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            var result = await DependencyService.Get<IGoogleLogin>().SignOut();
            if(result.IsSuccess)
            {
                imgProfile.Source = "";
                await DisplayAlert("Success", "Successfully Logout", "Ok");
            }
        }

    }
}
