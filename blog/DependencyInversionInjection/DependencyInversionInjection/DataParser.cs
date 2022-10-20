using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversionInjection
{
    public class DataParser : IDataParser
    {

        public List<UserData> Parse(List<string> data)
        {
            string[] currentData;
            UserData userData;
            List<UserData> result = new List<UserData>();
            foreach (string line in data)
            {
                currentData = line.Split('/');
                userData = new UserData();
                userData.ID = Guid.Parse(currentData[0]);
                userData.Name = currentData[1];
                userData.PhoneNumber = currentData[2];

                result.Add(userData);
            }
            return result;
        }
    }

    public class UserData
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}
