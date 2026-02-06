using LeavePortal.Models;

namespace LeavePortal.Repositories
{
    public interface ILeaveService
    {
        Task<List<LeaveRequest>> GetAllAsync();
        Task<List<LeaveRequest>> GetByEmployeeIdAsync(string employeeId);
        Task AddAsync(LeaveRequest request);
        Task UpdateAsync(LeaveRequest request);
    }
}
