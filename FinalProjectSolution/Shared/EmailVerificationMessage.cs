using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared
{
    public class EmailVerificationMessage
    {
        public string Email { get; set; } = "";
        public string Token { get; set; } = "";
    }
}