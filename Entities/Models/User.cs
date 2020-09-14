using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class User
    {
        public string UserName { get; set; }
        public int RealEstates { get; set; }
        public int Comments { get; set; }
        public double Rating { get; set; }
        public string AvatarImageUrl { get; set; }
    }
}
