using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReportRegister.Areas.Identity.Data;
using ReportRegister.Models;

namespace ReportRegister.Controllers
{
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public ReportsController(AppDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reports.ToListAsync());
        }

        // GET: Reports/Download/link
        public async Task<IActionResult> Download([FromQuery]string filename)
        {
            Console.WriteLine("##############"+filename);
            if (filename == null)
                return NotFound();

            string path = Path.Combine(hostingEnvironment.WebRootPath, "uploads", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var type = Path.GetExtension(path).ToLowerInvariant();
            if (type == ".pdf")
            {
                type = "application/pdf";
            }
            else
            {
                type = "image/jpeg";
            }

            var name = Path.GetFileName(path);
            name = name.Substring(name.IndexOf("_")+1);
            return File(memory, type, name);
        }
        

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.Files)
                .Include(r => r.Replies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Files")] ReportViewModel report)
        {
            if (ModelState.IsValid)
            {
                List<Models.File> newFiles = new List<Models.File>();
                if (report.Files != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    foreach (IFormFile item in report.Files)
                    {

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                        string readyPath = Path.Combine(uploadsFolder, uniqueFileName);
                        item.CopyTo(new FileStream(readyPath, FileMode.Create));
                        newFiles.Add(new Models.File { Name = uniqueFileName });
                    }
                }
                
                Report newReport = new Report() 
                {
                    Status = ReportStatus.CREATED,
                    //Author = "ZMIEN TO",
                    Date = DateTime.Now,
                    Title = report.Title,
                    Description = report.Description,
                    Files = newFiles

                };
                _context.Add(newReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }

        // GET: Reports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.Include(r => r.Files)
                .FirstOrDefaultAsync(m => m.Id == id); ;
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Status")] Report report)
        {
            if (id != report.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldReport = _context.Reports.Find(id);
                    report.Title = oldReport.Title;
                    report.Date = oldReport.Date;
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }

        // GET: Reports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }
    }
}
