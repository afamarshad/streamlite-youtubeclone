using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace youtubeclone.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string email { get; set; }
        public bool emailConfirmed { get; set; }
        public string password { get; set; }
        public string security { get; set; }
        public string phonenumber { get; set; }
        public string phonenumberconfirmed { get; set; }
    }
}