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
       [Required(ErrorMessage = "Field is required.")] 
       public string Title { get; set; }
       [Required(ErrorMessage = "Field is required.")] 
       public string Description { get; set; }
       [Required(ErrorMessage = "Field is required.")] 
       public string Address { get; set; }
       [Required(ErrorMessage = "Field is required.")] 
       public string PostalCode { get; set; }

       [Required(ErrorMessage = "Field is required.")] 
       public int Price { get; set;}

       public int Floors { get; set; }
       [Required(ErrorMessage = "Field is required.")] 
       public int BedroomCount { get; set; }
       [Required(ErrorMessage = "Field is required.")] 
       public int BathroomCount { get; set; }
       public int GarageCont { get; set; }
       [Required(ErrorMessage = "Field is required.")] 
       public DateTime YearBuilt { get; set; }
       
       public int FloorArea { get; set; }
       public int LotArea { get; set; }
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

        [Required(ErrorMessage = "Status is required.")]
        public PropertyStatus Status { get; set; }

        public enum PropertyType
        {
            House = 0, Apartment = 1, Duplex = 2, TownHouse = 3, Condor = 4
        }
        [Required(ErrorMessage = "Type is required.")]
        public PropertyType Type { get; set; }

        public ApplicationUser Agent { get; set; }
        public ICollection<Image> Images { get; set; }

        public ICollection<Offer> Offers { get; set; }
       // public ICollection<Favorite> Favorites { get; set; }



    }
}
