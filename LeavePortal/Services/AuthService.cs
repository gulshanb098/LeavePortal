using LeavePortal.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LeavePortal.Services
{
    public class AuthService
    {
        private readonly List<User> users;

        public User? CurrentUser { get; private set; }

        public event Action? OnAuthStateChanged;

        public AuthService()
        {
            var jsonData = File.ReadAllText("Data/users.json");
            users = JsonSerializer.Deserialize<List<User>>(
                jsonData,
                new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() }
                })!;
        }

        public bool IsLoggedIn => CurrentUser != null;

        public async Task<bool> LoginAsync(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user is null)
            {
                Console.WriteLine("No Users");
                return false;
            }

            Console.WriteLine("User Found -> " + user.Username);
            CurrentUser = user;
            OnAuthStateChanged?.Invoke();
            return true;
        }

        public async Task LogoutAsync()
        {
            CurrentUser = null;
            OnAuthStateChanged?.Invoke();
        }
    }
}
