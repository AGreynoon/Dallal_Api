using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DallalCore.Models
{
    public class User
    {
        public int userID { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        [Required]
        [MaxLength(15)]
        public string Fname { get; set; }
        [Required]
        [MaxLength(15)]
        public string Lname { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        public string UserPassword { get; set; }
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(50)]
        public string WhatsApp { get; set; }
        [Required]
        public int addressId { get; set; }
        public virtual Address address { get; set; }
    }
}
