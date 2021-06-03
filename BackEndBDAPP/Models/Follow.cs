using System;
using System.Collections.Generic;

#nullable disable

namespace BackEndBDAPP.Models
{
    public partial class Follow
    {
        public string IdFollower { get; set; }
        public string IdFollowed { get; set; }
        public long IdFollow { get; set; }

        public virtual User IdFollowedNavigation { get; set; }
        public virtual User IdFollowerNavigation { get; set; }
    }
}
