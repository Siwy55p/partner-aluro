using System.ComponentModel.DataAnnotations;


namespace partner_aluro.Models
{
    public class Adress2
    {
        [Required]
        [Key] //Entity inkrementacja po ID
        public int Id { get; set; }

        public string? Miasto { get; set; }
        public string? Kraj { get; set; }

        public string? Ulica { get; set; }

        public string? KodPocztowy { get; set; }

        public string? Telefon { get; set; }

        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
    }
}
