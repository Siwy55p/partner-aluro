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
    public string? NazwaFirmy { get; set; }
    public DateTime? DataZałożenia { get; set; }

    public virtual Adress1 Adres1 { get; set; }
    public virtual Adress2? Adres2 { get; set; }

    public string? NotatkaOsobista { get; set; }


}
public class ApplicationRole : IdentityRole
{

}
