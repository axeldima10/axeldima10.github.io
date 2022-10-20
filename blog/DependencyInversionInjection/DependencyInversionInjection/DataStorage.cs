using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DependencyInversionInjection
{
    public class DataStorage
    {
        public void Persist(List<UserData> users)
        {
            var json = JsonSerializer.Serialize(users);
            File.WriteAllText("cleaned.json", json);
        }
    }
}
