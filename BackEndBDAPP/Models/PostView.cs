using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndBDAPP.Models
{
    public class PostView
    {
        public byte[] Photo { get; set; }
        public string IdUser { get; set; }
        public string Description { get; set; }
        public List<string> tags { get; set; }
    }
}
