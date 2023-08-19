using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ContactManager.Models;
using ContactManager.Data;
using Microsoft.EntityFrameworkCore;
using PaginatedList.Models;

namespace ContactManager.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index(int pageNumber=1)
        {
              return _context.Contact != null ? 
                          View(await PaginatedList<Contact>.CreateAsync(_context.Contact, pageNumber, 5)) :
                          Problem("Entity set 'ApplicationDbContext.Contact'  is null.");
        }

    // public async Task<IActionResult> Index()
    //     {
    //           return _context.Contact != null ? 
    //                       View(await _context.Contact.ToListAsync()) :
    //                       Problem("Entity set 'ApplicationDbContext.Contact'  is null.");
    //     }
   
    // public IActionResult Index()
    // {
    //     return View();
    // }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
