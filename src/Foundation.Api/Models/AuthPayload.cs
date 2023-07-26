using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Api.Models
{
    public class AuthPayload
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}