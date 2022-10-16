namespace partner_aluro.Models
{
    public class ProductImages
    {
        public int Id { get; set; }
        public string ImageURL { get; set; }

        public Product Product { get; set; }
    }
}
