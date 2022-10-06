using System.ComponentModel.DataAnnotations;

namespace partner_aluro.Models
{
    public class Adress1
    {
        [Required]
        [Key] //Entity inkrementacja po ID
        public int Id { get; set; }

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

        public string? UserID { get; set; }
        public ApplicationUser User { get; set; }


    }
}
