using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.DTO
{
    public class CheckInOutSetitngDto
    {
        public TimeSpan Checkin { get; set; }
        public TimeSpan Checkout { get; set; }
    }
}
