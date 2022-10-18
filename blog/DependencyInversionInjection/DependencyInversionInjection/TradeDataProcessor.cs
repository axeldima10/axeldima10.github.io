using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInversionInjection
{
    public class TradeDataProcessor
    {

        private readonly DataProvider _data;
        private readonly DataParser _parser;
        private readonly DataStorage _storage;

        public TradeDataProcessor(DataProvider data, DataParser parser, DataStorage storage)
        {
            _data = data;
            _parser = parser;
            _storage = storage;
        }

        public void ProcessTrade()
        {
            var lines = _data.GetAll();
            var trades = _parser.Parse(lines);
            //_storage.Persist(trades);
        }
    }
}
