using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace partner_aluro.Models
{
    public class Adress1rozliczeniowy
    {
        

        [Required]
        [Key] //Entity inkrementacja po ID
        public int Adres1rozliczeniowyId { get; set; }

        [Required]
        public string Miasto { get; set; }
        [Required]
        public string? Kraj { get; set; }
        [Required]
        public string? Ulica { get; set; }
        [Required]
        [StringLength(7)]
        public string? KodPocztowy { get; set; }


        [StringLength(9)]
        public string? Telefon { get; set; }


        [ForeignKey("ApplicationUser")]
        public string? UserID { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }




    }

}
