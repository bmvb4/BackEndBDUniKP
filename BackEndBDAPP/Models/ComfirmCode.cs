using System;
using System.Collections.Generic;

#nullable disable

namespace BackEndBDAPP.Models
{
    public partial class ComfirmCode
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Code { get; set; }
        public DateTime DeleteDate { get; set; }

        public virtual User UsernameNavigation { get; set; }
    }
}
