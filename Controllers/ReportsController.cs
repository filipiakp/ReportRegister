using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using ReportRegister.Areas.Identity.Data;
using ReportRegister.Models;

namespace ReportRegister.Controllers
{
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(AppDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        // GET: Reports
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(PredefinedRoles.Employee))
            {
                return View(await _context.Reports.ToListAsync());
            }
            else
            {
                ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
                return View(_context.Reports.Where(r => r.Author.Id == applicationUser.Id));
            }
                
        }

        // GET: Reports/Download?filename=fullname
        [Authorize]
        public async Task<IActionResult> Download([FromQuery]string filename)
        {
            if (filename == null)
                return NotFound();

            string path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", filename);

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
        [Authorize]
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
        // POST: Reports/Reply/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Reply(int id, [Bind("Content")] Reply reply)
        {
            var report = await _context.Reports
                .Include(r => r.Replies)
                .FirstOrDefaultAsync(r => r.Id == id);
            Console.WriteLine("dziala "+reply.Content);
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            //string userEmail = applicationUser?.Email; // will give the user's Email
            reply.Author = applicationUser;
            reply.Date = DateTime.Now;
            report.Replies.Add(reply);
            _context.Update(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details),new RouteValueDictionary { { "id",id} });
        }

        // GET: Reports/Create
        [Authorize(Roles = PredefinedRoles.User)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = PredefinedRoles.User)]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Files")] ReportViewModel report)
        {
            if (ModelState.IsValid)
            {
                List<Models.File> newFiles = new List<Models.File>();
                if (report.Files != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    if (report.Files.Sum(x => x.Length)<20*1024*1024)
                    foreach (IFormFile item in report.Files)
                    {
                        //filename = guid-number_name
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                        string readyPath = Path.Combine(uploadsFolder, uniqueFileName);
                        item.CopyTo(new FileStream(readyPath, FileMode.Create));
                        newFiles.Add(new Models.File { Name = uniqueFileName });
                    }
                }
                ApplicationUser author = await _userManager.GetUserAsync(User);
                Report newReport = new Report() 
                {
                    Status = ReportStatus.CREATED,
                    Author = author,
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
        [Authorize(Roles = PredefinedRoles.Employee)]
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
        [Authorize(Roles = PredefinedRoles.Employee)]
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
                    report.Author = oldReport.Author;
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
        [Authorize(Roles = PredefinedRoles.Employee)]
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
        [Authorize(Roles = PredefinedRoles.Employee)]
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
