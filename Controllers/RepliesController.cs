using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportRegister.Controllers
{
    public class RepliesController : Controller
    {
        // GET: RepliesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RepliesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RepliesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RepliesController/Create
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

        // GET: RepliesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RepliesController/Edit/5
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

        // GET: RepliesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RepliesController/Delete/5
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
