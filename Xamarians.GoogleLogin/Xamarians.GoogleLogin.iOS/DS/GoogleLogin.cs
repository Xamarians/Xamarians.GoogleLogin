using CoreGraphics;
using Foundation;
using Google.SignIn;
using UIKit;
using Xamarians.GoogleLogin.Interface;
using Xamarians.GoogleLogin.iOS.DS;
using Xamarians.GoogleLogin.Platforms;
using Xamarin.Forms;

[assembly: Dependency(typeof(GoogleLogin))]
namespace Xamarians.GoogleLogin.iOS.DS
{
	public class GoogleLogin : UIViewController, IGoogleLogin, ISignInDelegate, ISignInUIDelegate
	{
		public new static void Init()
		{

		}

		public static UIViewController GetController()
		{
			var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
			while (vc.PresentedViewController != null)
				vc = vc.PresentedViewController;
			return vc;
		}

		public GoogleLogin()
		{

			Google.SignIn.SignIn.SharedInstance.UIDelegate = this;
			Google.SignIn.SignIn.SharedInstance.Delegate = this;
		}


		public void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
		{
			if (user != null && error == null)
			{
				uint size = 50;
				var profile = new GoogleLoginResult();
				profile.Name = user.Profile.Name;
				profile.Email = user.Profile.Email;
				profile.UserId = user.UserID;
				profile.IsSuccess = true;
				profile.Image = user.Profile.GetImageUrl(new System.nuint(size)).AbsoluteString;
				_tcs.SetResult(profile);
			}
		}


		System.Threading.Tasks.TaskCompletionSource<GoogleLoginResult> _tcs;
		public System.Threading.Tasks.Task<GoogleLoginResult> SignIn()
		{
			_tcs = new System.Threading.Tasks.TaskCompletionSource<GoogleLoginResult>();
			Google.SignIn.SignIn.SharedInstance.SignInUser();
			return _tcs.Task;
		}

		[Export("signInWillDispatch:error:")]
		public void WillDispatch(SignIn signIn, NSError error)
		{
			//myActivityIndicator.StopAnimating();
		}

		[Export("signIn:presentViewController:")]
		public void PresentViewController(SignIn signIn, UIViewController viewController)
		{
			GetController().PresentViewController(viewController, true, null);
		}

		[Export("signIn:dismissViewController:")]
		public void DismissViewController(SignIn signIn, UIViewController viewController)
		{
			GetController().DismissViewController(true, null);
		}

	}
}
