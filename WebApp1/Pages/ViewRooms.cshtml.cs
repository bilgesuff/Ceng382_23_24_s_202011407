using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp1.Data;

namespace MyApp.Namespace
{   
     public class ViewRoomsModel : PageModel
    {
        private readonly AppDbContext _context;

        public ViewRoomsModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Room> RoomsList { get; set; }

        public void OnGet()
        {
            RoomsList = (from item in _context.Rooms
                         select item).ToList();
        }
            public IActionResult OnPostDelete(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room != null)
            {
                room.IsDeleted = true;
                _context.SaveChanges();
            }

            return RedirectToPage("./ViewRooms");
        }
    }
        
    }


