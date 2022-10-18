---
title: L'inversion de dépendance en C# (ID de SOLID)
date: 2022-10-14
description: "L'inversion de dépendance est un des principes de SOLID et stipule qu'une classe ne doit pas dépendre de l'implémentation de la classe dépendante mais plutôt sur une abastraction de celle-ci."
---

# Exemple

## Implémentation sans inversion de dépendance

Pour mieux saisir le principe de l'inversion de dépendance, il me semble pertinent d'étudier un exemple pratique. 
Supposons que nous ayons une classe nommée `TradeProcessor` qui lit des données à partir d'une source, retraite les données brute et les stocke quelque part.  

Cette classe dépend de trois autres classes notamment les classes:
-  `DataProvider` qui fournit à la classe un ensemble de méthodes pour récupérer les données brutes. 
- `DataParser` qui est une classe qui contient des méthodes pour traiter les données et les rendre prêtes à stocker. 
- `DataStorage` qui est une classe qui se charge de stocker les données dans une base de données et d'écrire des logs. 

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



```

## Implémentation avec inversion de dépendance

## Les interfaces en C#

## Créer des interfaces pour chacune des classes

Il est possible d'extraire une interface à partir d'une classe existante grâce à Visual Studio.

### Extraire l'interface d'une classe