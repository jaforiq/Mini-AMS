using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mini_AMS.Models
{
    public class VoucherHeader
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string ReferenceNo { get; set; }
        [Required]
        public string VoucherType { get; set; } 

        public List<VoucherLine> Lines { get; set; } = new();
    }
}
