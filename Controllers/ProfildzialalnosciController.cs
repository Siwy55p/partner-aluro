using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace partner_aluro.Controllers
{
    public class ProfilDzialalnosciController : Controller
    {
        // GET: ProfilDzialalnosciController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProfilDzialalnosciController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProfilDzialalnosciController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProfilDzialalnosciController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProfilDzialalnosciController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProfilDzialalnosciController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProfilDzialalnosciController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProfilDzialalnosciController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
