using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DallalCore.Models
{
    public class Category
    {
        public int categoryID { get; set; }
        [Required]
        [MaxLength(30)]
        public string category { get; set; }
    }
}
