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
    public class TemplatesController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TemplatesController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Templates
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 3;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Templates.Count() / pageSize);

            return View(await _context.Templates.OrderByDescending(p => p.Id).Skip((p - 1) * pageSize)
                                                                             .Take(pageSize)
                                                                             .ToListAsync());
            //stock code 
            //return _context.Templates != null ? 
            //              View(await _context.Templates.ToListAsync()) :
            //              Problem("Entity set 'DataContext.Templates'  is null.");
        }

        // GET: Templates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Templates == null)
            {
                return NotFound();
            }

            var template = await _context.Templates.FirstOrDefaultAsync(m => m.Id == id);
            if (template == null)
            {
                return NotFound();
            }

            return View(template);
        }


        // GET: Templates/Profile/5
        public async Task<IActionResult> Profile(int? id)
        {
            if (id == null || _context.Templates == null)
            {
                return NotFound();
            }

            var template = await _context.Templates.FirstOrDefaultAsync(m => m.Id == id);
            if (template == null)
            {
                return NotFound();
            }

            return View(template);
        }



       



        // GET: Templates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Templates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Template template)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(template);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}

            if (ModelState.IsValid)
            {
                //template.Slug = template.FName.ToLower().Replace(" ", "-") +"-"+ template.LName.ToLower().Replace(" ", "-");

                //var slug = await _context.Templates.FirstOrDefaultAsync(p => p.Slug == template.Slug);
                //if (slug != null)
                //{
                //    ModelState.AddModelError("", "The template already exists.");
                //    return View(template);
                //}

                if (template.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/Templates");
                    //string imageName = Guid.NewGuid().ToString() + "_" + Template.ImageUpload.FileName;
                    string ImgExtension = Path.GetExtension(template.ImageUpload.FileName);
                    string imageName = template.Name + ImgExtension;

                    string filePath = Path.Combine(uploadsDir, imageName);

                    //string ImagePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await template.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    template.ImageName = imageName;
                    //template.ImageURL = "~/wwwroot/media/Templates/" + imageName ;
    }

                _context.Add(template);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The New Template has been created!";

                return RedirectToAction("Index");

            }


            return View(template);
        }

        // GET: Templates/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Templates == null)
            {
                return NotFound();
            }

            var template = await _context.Templates.FindAsync(id);
            if (template == null)
            {
                return NotFound();
            }
            return View(template);
        }

        // POST: Templates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Template template)
        {
            if (id != template.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //template.Slug = template.FName.ToLower().Replace(" ", "-") + "-" + template.LName.ToLower().Replace(" ", "-");

                //var slug = await _context.Templates.FirstOrDefaultAsync(p => p.Slug == template.Slug);
                //if (slug != null)
                //{
                //    ModelState.AddModelError("", "The template already exists.");
                //    return View(template);
                //}

                if (template.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/Templates");
                    //string imageName = Guid.NewGuid().ToString() + "_" + template.ImageUpload.FileName;
                    string ImgExtension = Path.GetExtension(template.ImageUpload.FileName);
                    string imageName = template.Name + ImgExtension ;

                    string filePath = Path.Combine(uploadsDir, imageName);

                    //string ImagePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await template.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    template.ImageName = imageName;
                    //template.ImageURL = "~/wwwroot/media/Templates/" + imageName;
                }

                try
                {
                    _context.Update(template);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "The New Template has been edited!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemplateExists(template.Id))
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
            return View(template);
        }

        // GET: Templates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Templates == null)
            {
                return NotFound();
            }

            var template = await _context.Templates.FirstOrDefaultAsync(m => m.Id == id);
            if (template == null)
            {
                return NotFound();
            }

            return View(template);
        }

        // POST: Templates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Templates == null)
            {
                return Problem("Entity set 'DataContext.Templates'  is null.");
            }
            var template = await _context.Templates.FindAsync(id);
            if (template != null)
            {
                _context.Templates.Remove(template);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TemplateExists(int id)
        {
          return (_context.Templates?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
