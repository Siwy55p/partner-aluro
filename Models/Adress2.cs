using System.ComponentModel.DataAnnotations;

namespace partner_aluro.Models
{
    public class Adress2
    {
        [Required]
        [Key] //Entity inkrementacja po ID
        public int Id { get; set; }

        public string? Miasto { get; set; }

        public string? Ulica { get; set; }

        [Required(ErrorMessage = "Wprowadz Kod Pocztowy")]
        [StringLength(7)]
        public string? KodPocztowy { get; set; }

        [Required(ErrorMessage = "Wprowadz nr tel.")]
        [StringLength(9)]
        public string? Telefon { get; set; }

        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
    }
}
