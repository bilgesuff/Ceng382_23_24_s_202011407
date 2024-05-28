using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp1.Models;
using WebApp1.Data;

namespace MyApp.Namespace
{
    public class ShowReservationsModel : PageModel
    {
        private readonly AppDbContext _context;

        public ShowReservationsModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Reservation> ReservationsList { get; set; }
        public Dictionary<string, List<Reservation>> WeeklyReservations { get; set; }
        public List<DateTime> Weeks { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime SelectedWeek { get; set; }

        [BindProperty(SupportsGet = true)]
        public string RoomNameFilter { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public DateTime? StartDateFilter { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public DateTime? EndDateFilter { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int? CapacityFilter { get; set; }

        public async Task OnGetAsync()
        {
            // Varsayılan olarak bu haftayı seç
            if (SelectedWeek == default)
            {
                SelectedWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            }

            // Haftalık rezervasyonları getir
            var reservationsQuery = _context.Reservations
                .Include(r => r.Room)
                .Where(r => r.Room.IsDeleted == false && r.Time >= SelectedWeek && r.Time < SelectedWeek.AddDays(7));

            if (!string.IsNullOrEmpty(RoomNameFilter))
            {
                reservationsQuery = reservationsQuery.Where(r => r.Room.RoomName.Contains(RoomNameFilter));
            }

            if (StartDateFilter.HasValue)
            {
                reservationsQuery = reservationsQuery.Where(r => r.Time.Date >= StartDateFilter.Value);
            }

            if (CapacityFilter.HasValue)
            {
                reservationsQuery = reservationsQuery.Where(r => r.Room.Capacity >= CapacityFilter.Value);
            }

            ReservationsList = await reservationsQuery.ToListAsync();

            WeeklyReservations = ReservationsList
                .GroupBy(r => r.Time.Date.ToString("yyyy-MM-dd"))
                .ToDictionary(g => g.Key, g => g.ToList());

           
            Weeks = new List<DateTime>();
            for (int i = -4; i <= 4; i++)
            {
                Weeks.Add(DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(i * 7));
            }
        }



        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return RedirectToPage("/EditReservation", new { id = reservation.Id });
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}