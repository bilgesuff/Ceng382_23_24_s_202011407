using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class CreateRoomsModel : PageModel
    {
        [BindProperty]
        public Room NewRoom { get; set; } = default!;
        public AppDbContext RoomList = new();
        public void OnGet()
        {
        }
        public IActionResult OnPost(){
            if(!ModelState.IsValid || NewRoom == null){
                return Page();
            }
            NewRoom.IsDeleted = false;
            RoomList.Add(NewRoom);
            RoomList.SaveChanges();
            return RedirectToPage("/ViewRooms");

        }
    }
}
