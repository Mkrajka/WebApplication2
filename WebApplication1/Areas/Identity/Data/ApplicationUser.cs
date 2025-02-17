using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;

namespace WebApplication1.Areas.Identity.Data;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }

    public List<Member> Member { get; set; } = new List<Member>();

    public List<Resource> Resource { get; set; } = new List<Resource>();
    public List<Employee> Employee { get; set; } = new List<Employee>();

    public List<Classes> Classes { get; set; } = new List<Classes>();
}

