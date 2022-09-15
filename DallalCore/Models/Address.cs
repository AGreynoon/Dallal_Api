using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DallalCore.Models
{
    public class Address
    {
        public int addressID { get; set; }
        [Required]
        [MaxLength(15)]
        public string governorate { get; set; }
        [Required]
        [MaxLength(15)]
        public string district { get; set; }
    }
}
