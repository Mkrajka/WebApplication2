using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApplication1.Models
{
    public class ClassMember
    {
        [Key]
        public int ClassMemberID { get; set; }


        public int ClassID { get; set; }
        
        [ForeignKey("ClassID")]
        public Classes Class { get; set; }

        
        public int MemberID { get; set; }
        
        [ForeignKey("MemberID")]
        public Member Member { get; set; }
    }
}
