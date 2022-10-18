namespace DependencyInversionInjection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] names = { "Alexa", "Léonie", "Noémie", "Jacques", "Matthieu", "Bartimée", "Marie", "François", "Claude", "Denis" };
            string[] phoneNumbers = { "00123", "00321", "00147", "00258", "00369", "00789", "00951", "00258", "00856", "00742" };
            int len = phoneNumbers.Length;

            using (var writer = new StreamWriter("rawData.txt"))
            {
                for (int i = 0; i < len; i++)
                {
                    writer.WriteLine($" {Guid.NewGuid()} / {names[i]} / {phoneNumbers[i]}");
                }
                
            }
            DataProvider data = new DataProvider("data/rawData.txt");
            DataParser parser = new DataParser();
            DataStorage storage = new DataStorage();

            TradeDataProcessor processor = new TradeDataProcessor(data, parser, storage);
            processor.ProcessTrade();
        }
    }
}