using CreateDbFromScratch.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks; // Added this
using System.Collections.Generic; // Added this
using System; // Added this

namespace MyApp.Namespace
{
    public class dashboardModel : PageModel
    {
        private readonly AppDbContext _context;
        public dashboardModel(AppDbContext context) { _context = context; }

        public List<Work> ToDoList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Auth/Login");
            }

            var now = DateTime.Now;

            ToDoList = await _context.Works
                .Include(w => w.Subject)
                .Where(w => w.Subject.UserId == userId.Value &&
                            w.Status != WorkStatus.Completed) // Hides completed
                .OrderBy(w => w.DueDate) // Overdue (past) comes first
                .ToListAsync();

            return Page();
        }

        // In Pages/Dashboard.cshtml.cs
        public async Task<JsonResult> OnGetCalendarEventsAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return new JsonResult(new List<object>());
            }

            var now = DateTime.Now;

            var events = await _context.Works
                .Include(w => w.Subject)
                .Where(w => w.Subject.UserId == userId.Value)
                .Select(w => new {
                    // --- FIX 1: ADD THIS ID ---
                    // Your JavaScript eventClick needs this
                    id = w.Id,

                    title = w.Title,

                    // --- FIX 2: FORMAT THE DATE ---
                    // This is a safer format for FullCalendar
                    start = w.DueDate.ToString("yyyy-MM-dd"),

                    // --- FIX 3: REMOVE THE URL ---
                    // Your JavaScript is handling the redirect, so remove this:
                    // url = $"/Work/Workedit?id={w.Id}",

                    // Your color and className logic is great and can stay
                    color = (w.Status == WorkStatus.Completed)
                        ? "#198754" // Green
                        : (w.DueDate < now && w.Status != WorkStatus.Completed)
                            ? "#dc3545" // Red
                            : "#2563eb", // Default: Your site's primary blue

                    className = (w.Status == WorkStatus.Completed)
                                        ? "event-completed"
                                        : (w.DueDate < now && w.Status != WorkStatus.Completed)
                                            ? "event-overdue"
                                            : "event-pending"
                })
                .ToListAsync();

            return new JsonResult(events);
        }
    }
}