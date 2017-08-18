using Android.Content;
using Xamarin.Forms;
using Xamarians.GoogleLogin.Droid.DS;
using Xamarians.GoogleLogin.Interface;
using System.Threading.Tasks;
using Xamarians.GoogleLogin.Platforms;
using Xamarians.GoogleLogin.Droid.Platforms;
using System;

[assembly: Dependency(typeof(GoogleLogin))]
namespace Xamarians.GoogleLogin.Droid.DS
{
    class GoogleLogin : IGoogleLogin
    {
        public Task<GoogleLoginResult> SignIn()
        {
            var tcs = new TaskCompletionSource<GoogleLoginResult>();
            GoogleLoginActivity.OnLoginCompleted(tcs);
            var googleIntent = new Intent(Forms.Context, typeof(GoogleLoginActivity));
            Xamarin.Forms.Forms.Context.StartActivity(googleIntent);
            return tcs.Task;
        }

        public Task<GoogleLoginResult> SignOut()
        {
            var tcs = new TaskCompletionSource<GoogleLoginResult>();
            GoogleLoginActivity.OnLogOutClicked(tcs);
            GoogleLoginActivity.SignOut();
            return tcs.Task;
        }
    }
}