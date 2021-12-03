using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Serialization.Json.Example7
{
    /// <summary>
    /// Application configuration
    /// </summary>
    internal class  Configuration
    {
        /// <summary>
        /// Application mode.
        /// </summary>
        public enum ApplicationMode
        {
            Default = 0,
            Silent = 1
        }
        
        /// <summary>
        /// Users server API URL.
        /// </summary>
        public string Api { get; set; }
        
        /// <summary>
        /// Application mode.
        /// </summary>
        public ApplicationMode Mode { get; set; }
        
        /// <summary>
        /// Inidicates that application is in <see cref="ApplicationMode.Silent"/> mode.
        /// </summary>
        public bool IsSilentMode => Mode == ApplicationMode.Silent;
    }
    
    /// <summary>
    /// User DTO.
    /// </summary>
    internal class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    /// <summary>
    /// Complete demo for : 
    /// <see cref="File"/>; 
    /// <see cref="Stream"/>; 
    /// <see cref="HttpClient"/>;
    /// <see cref="JsonSerializer"/>; 
    /// <see cref="JsonSerializerOptions"/>;
    /// <see cref="JsonStringEnumConverter"/>
    /// </summary>
    class Program
    {
        private const string ConfigFilePath = "config.json";

        /// <summary>
        /// Async entry point.
        /// </summary>
        /// <returns></returns>
        public static async Task Main()
        {
            // Reads configuration from file
            var config = await ReadConfigAsync(ConfigFilePath);

            // Creates HTTP client 
            using var client = new HttpClient() { BaseAddress = new Uri(config.Api) };

            // Gets the user information from server.
            var user = await GetUserAsync(client, "1");
            
            // Prints user details
            if (!config.IsSilentMode)
            {
                Console.WriteLine($"Id: {user.Id}");
                Console.WriteLine($"Name: {user.Name}");
                Console.WriteLine($"Username: {user.Username}");
                Console.WriteLine($"Email: {user.Email}");
            }

            // Post a new user.
            user.Id = 2;
            var success = await PostUserAsync(client, user);

            // Prints user POST result
            if (!config.IsSilentMode)
            {
                Console.WriteLine($"New user creation result: {(success ? "Success" : "Error")}");
            }
        }

        /// <summary>
        /// Reads configuration from file.
        /// </summary>
        private static async Task<Configuration> ReadConfigAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentOutOfRangeException(nameof(path), "Invalid path");

            // IMPORTANT : to work with enums as strings, need to use JsonStringEnumConverter.
            var options = new JsonSerializerOptions()
            {
                Converters = { new JsonStringEnumConverter() }
            };

            // Use file stream to deserializeconfiguration async
            // Alternative way is to use File.ReadAllTextAsync
            using var configStream = File.OpenRead(path);
            var config = await JsonSerializer.DeserializeAsync<Configuration>(configStream, options);
            return config;
        }

        /// <summary>
        /// Returns user from server.
        /// </summary>
        private static async Task<User> GetUserAsync(HttpClient client, string userId)
        {
            if (client is null) 
                throw new ArgumentNullException(nameof(client));

            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentOutOfRangeException(nameof(userId), "Invalid id");

            // Makes HTTP GET call to Users server and deserializes response.
            var user = await client.GetFromJsonAsync<User>($"users/{userId}");
            return user;
        }

        /// <summary>
        /// Sends user to server.
        /// </summary>
        private static async Task<bool> PostUserAsync(HttpClient client, User user)
        {
            if (client is null)
                throw new ArgumentNullException(nameof(client));

            if (user is null)
                throw new ArgumentNullException(nameof(user));

            // Serializes request and makes HTTP POST call to Users server.
            using var response = await client.PostAsJsonAsync("users", user);
            return response.IsSuccessStatusCode;
        }
    }
}
