using System;
using System.Collections.Generic;

#nullable disable

namespace BackEndBDAPP.Models
{
    public partial class User
    {
        public User()
        {
            ComfirmCodes = new HashSet<ComfirmCode>();
            Comments = new HashSet<Comment>();
            FollowIdFollowedNavigations = new HashSet<Follow>();
            FollowIdFollowerNavigations = new HashSet<Follow>();
            Likes = new HashSet<Like>();
            Posts = new HashSet<Post>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }
        public byte[] Salt { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireTime { get; set; }
        public bool? EmailConfirm { get; set; }

        public virtual ICollection<ComfirmCode> ComfirmCodes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Follow> FollowIdFollowedNavigations { get; set; }
        public virtual ICollection<Follow> FollowIdFollowerNavigations { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
