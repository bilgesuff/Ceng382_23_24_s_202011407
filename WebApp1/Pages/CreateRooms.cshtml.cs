using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class CreateRoomsModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateRoomsModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Room NewRoom { get; set; } = new Room();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || NewRoom == null || string.IsNullOrWhiteSpace(NewRoom.RoomName) || NewRoom.Capacity <= 0)
            {
                if (string.IsNullOrWhiteSpace(NewRoom.RoomName))
                {
                    ModelState.AddModelError("NewRoom.RoomName", "Room name is required.");
                }

                if (NewRoom.Capacity <= 0)
                {
                    ModelState.AddModelError("NewRoom.Capacity", "Capacity must be greater than 0.");
                }

                return Page();
            }

            NewRoom.IsDeleted = false;
            _context.Rooms.Add(NewRoom);
            _context.SaveChanges();

            return RedirectToPage("/ViewRooms");
        }
    }
}
