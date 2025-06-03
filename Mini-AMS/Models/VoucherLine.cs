using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mini_AMS.Models
{
    public class VoucherLine
    {
        public int Id { get; set; }
        [Required]
        public int VoucherHeaderId { get; set; }
        [ForeignKey("VoucherHeaderId")]
        public VoucherHeader VoucherHeader { get; set; }

        [Required]
        public int AccountId { get; set; }
        [Required]
        public decimal Debit { get; set; }
        [Required]
        public decimal Credit { get; set; }
        public string Narration { get; set; }
    }
}
