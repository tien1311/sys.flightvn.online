using Microsoft.AspNetCore.Mvc;

namespace Manager_EV.Views.Shared.Components.BookingContent
{
    public class BookingContent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Views/Shared/_Partial_Data_Booking.cshtml");
        }

    }
}
