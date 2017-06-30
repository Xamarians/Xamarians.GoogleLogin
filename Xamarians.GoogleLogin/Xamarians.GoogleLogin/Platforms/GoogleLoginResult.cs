using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarians.GoogleLogin.Platforms
{
    public class GoogleLoginResult
    {
        public bool IsSuccess { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
