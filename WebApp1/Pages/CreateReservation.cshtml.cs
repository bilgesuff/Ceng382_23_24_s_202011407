using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApp1.Models;
using WebApp1.Data;

namespace WebApp1.Pages
{
    public class CreateReservationModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateReservationModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reservation NewReservation { get; set; }
        public IList<Room> RoomsList { get; set; }

        public async Task OnGetAsync()
        {
            RoomsList = await _context.Rooms.Where(r => r.IsDeleted == false).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Reservations.Add(NewReservation);
            await _context.SaveChangesAsync();

            // Log the reservation creation
            var log = new LogRecord
            {
                Timestamp = DateTime.Now,
                ReservationId = NewReservation.Id,
                RoomId = NewReservation.RoomId
            };
            _context.LogRecords.Add(log);
            await _context.SaveChangesAsync();

            return RedirectToPage("/ShowReservations");
        }
    }
}
