using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarians.GoogleLogin.Platforms;

namespace Xamarians.GoogleLogin.Interface
{
    public interface IGoogleLogin
    {
        Task<GoogleLoginResult> SignIn();
        Task<GoogleLoginResult> SignOut();
    }
}
