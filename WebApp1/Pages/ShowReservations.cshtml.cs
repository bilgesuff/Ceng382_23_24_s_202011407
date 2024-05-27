using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp1.Models;
using WebApp1.Data;

namespace WebApp1.Pages
{
    public class ShowReservationsModel : PageModel
    {
        private readonly AppDbContext _context;

        public ShowReservationsModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Reservation> Reservations { get; set; }
        public IList<Room> Rooms { get; set; }

        [BindProperty(SupportsGet = true)]
        public string RoomName { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? RoomCapacity { get; set; }

        public async Task OnGetAsync()
        {
            Rooms = await _context.Rooms.ToListAsync();

            var reservationsQuery = _context.Reservations.Include(r => r.Room).AsQueryable();

            if (!string.IsNullOrEmpty(RoomName))
            {
                reservationsQuery = reservationsQuery.Where(r => r.Room.RoomName.Contains(RoomName));
            }

          if (StartDate.HasValue)
            {
                reservationsQuery = reservationsQuery.Where(r => r.Time.Date >= StartDate.Value);
            }

            if (RoomCapacity.HasValue)
            {
                reservationsQuery = reservationsQuery.Where(r => r.Room.Capacity >= RoomCapacity.Value);
            }

            Reservations = await reservationsQuery.ToListAsync();
        }
    }
}

