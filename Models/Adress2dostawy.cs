using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class Adress2dostawy
    {

        [Required]
        [Key] //Entity inkrementacja po ID
        public int Adres2dostawyId { get; set; }

        public string? Ulica { get; set; }

        public string Miasto { get; set; }
        public string Kraj { get; set; }
        public string? KodPocztowy { get; set; }

        public string? Telefon { get; set; }


        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
