using System;
using System.Collections.Generic;

#nullable disable

namespace BackEndBDAPP.Models
{
    public partial class Like
    {
        public string IdUser { get; set; }
        public long IdPost { get; set; }
        public long IdLike { get; set; }

        public virtual Post IdPostNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
