using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Plugins;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Areas.Identity.Data;

namespace WebApplication1.Models
{
    [Table("Member")]
    public class Member
    {
        [Key]
        public int MemberID { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string MemberFirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string MemberLastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string MemberEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string MemberPhoneNumber { get; set; }

        [Required(ErrorMessage = "Date joined is required.")]
        [DataType(DataType.Date)]
        public DateTime MemberDateJoined { get; set; }

        // Date if left
        [DataType(DataType.Date)]
        public DateTime? MemberDateLeft { get; set; }

        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } 

        public ICollection<TransactionFee> TransactionFee {  get; set; }
        public ICollection<EntryLog> EntryLog { get; set; } 

        public ICollection<CalorieEntry> CalorieEntry { get; set;}

        public ICollection<WeightEntry> WeightEntry { get; set; }
    }
}
