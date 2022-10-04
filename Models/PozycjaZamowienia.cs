namespace partner_aluro.Models
{
    public class PozycjaZamowienia
    {
        public int PozycjaZamowieniaID { get; set; }
        public int ZamowienieId { get; set; } //klucz obcy zamowienia
        public int ProduktID { get; set; } //klicz obcy Produktu ID
        public int Ilosc { get; set; }
        public decimal CenaZakupu { get; set; } // cena moze byc inna niz cena producktu 

        public virtual Product produkt { get; set; }
        public virtual Zamowienie zamowienie { get; set; }

    }
}