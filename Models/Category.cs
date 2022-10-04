using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace partner_aluro.Models
{
    public class Category
    {
        [Key] //Entity inkrementacja po ID
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Wprowadz nazwę kategorii")]
        public string Name { get; set; }

        public string? Description { get; set;}

        public string? NazwaPlikuIkony { get; set; }


        //Kategoria moze przechowywac produckty
        public virtual ICollection<Product>? Produkty { get; set; }


    }
}
