using LeavePortal.Models;
using LeavePortal.Repositories;

namespace LeavePortal.Services
{
    public class LeaveService
    {
        private readonly ILeaveService leaveService;

        public LeaveService(ILeaveService leaveService)
        {
            this.leaveService = leaveService;
        }

        public async Task<List<LeaveRequest>> GetLeavesForUserAsync(User user)
        {
            return user.Role == UserRole.Manager
                ? await leaveService.GetAllAsync()
                : await leaveService.GetByEmployeeIdAsync(user.Id);
        }

        public async Task ApplyLeaveAsync(User user, LeaveRequest request)
        {
            var existing = await leaveService.GetByEmployeeIdAsync(user.Id);

            if (existing.Any(l => l.LeaveDate.Date == request.LeaveDate.Date))
            {
                throw new Exception($"You've already requested leave for {request.LeaveDate:MM-dd-yyyy}");
            }

            request.Id = Guid.NewGuid().ToString();
            request.EmployeeId = user.Id;
            request.Status = LeaveStatus.Pending;

            await leaveService.AddAsync(request);
        }

        public async Task UpdateLeaveRequestAsync(string id, LeaveStatus leaveStatus)
        {
            var all = await leaveService.GetAllAsync();
            var leave = all.First(l => l.Id == id);
            leave.Status = leaveStatus;
            await leaveService.UpdateAsync(leave);
        }
    }
}
