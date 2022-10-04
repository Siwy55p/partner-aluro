using partner_aluro.DAL;
using partner_aluro.Models;

namespace partner_aluro.Services
{
    public class KoszykMenager
    {
        private ApplicationDbContext _context;
        private string _session;
        public KoszykMenager(string session, ApplicationDbContext context)
        {
            _context = context;
            _session = session;
        }

        public List<PozycjaKoszyka> PobierzKoszyk()
        {
            List<PozycjaKoszyka> koszyk;

            if(true) //Jesli nasz koszyk jest pusty 
            {
                koszyk = new List<PozycjaKoszyka>();
            }
            else
            {
                koszyk = PobierzKoszyk();// pobierz koszyk z sessi
            }

            return koszyk;
        }

        public void DodajDoKoszyka(int produktId)
        {

        }
    }


}
