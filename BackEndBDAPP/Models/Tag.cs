using System;
using System.Collections.Generic;

#nullable disable

namespace BackEndBDAPP.Models
{
    public partial class Tag
    {
        public Tag()
        {
            Tag1s = new HashSet<Tag1>();
        }

        public string TagName { get; set; }

        public virtual ICollection<Tag1> Tag1s { get; set; }
    }
}
