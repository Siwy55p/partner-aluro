using System.ComponentModel.DataAnnotations;

namespace partner_aluro.Models
{
    public class Product
    {

        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "*Pole nazwa jest wymagane")]
        [StringLength(100)]
        public string Name { get; set; }
        public string? Symbol { get; set; }

        public string? Description { get; set; }
        public DateTime? DataDodania { get; set; }
        public string? NazwaPlikuObrazka { get; set; }
        public decimal? CenaProduktu { get; set; }
        public decimal? CenaProduktuDetal { get; set; }
        public decimal? WagaProduktu { get; set; }
        public decimal? SzerokoscProduktu { get; set; }
        public decimal? WysokoscProduktu { get; set; }
        public decimal? GlebokoscProduktu { get; set; }
        public bool Bestseller { get; set; }
        public bool Ukryty { get; set; }



        //wlasciwosc nawigacyjna do kategorii
        public virtual Category? Category { get; set; }

    }
}
