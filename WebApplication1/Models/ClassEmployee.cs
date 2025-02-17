using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class ClassEmployee
    {
        [Key]
        public int ClassEmployeeID { get; set; }


        public int ClassID { get; set; }

        [ForeignKey("ClassID")]
        public Classes Class { get; set; }


        public int EmployeeID { get; set; }

        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }
    }
}
