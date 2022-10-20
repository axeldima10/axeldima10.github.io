using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversionInjection
{
    public class DataProvider : IDataProvider
    {
        private readonly StreamReader _reader;

        public DataProvider(string path)
        {
            _reader = new StreamReader(path);
        }

        // Read raw text from specified path

        public List<string> GetAll()
        {
            var rawData = new List<string>();
            while (_reader.ReadLine() != null)
            {
                rawData.Add(_reader.ReadLine());
            }

            return rawData;
        }
    }
}
