using NuGet.DependencyResolver;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Areas.Identity.Data;

namespace WebApplication1.Models
{
    public class Classes
    {
        [Key]
        public int ClassID { get; set; }

        public string ClassName { get; set; }
        public string Description { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public TimeSpan Duration { get; set; }

        // Navigation properties for related entities
        public ICollection<ClassMember> ClassMember { get; set; }
        public ICollection<ClassEmployee> ClassEmployee { get; set; }


        // Additional properties for storing selected EmployeeIDs and MemberIDs
        [NotMapped] // Exclude from database mapping
        public List<int> EmployeeIDs { get; set; } = new List<int>();

        [NotMapped] // Exclude from database mapping
        public List<int> MemberIDs { get; set; } = new List<int>();


        // Foreign Key
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
