using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class RealEstate
    {
        public string ImageUrl { get; set; }
        public string Contact { get; set; }
        public List<Comment> Comments { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime CreatedOn { get; set; }
        [Required]
        [Range(1600, 2020)]
        public int ConstructionYear { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string RealEstateType { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]

        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
        [Required]

        public int SellingPrice { get; set; }
        [Required]
        public int RentingPrice { get; set; }
        public bool CanBeSold { get; set; }
        public bool CanBeRented { get; set; }
    }
}
