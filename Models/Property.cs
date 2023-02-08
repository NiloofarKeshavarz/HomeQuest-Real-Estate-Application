using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeQuest.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }
        // TODO: Add sellerId
        [Required(ErrorMessage = "Required")]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Required")]
        public string PostalCode { get; set; }

        // [Required(ErrorMessage = "Required")]
        // [StringLength(50)]
        //public CityNames City { get; set; }

        [Required(ErrorMessage = "Required")]
        public decimal? Price { get; set; }

       
        public byte? Floors { get; set; }
        [Required(ErrorMessage = "Required")]
        public byte? BedroomCount { get; set; }
        [Required(ErrorMessage = "Required")]
        public byte? BathroomCount { get; set; }

        public byte? GarageCont { get; set; }

        [Required(ErrorMessage = "Required")]
        public DateTime YearBuilt { get; set; }

        [Required(ErrorMessage = "Required")]
        public decimal? FloorArea { get; set; }

        [Required(ErrorMessage = "Required")]
        public decimal? LotArea { get; set; }

        public DateTime CreatedAt { get; set; }

        public enum PropertyStatus
        {
            Pending = 0, Sold = 1, ForSale = 2
        }

        // public enum CityNames
        // {
        //     Montreal = 0, Laval = 1, Longueuil = 2 , Dorval = 3, CôteSaintLuc = 4, Brossard = 5,
        //     Boucherville = 6, Candiac = 7, DollardDesOrmeaux = 8, LaPrairie = 9, Kirkland =10
        // }

        [Required(ErrorMessage = "Required")]
        public PropertyStatus Status { get; set; }

        public enum PropertyType
        {
            House = 0, Apartment = 1, Duplex = 2, TownHouse = 3, Condor = 4
        }
        [Required(ErrorMessage = "Required")]
        public PropertyType Type { get; set; }


        public ICollection<Image> Images { get; set; }

        public ICollection<Offer> Offers { get; set; }
        public IList<Favorite> Favorites { get; set; }



    }
}