using _04Laboratory_Matsevko.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _04Laboratory_Matsevko.Repositories
{
    class FileRepository
    {
        private static readonly string BaseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KMAUsersStorage");
        public FileRepository()
        {
            if (!Directory.Exists(BaseFolder))
                Directory.CreateDirectory(BaseFolder);
        }
        public bool Delete(string email)
        {
            try
            {
                string filePath = Path.Combine(BaseFolder, email);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return true;
            }
            catch (IOException e)
            {
                return false;
            }
        }
        public async Task AddOrUpdateAsync(Person obj)
        {
            var stringObj = JsonSerializer.Serialize(obj);
            using (StreamWriter sw = new StreamWriter(Path.Combine(BaseFolder, obj.Email), false))
            {
                await sw.WriteLineAsync(stringObj);
            }
        }
        public async Task<Person> GetAsync(string email)
        {
            string filePath = Path.Combine(BaseFolder, email);
            if (!File.Exists(filePath))
                return null;

            string stringObj = null;
            using (StreamReader sr = new StreamReader(filePath))
            {
                stringObj = await sr.ReadToEndAsync();
            }
            return JsonSerializer.Deserialize<Person>(stringObj);
        }
        public List<Person> GetAll()
        {
            var res = new List<Person>();
            foreach (var file in Directory.EnumerateFiles(BaseFolder))
            {
                string stringObj = null;
                using (StreamReader sr = new StreamReader(file))
                {
                    stringObj = sr.ReadToEnd();
                }
                res.Add(JsonSerializer.Deserialize<Person>(stringObj));
            }
            return res;
        }
    }
}

