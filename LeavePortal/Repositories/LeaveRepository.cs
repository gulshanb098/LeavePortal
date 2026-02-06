using LeavePortal.Models;
using System.Text.Json;

namespace LeavePortal.Repositories
{
    public class LeaveRepository: ILeaveService
    {
        private readonly string _file = "Data/leaves.json";

        private async Task<List<LeaveRequest>> ReadAsync()
        {
            if (!File.Exists(_file)) return new();
            var json = await File.ReadAllTextAsync(_file);
            return JsonSerializer.Deserialize<List<LeaveRequest>>(json) ?? new();
        }

        private async Task WriteAsync(List<LeaveRequest> data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_file, json);
        }

        public async Task<List<LeaveRequest>> GetAllAsync()
        {
            var allLeaves = await ReadAsync();
            return allLeaves
                .OrderByDescending(l => l.Status == LeaveStatus.Pending) // Pending first
                .ThenByDescending(l => l.LeaveDate)                     // Newest date first
                .ToList();
        }

        public async Task<List<LeaveRequest>> GetByEmployeeIdAsync(string employeeId)
        {
            var leaves = (await ReadAsync())
                .Where(l => l.EmployeeId == employeeId)
                .OrderByDescending(l => l.LeaveDate) // Newest first
                .ToList();

            return leaves;
        }

        public async Task AddAsync(LeaveRequest request)
        {
            var data = await ReadAsync();
            data.Add(request);
            await WriteAsync(data);
        }

        public async Task UpdateAsync(LeaveRequest request)
        {
            var data = await ReadAsync();
            var index = data.FindIndex(l => l.Id == request.Id);
            data[index] = request;
            await WriteAsync(data);
        }
    }
}
