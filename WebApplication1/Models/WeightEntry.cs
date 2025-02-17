using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("WeightEntry")]
    public class WeightEntry
    {
        [Key]
        public int WeigthID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Required]
        public double Weight { get; set; }



        // Foreign Key
        public int MemberID { get; set; }

        [ForeignKey("MemberID")]
        public Member Member { get; set; }
    }
}
