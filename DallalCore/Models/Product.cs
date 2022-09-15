using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DallalCore.Models
{
    public class Product
    {
        public int productId { get; set; }
        [Required]
        [MaxLength(100)]
        public string title { get; set; }
        [Required]
        [MaxLength(250)]
        public string description { get; set; }
        [MaxLength(100)]
        public string pics_path { get; set; }       
        [DataType(DataType.Currency)]
        public float? price { get; set; }
        [MaxLength(50)]
        public string currency { get; set; }
        [MaxLength(50)]
        public string period_of_time { get; set; }
        public DateTime dateTime { get; set; }
        public bool isBlocked { get; set; }
        public int? userId { get; set; }
        public int addressId { get; set; }
        public int categoryId { get; set; }
        public virtual Category category { get; set; }
        public virtual Address address { get; set; }
        public virtual User user { get; set; }

    }
}
