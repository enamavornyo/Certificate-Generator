using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Certificate_Generator.Context;
using Certificate_Generator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;

namespace Certificate_Generator.Views
{
    public class CertificatesController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CertificatesController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Certificates
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 3;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Certificates.Count() / pageSize);

            return View(await _context.Certificates.OrderByDescending(p => p.Id).Include(c => c.Course)
                                                                                .Include(c => c.Title)
                                                                                .Skip((p - 1) * pageSize)
                                                                             .Take(pageSize)
                                                                             .ToListAsync());
            //stock code 
            //return _context.Certificates != null ? 
            //              View(await _context.Certificates.ToListAsync()) :
            //              Problem("Entity set 'DataContext.Certificates'  is null.");
        }

        // GET: Certificates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Certificates == null)
            {
                return NotFound();
            }

            var certificate = await _context.Certificates
                .Include(c => c.Course)
                .Include(c => c.Title)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (certificate == null)
            {
                return NotFound();
            }

            return View(certificate);
        }


        // GET: Certificates/Profile/5
        public async Task<IActionResult> Print(int? id)
        {
            if (id == null || _context.Certificates == null)
            {
                return NotFound();
            }

            var certificate = await _context.Certificates.Include(c => c.Course)
                .Include(c => c.Title).FirstOrDefaultAsync(m => m.Id == id);
            if (certificate == null)
            {
                return NotFound();
            }

            return View(certificate);
        }







        // GET: Certificates/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["TitleId"] = new SelectList(_context.Titles, "Id", "Name");
            return View();
        }

        // POST: Certificates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Certificate certificate)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(certificate);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}

            if (ModelState.IsValid)
            {
                //certificate.Slug = certificate.FName.ToLower().Replace(" ", "-") +"-"+ certificate.LName.ToLower().Replace(" ", "-");

                //var slug = await _context.Certificates.FirstOrDefaultAsync(p => p.Slug == certificate.Slug);
                //if (slug != null)
                //{
                //    ModelState.AddModelError("", "The certificate already exists.");
                //    return View(certificate);
                //}

                if (certificate.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/Certificates");
                    //string imageName = Guid.NewGuid().ToString() + "_" + certificate.ImageUpload.FileName;
                    string ImgExtension = Path.GetExtension(certificate.ImageUpload.FileName);
                    string imageName = certificate.SerialNum + ImgExtension;

                    //+certificate.FName + certificate.LName +

                    string filePath = Path.Combine(uploadsDir, imageName);

                    //string ImagePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await certificate.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    certificate.ImageName = imageName;
                    //certificate.ImageURL = "~/wwwroot/media/Certificates/" + imageName ;
                }

                _context.Add(certificate);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The New Certificate has been created!";

                return RedirectToAction("Index");

            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", certificate.CourseId);
            ViewData["TitleId"] = new SelectList(_context.Titles, "Id", "Name", certificate.TitleId);

            return View(certificate);
        }

        // GET: Certificates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Certificates == null)
            {
                return NotFound();
            }
            

            var certificate = await _context.Certificates.FindAsync(id);

            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", certificate.CourseId);
            ViewData["TitleId"] = new SelectList(_context.Titles, "Id", "Name", certificate.TitleId);
            if (certificate == null)
            {
                return NotFound();
            }
            return View(certificate);
        }

        // POST: Certificates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Certificate certificate)
        {
            if (id != certificate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //certificate.Slug = certificate.FName.ToLower().Replace(" ", "-") + "-" + certificate.LName.ToLower().Replace(" ", "-");

                //var slug = await _context.Certificates.FirstOrDefaultAsync(p => p.Slug == certificate.Slug);
                //if (slug != null)
                //{
                //    ModelState.AddModelError("", "The certificate already exists.");
                //    return View(certificate);
                //}

                if (certificate.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/Certificates");
                    //string imageName = Guid.NewGuid().ToString() + "_" + certificate.ImageUpload.FileName;
                    string ImgExtension = Path.GetExtension(certificate.ImageUpload.FileName);
                    string imageName = certificate.SerialNum + certificate.FName + certificate.LName + ImgExtension;

                    string filePath = Path.Combine(uploadsDir, imageName);

                    //string ImagePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await certificate.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    certificate.ImageName = imageName;
                    //certificate.ImageURL = "~/wwwroot/media/Certificates/" + imageName;
                }

                try
                {
                    _context.Update(certificate);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "The New Certificate has been edited!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CertificateExists(certificate.Id))
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

            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", certificate.CourseId);
            ViewData["TitleId"] = new SelectList(_context.Titles, "Id", "Name", certificate.TitleId);

            return View(certificate);
        }

        // GET: Certificates/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Certificates == null)
            {
                return NotFound();
            }

            var certificate = await _context.Certificates.FirstOrDefaultAsync(m => m.Id == id);
            if (certificate == null)
            {
                return NotFound();
            }

            return View(certificate);
        }

        // POST: Certificates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Certificates == null)
            {
                return Problem("Entity set 'DataContext.Certificates'  is null.");
            }
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate != null)
            {
                _context.Certificates.Remove(certificate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CertificateExists(long id)
        {
            return (_context.Certificates?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
