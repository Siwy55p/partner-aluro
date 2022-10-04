using System.ComponentModel.DataAnnotations;

namespace partner_aluro.Models
{
    public class Adress1
    {
        [Required]
        [Key] //Entity inkrementacja po ID
        public int Id { get; set; }

        public string Miasto { get; set; }
        public string? Kraj { get; set; }

        public string? Ulica { get; set; }

        //[StringLength(7)]
        public string? KodPocztowy { get; set; }

        //[StringLength(9)]
        public string? Telefon { get; set; }

        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
    }
}
