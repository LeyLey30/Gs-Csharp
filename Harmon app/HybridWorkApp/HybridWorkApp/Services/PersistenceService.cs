using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using HybridWorkApp.Models;

namespace HybridWorkApp.Services
{
    public class PersistenceService
    {
        private readonly string _path;

        public PersistenceService()
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HybridWorkApp");
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            _path = Path.Combine(dir, "userdata.json");
        }

        public async Task SaveAsync(UserData data)
        {
            var opts = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(data, opts);
            await File.WriteAllTextAsync(_path, json);
        }

        public async Task<UserData?> LoadAsync()
        {
            if (!File.Exists(_path)) return null;
            var json = await File.ReadAllTextAsync(_path);
            return JsonSerializer.Deserialize<UserData>(json);
        }
    }
}
