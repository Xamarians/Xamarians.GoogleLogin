using System;
using Android.App;
using Android.Content;
using Android.OS;
using Xamarians.GoogleLogin.Platforms;
using Android.Gms.Common.Apis;
using Android.Support.V7.App;
using Android.Gms.Plus;
using Android.Gms.Common;
using System.Threading.Tasks;

namespace Xamarians.GoogleLogin.Droid.Platforms
{
    [Activity(Label = "GoogleLogin", Theme = "@style/Theme.AppCompat.Light.DarkActionBar")]
    public class GoogleLoginActivity : AppCompatActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        const string TAG = "MainActivity";

        const int RC_SIGN_IN = 9001;

        const string KEY_IS_RESOLVING = "is_resolving";
        const string KEY_SHOULD_RESOLVE = "should_resolve";


        GoogleApiClient mGoogleApiClient;

        bool mIsResolving = false;

        bool mShouldResolve = false;

        static TaskCompletionSource<GoogleLoginResult> _tcs;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mGoogleApiClient = new GoogleApiClient.Builder(this)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .AddApi(PlusClass.API)
                .AddScope(new Scope(Scopes.PlusMe))
                .AddScope(new Scope(Scopes.Email))
                .AddScope(new Scope(Scopes.Profile))
                .AddScope(new Scope(Scopes.PlusLogin))
                .Build();
        }

        private void HandleResult(GoogleLoginResult result)
        {
            Finish();
            if (_tcs != null)
                _tcs.SetResult(result);
        }

        private async void UpdateUI(bool isSignedIn)
        {
            if (isSignedIn)
            {
                var email = string.Empty;
                try
                {
                   email = PlusClass.AccountApi.GetAccountName(mGoogleApiClient);
                }
                catch(Exception ex)
                {

                }
                var person = PlusClass.PeopleApi.GetCurrentPerson(mGoogleApiClient);
                var name = string.Empty;
                var gender = string.Empty;
                var userId = string.Empty;
                var image = string.Empty;
                if (person != null)
                {
                    name = person.DisplayName;
                    userId = person.Id;
                    if (person.Gender == 0)
                        gender = "male";
                    else
                        gender = "female";
                    image = person.Image.Url;
                    
                }
                HandleResult(new GoogleLoginResult { IsSuccess = true, Name = name, UserId = userId, Image = image, Email = email });
            }
            else
            {
                await System.Threading.Tasks.Task.Delay(2000);
                mShouldResolve = true;
                mGoogleApiClient.Connect();
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            mGoogleApiClient.Connect();
        }

        protected override void OnStop()
        {
            base.OnStop();
            mGoogleApiClient.Disconnect();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutBoolean(KEY_IS_RESOLVING, mIsResolving);
            outState.PutBoolean(KEY_SHOULD_RESOLVE, mIsResolving);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == RC_SIGN_IN)
            {
                if (resultCode != Result.Ok)
                {
                    mShouldResolve = false;
                }
                mIsResolving = false;
                mGoogleApiClient.Connect();
            }
        }

        public void OnConnected(Bundle connectionHint)
        {
            UpdateUI(true);
        }

        public void OnConnectionSuspended(int cause)
        {

        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            if (!mIsResolving && mShouldResolve)
            {
                if (result.HasResolution)
                {
                    try
                    {
                        result.StartResolutionForResult(this, RC_SIGN_IN);
                        mIsResolving = true;
                    }
                    catch (IntentSender.SendIntentException e)
                    {
                        mIsResolving = false;
                        mGoogleApiClient.Connect();
                    }
                }
                else
                {
                    ShowErrorDialog(result);
                }
            }
            else
            {
                UpdateUI(false);
            }
        }

        class DialogInterfaceOnCancelListener : Java.Lang.Object, IDialogInterfaceOnCancelListener
        {
            public Action<IDialogInterface> OnCancelImpl { get; set; }

            public void OnCancel(IDialogInterface dialog)
            {
                OnCancelImpl(dialog);
            }
        }

        void ShowErrorDialog(ConnectionResult connectionResult)
        {
            int errorCode = connectionResult.ErrorCode;

            if (GooglePlayServicesUtil.IsUserRecoverableError(errorCode))
            {
                var listener = new DialogInterfaceOnCancelListener();
                listener.OnCancelImpl = (dialog) =>
                {
                    mShouldResolve = false;
                };
                GooglePlayServicesUtil.GetErrorDialog(errorCode, this, RC_SIGN_IN, listener).Show();
            }
            else
            {
                mShouldResolve = false;
            }
            HandleResult(new GoogleLoginResult { IsSuccess = false, Message = connectionResult.ErrorMessage });
        }


        public void SignOut()
        {
            if (mGoogleApiClient.IsConnected)
            {
                PlusClass.AccountApi.ClearDefaultAccount(mGoogleApiClient);
                mGoogleApiClient.Disconnect();
            }
        }

        public static void OnLoginCompleted(TaskCompletionSource<GoogleLoginResult> tcs)
        {
            _tcs = tcs;
        }

    }
}