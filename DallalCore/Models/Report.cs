using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DallalCore.Models
{
    public class Report
    {
        public int reportID { get; set; }
        public int? userId { get; set; }
        [Required]
        public int productId { get; set; }
        public virtual Product product { get; set; }
        public virtual User user { get; set; }


    }
}
