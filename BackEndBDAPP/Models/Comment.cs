using System;
using System.Collections.Generic;

#nullable disable

namespace BackEndBDAPP.Models
{
    public partial class Comment
    {
        public string CommentText { get; set; }
        public string IdUser { get; set; }
        public long IdPost { get; set; }
        public long IdComment { get; set; }

        public virtual Post IdPostNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
