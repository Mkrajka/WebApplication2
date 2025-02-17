using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Areas.Identity.Data;

namespace WebApplication1.Models
{
    [Table("Resource")]
    public class Resource
    {
        [Key]
        public int ResourceID { get; set; }

        [Required(ErrorMessage = "Resource Item is required.")]
        public string ItemName { get; set; }

        [Required]
        [StringLength(50)]
        public string ItemType { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int ItemQuantity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PurchasedDate { get; set; }



        [Required]
        /*[MaxLength(50)]*/
        public int ItemPrice { get; set; }

        [StringLength(500)]
        public string ItemNotes { get; set; }




        // Foreign Key
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
