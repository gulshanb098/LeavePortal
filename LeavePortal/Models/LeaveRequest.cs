using System.ComponentModel.DataAnnotations;

namespace LeavePortal.Models
{
    public class LeaveRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string EmployeeId { get; set; }

        [Required(ErrorMessage = "Leave Date is required")]
        [DataType(DataType.Date)]
        public DateTime LeaveDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Reason is required")]
        public string Reason { get; set; } = string.Empty;

        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
    }
}
