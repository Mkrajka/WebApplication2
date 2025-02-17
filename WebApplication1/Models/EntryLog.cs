using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class EntryLog
    {
        [Key]
        public int EntryLogId { get; set; }

        // Member information (link this to your existing Member table)
        public int MemberId { get; set; }
        public Member Member { get; set; }

        // Entry information
        public DateTime EntryDate { get; set; }

        // Additional information if needed (e.g., RFID tag identifier)
        public string RfidTag { get; set; }
    }
}
