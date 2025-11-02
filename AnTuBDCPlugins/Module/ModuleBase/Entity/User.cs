using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleBase.Entity
{
    public class User
    {
        public String Account { get; set; }
        public String Token { get; set; }
        public String Password { get; set; }
        public String PasswordEncode { get; set; }
        public String TenantID { get; set; }
        public String SystemCode { get; set; }
        public String WithPhoneVerify { get; set; }
    }
}
