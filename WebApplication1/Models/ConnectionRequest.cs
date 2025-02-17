﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ConnectionRequest
    {
        [Key]
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Status { get; set; }
    }
}
