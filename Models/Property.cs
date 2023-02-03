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

       public string Title { get; set; }
       public string Description { get; set; }
       public string Address { get; set; }
       public string PostalCode { get; set; }

       public int Price { get; set;}
       public int Floors { get; set; }
       public int BedroomCount { get; set; }
       public int BathroomCount { get; set; }

       public int GarageCont { get; set; }
       public DateTime YearBuilt { get; set; }
       public int FloorArea { get; set; }
       public int LotArea { get; set; }
       
       public DateTime CreatedAt { get; set; }

       public enum PropertyStatus {
        Pending = 0, Sold = 1, ForSale = 2
       }
       public PropertyStatus Status{get; set;}

       public enum PropertyType {
        House = 0, Apartment = 1, Duplex = 2, TownHouse = 3, Condor = 4
       }
       public PropertyType Type{get; set;}

        
        public ICollection<Image> Images { get; set; }      
       



    }
}