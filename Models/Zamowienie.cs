using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace partner_aluro.Models
{
    public class Zamowienie
    {
        public int ZamowienieId { get; set; }

        [Required(ErrorMessage = "Wprowadz Imię")]
        [StringLength(100)]
        public string Imię   { get; set; }

        [Required(ErrorMessage = "Wprowadz Nazwisko")]
        [StringLength(100)]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Wprowadz Ulicę")]
        [StringLength(100)]
        public string Ulica { get; set; }

        [Required(ErrorMessage = "Wprowadz Miasto")]
        [StringLength(100)]
        public string Miasto { get; set; }

        [Required(ErrorMessage = "Wprowadz Kod Pocztowy")]
        [StringLength(7)]
        public string KodPocztowy { get; set; }


        [Required(ErrorMessage = "Wprowadz nr tel.")]
        [StringLength(9)]
        public string Telefon { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Komentarz { get; set;}
        public DateTime DataDodania { get; set; }
        public StanZamowienia2 StanZamowienia { get; set; }
        public decimal Wartosc { get; set; }

        List<PozycjaZamowienia> PozycjeZamowienia { get; set; }
    }

    public enum StanZamowienia2
    {
        Nowe,
        Zrealizowane
    }

}
