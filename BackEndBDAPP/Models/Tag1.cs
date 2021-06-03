using System;
using System.Collections.Generic;

#nullable disable

namespace BackEndBDAPP.Models
{
    public partial class Tag1
    {
        public long IdPost { get; set; }
        public string IdTag { get; set; }
        public long IdTagsConnrction { get; set; }

        public virtual Post IdPostNavigation { get; set; }
        public virtual Tag IdTagNavigation { get; set; }
    }
}
