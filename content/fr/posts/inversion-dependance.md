---
title: L'inversion de dépendance en C# (ID de SOLID)
date: 2022-10-14
description: "L'inversion de dépendance est un des principes de SOLID et stipule qu'une classe ne doit pas dépendre de l'implémentation de la classe dépendante mais plutôt sur une abastraction de celle-ci."
---

# Introduction & définition

L’inversion de dépendance est un des principes de l'approche SOLID. Elle traite de la question du couplage entre les différentes classes ou modules de votre application.
L'inversion de dépendance fait intervenir les notions de classe supérieure et de classes inférieures. La classe supérieure dépend d'une ou de plusieurs classes inférieures. La devise principale de l’inversion de dépendance est **Toute classe supérieure devrait toujours dépendre de l’abstraction de la classe inférieure plutôt que de l'implémentation de cette dernière**. 

Enoncer ce principe via sa devise ne suffit pas saisir entièrement ce que l'inversion de dépendance représente en réalité. Nous allons donc écrire du code C# pour voir l'application de ce principe en action. 

# Exemple

Pour mieux saisir le principe de l'inversion de dépendance, il me semble pertinent d'étudier un exemple pratique. 
Supposons que nous ayons une classe principale (supérieure) nommée `TradeProcessor`. Cette classe se charge de lire des données à partir d'une source, puis elle retraite les données brutes et les stocke quelque part.  

Elle dépend des trois classes suivantes :
-  `DataProvider` qui fournit à la classe un ensemble de méthodes pour récupérer les données brutes. 
- `DataParser` qui est une classe qui contient des méthodes pour traiter les données et les rendre prêtes à stocker. 
- `DataStorage` qui est une classe qui se charge de stocker les données dans une base de données et d'écrire des logs. 

## Implémentation sans inversion de dépendance

Nous présentons un à un le code de chacune de ces classes.

### La classe `TradeProcessor`

```csharp
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
```
### La classe `DataProvider`

```csharp
public class DataProvider
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
            while(_reader.ReadLine() != null)
            {
                rawData.Add(_reader.ReadLine());
            }

            return rawData;
        }
    }
```

### La classe `DataParser`

```csharp
public class DataParser
    {

        public List<UserData> Parse(List<string> data)
        {
            string[] currentData;
            UserData userData;
            List<UserData> result = new List<UserData>();
            foreach(string line in data)
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
```

### La classe `DataStorage`

```csharp
    public class DataStorage
    {
        public void Persist(List<UserData> users)
        {
            var json = JsonSerializer.Serialize(users);
            File.WriteAllText("cleaned.json", json);
        }
    }
```

## Implémentation avec inversion de dépendance

## Les interfaces en C#

## Créer des interfaces pour chacune des classes
### Extraire l'interface d'une classe

Il est possible d'extraire une interface à partir d'une classe existante grâce à Visual Studio.
Pour extraire une interface à partir d'une classe. Placez votre souris sur le nom de classe puis faites un clic droit. Un memu contextuel s'affiche. 

![](/blog/dependency-injection-inversion/extract-interface-1.png)

Ensuite sélectionner la première option *Actions rapides et factorisations...*.
![](/blog/dependency-injection-inversion/extract-interface-2.png)

Vous avez ensuite le choix des propriétés et méthodes publiques à inclure dans l'interface. Une fois que cela est fait cliquez sur OK pour que Visual Studio vous crée un fichier contenant l'interface. Vous pouvez choisir d'inclure le code de l'interface directement dans le fichier. 

![](/blog/dependency-injection-inversion/extract-interface-3.png)

## Implémentation avec l'inversion de la dépendance

L'idée ici c'est de faire en sorte que la classe `TradeProcessor` ne dépende pas des classes inférieures mais qu'elle dépende plutôt des abastractions de ces classes. 
L'élement que nous utilisons pour l'abastraction est justement une interface. 

Nous avons extrait les interfaces des classes inférieures, voilà donc comment nous implémentons la classe `TradeProcessor`, nous donnons le nom de `TradeProcessorDI`.

```csharp
    public class TradeProcessorDI
    {
        private readonly IDataProvider _data;
        private readonly IDataParser _parser;
        private readonly IDataStorage _storage;
        public TradeProcessorDI(IDataProvider data, IDataParser parser, IDataStorage storage)
        {
            _data = data;
            _parser = parser;
            _storage = storage;
        }

        public void ProcessTrade()
        {
            var lines = _data.GetAll();
            var trades = _parser.Parse(lines);
            _storage.Persist(trades);
        }
    }
```

Vous remarquerez que peu de choses diffèrent entre les classes `TradeDataProcessor` et `TradeDataProcessorDI`. En fait nous avons seulement remplacé les noms des classes inférieures par les noms de leurs interfaces. 
La classe fonctionne exactement pareil. 

La méthode `Main` de notre programme fonctionne exactement pareil avec les deux classes `TradeDataProcessor` et `TradeDataProcessorDI`

```csharp
    public class Program
    {
        static void Main(string[] args)
        {
            var data = new DataProvider("data/rawData.txt");
            var parser = new DataParser();
            var storage = new DataStorage();

            var processor = new TradeDataProcessorDI(data, parser, storage);
            processor.ProcessTrade();
        }
    }
```

```csharp
    public class Program
    {
        static void Main(string[] args)
        {
            var data = new DataProvider("data/rawData.txt");
            var parser = new DataParser();
            var storage = new DataStorage();

            var processor = new TradeDataProcessor(data, parser, storage);
            processor.ProcessTrade();
        }
    }
```

# Quel intérêt ?

Vous pouvez vous dire mais à quoi ça sert de faire toute cette bidouille pour arriver à la même solution qu'avec la première classe. 
En fait nous venons d'inverser les dépendances de la classe TradeDataProcessor. Cela signifie que la classe TradeDataProcessorDI peut prendre en argument dans son constructeur non seulement les classe `DataProvider`, `DataParser` et `DataStorage` mais aussi n'importe quelle classe qui implémente les interfaces `IDataProvider`, `IDataParser` et `IDataStorage`. 

```csharp
    public class XMLDataStorage : IDataStorage
    {
        public void Persist(List<UserData> users)
        {
            using (XmlWriter writer = XmlWriter.Create("users.xml"))
            {
                writer.WriteStartElement("UserData");
                foreach(UserData user in users)
                {
                    writer.WriteElementString("Id", user.ID.ToString());
                    writer.WriteElementString("Name", user.Name.ToString());
                    writer.WriteElementString("Phone", user.PhoneNumber.ToString());
                }
                writer.WriteEndElement();
                writer.Flush();
            }
        }
    }
```

```csharp
    public class Program
    {
        static void Main(string[] args)
        {
            var data = new DataProvider("data/rawData.txt");
            var parser = new DataParser();
            var storage = new XMLDataStorage(); // Nouvelle classe de stockage des données

            var processor = new TradeDataProcessorDI(data, parser, storage);
            processor.ProcessTrade();
        }
    }
```