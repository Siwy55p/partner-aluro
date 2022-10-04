using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace partner_aluro.Models;

public class ApplicationUser : IdentityUser
{
    public string? Imie { get; set; }
    public string? Nazwisko { get; set; }
}

public class ApplicationRole : IdentityRole
{

}
