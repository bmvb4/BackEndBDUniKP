using System;
using System.Collections.Generic;

#nullable disable

namespace BackEndBDAPP.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
            Tag1s = new HashSet<Tag1>();
        }

        public long IdPost { get; set; }
        public byte[] Photo { get; set; }
        public string IdUser { get; set; }
        public DateTime? DeleteDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Description { get; set; }

        public virtual User IdUserNavigation { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Tag1> Tag1s { get; set; }
    }
}
