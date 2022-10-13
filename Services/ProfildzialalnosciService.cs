using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System.Collections.Generic;

namespace partner_aluro.Services
{
    public class ProfildzialalnosciService : IProfildzialalnosciService
    {
        private readonly ApplicationDbContext _context;

        public ProfildzialalnosciService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ProfilDzialalnosci> GetListAllProfils()
        {
            List<ProfilDzialalnosci> Lista = _context.ProfileDzialalnosci.ToList();
            return Lista;
        }

        public ProfilDzialalnosci GetProfil(int Id)
        {
            ProfilDzialalnosci profil = _context.ProfileDzialalnosci.Find(Id);
            return profil;
        }

        public void Create(ProfilDzialalnosci profil)
        {
            _context.ProfileDzialalnosci.Add(profil);
            _context.SaveChanges();
        }

        public void Update(ProfilDzialalnosci profil)
        {
            _context.ProfileDzialalnosci.Update(profil);
            _context.SaveChanges();
        }

        public void Delete(ProfilDzialalnosci profil)
        {
            _context.ProfileDzialalnosci.Remove(profil);
            _context.SaveChanges();
        }
    }
}