using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class ViewRoomsModel : PageModel
    {
        public AppDbContext Rooms2 = new();
        public List<Room> RoomsList {get; set;} = default!;
        public void OnGet()
        {
            RoomsList = (from item in Rooms2.Rooms 
            where item.IsDeleted == false 
            select item).ToList();
        }
        public IActionResult OnPostDelete(int id){
            if(Rooms2.Rooms != null){
                var room = Rooms2.Rooms.Find(id);
                if(room!= null){
                    room.IsDeleted = true;
                    Rooms2.SaveChanges();
                }
            }
            return RedirectToAction("Get");
        }
    }
}
