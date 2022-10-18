namespace DependencyInversionInjection {

    public class TradeProcessor {

        private readonly DataProvider _data;
        private readonly DataParser _parser;
        private readonly DataStorage _storage;

        public TradeProcessor(DataProvider data, DataParser parser, DataStorage storage) 
        {
            _data = data;
            _parser = parser;
            _storage = storage;
        }

        public void ProcessTrade() {
            var lines = _data.GetAll();
            var trades = _parser.Parse(lines);
            _storage.Persist(trades);
        }
    }
}
