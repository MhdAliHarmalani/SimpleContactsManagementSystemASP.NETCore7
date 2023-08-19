using System.Security.AccessControl;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContactManager.Data;
using ContactManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using PaginatedList.Models;
using System.IO;

namespace ContactManager.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContactsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Contacts
        public async Task<IActionResult> Index(int pageNumber=1)
        {
              return _context.Contact != null ? 
                          View(await PaginatedList<Contact>.CreateAsync(_context.Contact, pageNumber, 5)) :
                          Problem("Entity set 'ApplicationDbContext.Contact'  is null.");
        }
        // public async Task<IActionResult> Index()
        // {
        //       return _context.Contact != null ? 
        //                   View(await _context.Contact.ToListAsync()) :
        //                   Problem("Entity set 'ApplicationDbContext.Contact'  is null.");
        // }

        // GET: Contacts/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone,File")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                // List of allowed image content types
                string[] allowedImageTypes = { "image/jpeg", "image/png", "image/gif","image/jpg" };

                if (!allowedImageTypes.Contains(contact.File.ContentType))
                {
                    ModelState.AddModelError("File", "Only image files are allowed.");
                    return View(contact);
                }

                // Maximum allowed file size in bytes (1MB)
                long maxFileSize = 1 * 1024 * 1024; // 1 MB

                if (contact.File.Length > maxFileSize)
                {
                    ModelState.AddModelError("File", "The file size cannot exceed 1MB.");
                    return View(contact);
                }

                var fakeFileName = Guid.NewGuid().ToString() + contact.File.FileName;

                contact.FileName = contact.File.FileName;
                contact.ContentType = contact.File.ContentType;
                contact.StoredFileName = fakeFileName;
                
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "images", fakeFileName);
                using FileStream fileStream = new(path, FileMode.Create);
                contact.File.CopyTo(fileStream);
                
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contacts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Phone,File")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // List of allowed image content types
                    string[] allowedImageTypes = { "image/jpeg", "image/png", "image/gif","image/jpg" };

                    if (!allowedImageTypes.Contains(contact.File.ContentType))
                    {
                        ModelState.AddModelError("File", "Only image files are allowed.");
                        return View(contact);
                    }

                    // Maximum allowed file size in bytes (1MB)
                    long maxFileSize = 1 * 1024 * 1024; // 1 MB

                    if (contact.File.Length > maxFileSize)
                    {
                        ModelState.AddModelError("File", "The file size cannot exceed 1MB.");
                        return View(contact);
                    }

                    
                    // Retrieve the existing contact from the database to delete the prev image file
                    var existingContact = await _context.Contact.FindAsync(contact.Id);
                    // Clear Tracking the _context
                    _context.ChangeTracker.Clear();
                    


                    var fakeFileName = Guid.NewGuid().ToString() + contact.File.FileName;
                    contact.FileName = contact.File.FileName;
                    contact.ContentType = contact.File.ContentType;
                    contact.StoredFileName = fakeFileName;
                    
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "images", fakeFileName);
                    using FileStream fileStream = new(path, FileMode.Create);
                    contact.File.CopyTo(fileStream);

                    _context.Update(contact);
                    await _context.SaveChangesAsync();

                    // Delete the prev image file
                    if (existingContact != null)
                    {
                        if (!string.IsNullOrEmpty(existingContact.StoredFileName))
                        {
                            // Construct the file path by combining the web root path and the stored file name
                            var prevFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", existingContact.StoredFileName);

                            // Check if the file exists at the specified path
                            if (System.IO.File.Exists(prevFilePath))
                            {
                                // Delete the existing file
                                System.IO.File.Delete(prevFilePath);
                            }
                        }
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
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
            return View(contact);
        }

        // GET: Contacts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contact == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Contact'  is null.");
            }


            // Retrieve the existing contact from the database to delete the prev image file
            var existingContact = await _context.Contact.FindAsync(id);
            // Clear Tracking the _context
            _context.ChangeTracker.Clear();


            var contact = await _context.Contact.FindAsync(id);
            if (contact != null)
            {
                _context.Contact.Remove(contact);
            }
            
            await _context.SaveChangesAsync();

            // Delete the prev image file
            if (existingContact != null)
            {
                if (!string.IsNullOrEmpty(existingContact.StoredFileName))
                {
                    // Construct the file path by combining the web root path and the stored file name
                    var prevFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", existingContact.StoredFileName);

                    // Check if the file exists at the specified path
                    if (System.IO.File.Exists(prevFilePath))
                    {
                        // Delete the existing file
                        System.IO.File.Delete(prevFilePath);
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
          return (_context.Contact?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
