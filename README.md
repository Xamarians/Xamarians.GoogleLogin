# Xamarians.GoogleLogin

Cross platform library to allow users to login through google account in the app and also retrieves the profile for that particular user.

First install package from nuget using following command -
## Install-Package Xamarians.GoogleLogin

You can integrate GoogleLogin in Xamarin Form application using following code:

 Shared Code -

Add this to call the Google login service
```c#
...
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var result = await DependencyService.Get<IGoogleLogin>().SignIn();
            if(result.IsSuccess)
            {
                imgProfile.Source = result.Image;
                var name = result.Name;
                await DisplayAlert("", "Account Name -" + name, "Ok");
            }
        }
```	    
iOS - in AppDelegate file write below code -
```c#
 Xamarians.GoogleLogin.iOS.DS.GoogleLogin.Init();
```
Note
```
Make sure your android package name has access to google api's.
follow this link for Android https://console.developers.google.com

For iOS configuration please follow steps given in the below link.
https://components.xamarin.com/gettingstarted/googleiossignin

```
Add the following permissions in Android Manifest file
```
  <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
	<uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
	<uses-permission android:name="android.permission.INTERNET" />
  <uses-permission android:name="android.permission.GET_ACCOUNTS"/>
```

