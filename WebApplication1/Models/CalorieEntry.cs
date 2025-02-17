using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Areas.Identity.Data;

namespace WebApplication1.Models
{
    [Table("CalorieEntry")]
    public class CalorieEntry
    {
        [Key]
        public int CalorieID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Required]
        public int BreakfastCalories { get; set; }

        [Required]
        public int LunchCalories { get; set; }

        [Required]
        public int DinnerCalories { get; set; }

        [Required]
        public int SnackCalories { get; set; }


        // Foreign Key
        public int MemberID { get; set; }

        [ForeignKey("MemberID")]
        public Member Member { get; set; }
    }
}
