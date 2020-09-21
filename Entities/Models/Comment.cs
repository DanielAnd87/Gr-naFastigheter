using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Comment
    {
        public int RealEstateId { get; set; }
        [Required]
        public string Content { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }
        //public User User { get; set; } = new User();
        public Comment(string content)
        {
            Content = content;
            CreatedOn = DateTime.Now;
        }

        public Comment(int realEstateId, string content, string userName)
        {
            RealEstateId = realEstateId;
            Content = content;
            UserName = userName;
            CreatedOn = DateTime.Now;
        }
        public Comment()
        {

        }
    }
}