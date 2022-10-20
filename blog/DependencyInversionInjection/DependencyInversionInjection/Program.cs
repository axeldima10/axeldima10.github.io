namespace DependencyInversionInjection
{
    public class Program
    {
        static void Main(string[] args)
        {
            var data = new DataProvider("data/rawData.txt");
            var parser = new DataParser();
            var storage = new XMLDataStorage();

            var processor = new TradeDataProcessorDI(data, parser, storage);
            processor.ProcessTrade();
        }
    }
}