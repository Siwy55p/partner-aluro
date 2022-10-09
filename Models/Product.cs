using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Pole Nazwa musi być wypełnione")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Symbol jest wymagany np: A01532")]
        [StringLength(13)]
        public string Symbol { get; set; }

        public string? Description { get; set; }
        public DateTime? DataDodania { get; set; }
        public string? NazwaPlikuObrazka { get; set; }

        [Required(ErrorMessage = "Cena Produktu jest wymagana")]
        [Range(0,99999)]
        public decimal CenaProduktu { get; set; }
        public decimal? CenaProduktuDetal { get; set; }
        public decimal? WagaProduktu { get; set; }
        public decimal? SzerokoscProduktu { get; set; }
        public decimal? WysokoscProduktu { get; set; }
        public decimal? GlebokoscProduktu { get; set; }
        public bool Bestseller { get; set; }
        public bool Ukryty { get; set; }

        [Required(ErrorMessage ="Wybierz obrazek główny")]
        [Display(Name="Obrazek główny")]
        [NotMapped]
        public IFormFile FrontImage { get; set; }

        public string ImageUrl { get; set; }


        //wlasciwosc nawigacyjna do kategorii
        public virtual Category? Category { get; set; }

    }
}
