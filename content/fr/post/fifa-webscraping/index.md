---
title: Analyse de données de joueurs de football à l'aide du web scraping avec C# et HtmlAgilityPack
subtitle: Dans ce projet j'utilise l'environnement Jupyter Notebook avec le langage C# et la bibliothèque HtmlAgilityPack pour effectuer extraire des données du web (webscraping) du site Sofifa. Sofifa est une base de données en ligne de joueurs de football.

# Summary for listings and search engines
summary: Dans ce projet j'utilise l'environnement Jupyter Notebook avec le langage C# et la bibliothèque HtmlAgilityPack pour effectuer extraire des données du web (webscraping) du site Sofifa. Sofifa est une base de données en ligne de joueurs de football.

# Link this post with a project
projects: [webscraping-cs]

# Date published
date: '2024-04-27T00:00:00Z'

# Date updated
lastmod: '2024-04-27T00:00:00Z'

# Is this an unpublished draft?
draft: false

# Show this page in the Featured widget?
featured: true

# Featured image
# Place an image named `featured.jpg/png` in this page's folder and customize its options here.
image:
  caption: ''
  focal_point: ''
  placement: 2
  preview_only: false

authors:
  - admin

tags: [dotnet, data]

categories: [C#, webscraping]
---

# Analyse de données de joueurs de football à l'aide du web scraping avec C# et HtmlAgilityPack

Dans ce projet j'utilise l'environnement Jupyter Notebook avec le langage C# et la bibliothèque HtmlAgilityPack pour effectuer extraire des données du web (webscraping) du site Sofifa. Sofifa est une base de données en ligne de joueurs de football.

## Extraction des données
Nous commençons par l'importation des packages nécessaires et le chargement du site Sofifa à l'aide de HtmlAgilityPack. Ensuite, nous sélectionnons les nœuds HTML contenant les informations des joueurs à l'aide de requêtes XPath. Les données importantes telles que le nom, l'âge, la note globale, l'équipe et le salaire sont extraites en utilisant des méthodes spécifiques.

## Importation des packages

Nous commençons par importer HtmlAgilityPack pour le traitement HTML.


```C#
#r "nuget: HtmlAgilityPack, 1.11.60"
```


<div><div></div><div></div><div><strong>Installed Packages</strong><ul><li><span>HtmlAgilityPack, 1.11.60</span></li></ul></div></div>



```C#
using HtmlAgilityPack;
```

## Chargement du site et extraction des données

Nous chargeons le site Sofifa et sélectionnons les nœuds HTML contenant les informations des joueurs à l'aide de XPath.


```C#
string sofifa = "https://sofifa.com/players";
```


```C#
HtmlWeb web = new HtmlWeb();
var htmlDoc = web.Load(sofifa);
```

1. `string sofifa = "https://sofifa.com/players";`

   - Cette ligne crée une variable `sofifa` et lui assigne la valeur `"https://sofifa.com/players"`. Cette URL représente la page web de Sofifa où se trouvent les informations sur les joueurs de football.

2. `HtmlWeb web = new HtmlWeb();`

   - Cette ligne crée une nouvelle instance de la classe `HtmlWeb`. Cette classe est fournie par la bibliothèque HtmlAgilityPack et est utilisée pour télécharger le contenu HTML d'une URL spécifiée.

3. `var htmlDoc = web.Load(sofifa);`

   - Cette ligne utilise l'instance de `HtmlWeb` créée précédemment pour charger le contenu HTML de l'URL spécifiée dans la variable `sofifa`. Le contenu HTML est téléchargé depuis le site web et stocké dans la variable `htmlDoc`. Cette variable contiendra ensuite le document HTML de la page Sofifa, ce qui permettra d'extraire les informations des joueurs à partir de celui-ci.

Nous utilisons XPath pour sélectionner les noms des joueurs dans la page HTML et les affichons.


```C#
htmlDoc.DocumentNode
    .SelectNodes("//a[starts-with(@href, '/player/')]")
    .Select((node, index) => (node, index))
    .Take(15)
    .ToList()
    .ForEach(res => Console.WriteLine($"{res.index} - {res.node.InnerText}"))
```

    0 - Random
    1 - M. Wieffer
    2 - Lucas Paquetá
    3 - W. Odobert
    4 - M. Salah
    5 - C. Palmer
    6 - M. O'Riley
    7 - A. Isak
    8 - G. Rodríguez
    9 - W. Zaïre-Emery
    10 - V. Gyökeres
    11 - A. Güler
    12 - K. Mainoo
    13 - K. Havertz
    14 - J. Zirkzee


Il est relativement simple avec XPATH de parcourir toute la page web et sélectionner le contenu texte d'une balise web. C'est ce que nous venons de faire pour extraire les noms des joueurs. Toutefois nous aimerons récupérer d'autres informations concernant chaque joueur. 

Pour cela, regardons comment la page HTML est structurée : 

![](featured.PNG)


```C#

```

Je veux extraire pour chaque joueur : nom, l'âge, la note globale, l'équipe et le salaire.
La strucure HTML est ainsi constituée :  

- Toutes les données des joueurs sont groupées dans une balise **table** 
- Le contenu du tableau se trouve dans la sous-balise **tbody**
- Les lignes pour chaque joueur apparaissent dans les balises **tr**
- Le contenu de chaque colonne est dans la balise **td**
  
Les données telles que l'âge et le salaire sont faciles à récupérer car nous voyons qu'elles ont explicitement des propriétés data-col définies au niveau des td. 

La manière la plus sûre de récupérer les données est de procéder à l'extraction ligne par ligne, c'est à dire de prendre pour point d'entrée chaque balise **tr**.   
Pour ce faire, nous allons dans un premier sélectionner le noeud tbody.


```C#
var tbody = htmlDoc.DocumentNode.SelectSingleNode("//tbody");
```

A l'aide du XPATH nous sélectionnons entièrement le noeud **tbody**.

Comptons le nombre de balises tr se trouvant dans ce noeud


```C#
tbody.SelectNodes("//tr").Count()
```




<div class="dni-plaintext"><pre>61</pre></div><style>
.dni-code-hint {
    font-style: italic;
    overflow: hidden;
    white-space: nowrap;
}
.dni-treeview {
    white-space: nowrap;
}
.dni-treeview td {
    vertical-align: top;
    text-align: start;
}
details.dni-treeview {
    padding-left: 1em;
}
table td {
    text-align: start;
}
table tr { 
    vertical-align: top; 
    margin: 0em 0px;
}
table tr td pre 
{ 
    vertical-align: top !important; 
    margin: 0em 0px !important;
} 
table th {
    text-align: start;
}
</style>



Il y a 61 lignes dans la page que nous explorons. 
Affichons un aperçu du contenu HTML des deux premières lignes.  


```C#
tbody.SelectNodes("//tr")
    .Select(x => x.InnerHtml)
    .Take(2)
    .ToList()
    .ForEach(x => Console.WriteLine($"-- {x}"))
```

    -- 
    <th class="a1"></th>
    <th class="s20">Name</th><th class="d2"><a rel="nofollow" href="/players?col=ae&sort=asc" data-tippy-top="" data-tippy-content="Age"><span class="sorter"></span>Age</a></th><th class="d2"><a rel="nofollow" href="/players?col=oa&sort=desc" data-tippy-top="" data-tippy-content="Overall rating"><span class="sorter"></span>Overall rating</a></th><th class="d2"><a rel="nofollow" href="/players?col=pt&sort=desc" data-tippy-top="" data-tippy-content="Potential"><span class="sorter"></span>Potential</a></th><th class="s20"><a rel="nofollow" href="/players?col=tm&sort=asc" data-tippy-top="" data-tippy-content="Team & Contract"><span class="sorter"></span>Team & Contract</a></th>
    <th class="d6"><a rel="nofollow" href="/players?col=vl&sort=asc" data-tippy-top="" data-tippy-content="Value"><span class="sorter"></span>Value</a></th><th class="d6"><a rel="nofollow" href="/players?col=wg&sort=asc" data-tippy-top="" data-tippy-content="Wage"><span class="sorter"></span>Wage</a></th><th class="d5"><a rel="nofollow" href="/players?col=tt&sort=desc" data-tippy-top="" data-tippy-content="Total stats"><span class="sorter"></span>Total stats</a></th><th></th>
    
    -- 
    <td class="a1"><figure class="avatar">
    <img alt="" data-src="https://cdn.sofifa.net/players/248/793/24_60.png" data-srcset="https://cdn.sofifa.net/players/248/793/24_120.png 2x, https://cdn.sofifa.net/players/248/793/24_180.png 3x" alt="" src="https://cdn.sofifa.net/player_0.svg" data-root="https://cdn.sofifa.net/" data-type="player" id="248793" class="player-check"></figure></td>
    <td>
    <a href="/player/248793/mats-wieffer/240038/" data-tippy-top="" data-tippy-content="Mats Wieffer">M. Wieffer</a><div><img title="Netherlands" alt="" src="https://cdn.sofifa.net/pixel.gif" data-src="https://cdn.sofifa.net/flags/nl.png" data-srcset="https://cdn.sofifa.net/flags/nl@2x.png 2x, https://cdn.sofifa.net/flags/nl@3x.png 3x" class="flag" width="21" height="16"> <a rel="nofollow" href="/players?pn=10"><span class="pos pos10">CDM</span></a> <a rel="nofollow" href="/players?pn=14"><span class="pos pos14">CM</span></a></div>
    </td><td class="d2" data-col="ae">23</td><td class="d2" data-col="oa"><em title="78">78</em></td><td class="d2" data-col="pt"><em title="84">84</em></td><td class="">
    <figure class="avatar small transparent">
    <img alt="" class="team" data-src="https://cdn.sofifa.net/meta/team/73/30.png" data-srcset="https://cdn.sofifa.net/meta/team/73/60.png 2x, https://cdn.sofifa.net/meta/team/73/90.png 3x" src="https://cdn.sofifa.net/empty.svg" data-root="https://cdn.sofifa.net/" data-type="team">
    </figure>
    <a href="/team/246/feyenoord/">Feyenoord</a><div class="sub">
    2022 ~ 2027</div>
    </td><td class="d6" data-col="vl">€20.5M</td><td class="d6" data-col="wg">€15K</td><td class="d5" data-col="tt"><em>2050</em></td><td></td>
    


La première ligne ne contient pas les données des joueurs mais uniquement l'entête des données.  
Nous prenons en compte cette information lorsque nous allons extraire le contenu des données. Nous sauterons la première ligne. 

### Définir un enregistrement (record) pour représenter une ligne 

Nous définissons une classe `Player` pour stocker les informations de chaque joueur. Ensuite, nous créons une méthode `ExtractPlayer` pour extraire les données pertinentes telles que le nom, l'âge, la note globale, l'équipe et le salaire.


```C#
record Player(string Name, 
              int Age, 
              int? OverallRating, 
              string TeamContract, 
              double? Wage);
```


```C#
Player ExtractPlayer(HtmlNode node)
{
    string name = node.SelectSingleNode(".//a[starts-with(@href, '/player/')]").InnerText;
    string extractedAge = node.SelectSingleNode(".//td[@data-col='ae']").InnerText;
    int.TryParse(extractedAge, out int age);
    string extractedOverall = node.SelectSingleNode(".//td[@data-col='oa']").InnerText;
    int.TryParse(extractedOverall, out int overall);
    string contract = node.SelectSingleNode(".//a[starts-with(@href, '/team/')]").InnerText;
    string extractedWage = node.SelectSingleNode(".//td[@data-col='wg']").InnerText;
    extractedWage = new string(extractedWage.Where(c=> (Char.IsDigit(c) || c=='.'|| c==',')).ToArray());

    double.TryParse(extractedWage, out double wage);
    
    var player = new Player(name, age, overall, contract, wage);

    return player;
}
```

1. `record Player(string Name, int Age, int OverallRating, string TeamContract, double? Wage);`

   - Cette ligne définit un enregistrement (record) nommé `Player`, qui représente un joueur de football. Il a cinq propriétés : `Name`, `Age`, `OverallRating`, `TeamContract` et `Wage`. Ces propriétés correspondent aux informations que nous extrayons pour chaque joueur.

2. `Player ExtractPlayer(HtmlNode node)`

   - Cette ligne définit une méthode appelée `ExtractPlayer`, qui prend en paramètre un nœud HTML (`HtmlNode`). Cette méthode extrait les informations d'un joueur à partir d'un nœud HTML et les retourne sous forme d'un objet `Player`.

3. `string name = node.SelectSingleNode(".//a[starts-with(@href, '/player/')]").InnerText;`

   - Cette ligne extrait le nom du joueur en recherchant une balise `<a>` qui commence par l'attribut `href` contenant `'/player/'` à l'intérieur du nœud HTML donné, puis récupère son texte interne à l'aide de la propriété `InnerText`.

4. `string extractedAge = node.SelectSingleNode(".//td[@data-col='ae']").InnerText;`

   - Cette ligne extrait l'âge du joueur en recherchant une balise `<td>` avec l'attribut `data-col` égal à `'ae'` à l'intérieur du nœud HTML donné, puis récupère son texte interne à l'aide de la propriété `InnerText`.

5. `int.TryParse(extractedAge, out int age);`

   - Cette ligne tente de convertir la chaîne `extractedAge` en un entier (`int`). Si la conversion réussit, elle stocke la valeur dans la variable `age`.

6. `string extractedOverall = node.SelectSingleNode(".//td[@data-col='oa']").InnerText;`

   - Cette ligne extrait la note globale du joueur de la même manière que l'âge.

7. `int.TryParse(extractedOverall, out int overall);`

   - Cette ligne tente de convertir la chaîne `extractedOverall` en un entier (`int`). Si la conversion réussit, elle stocke la valeur dans la variable `overall`.

8. `string contract = node.SelectSingleNode(".//a[starts-with(@href, '/team/')]").InnerText;`

   - Cette ligne extrait le contrat de l'équipe du joueur en recherchant une balise `<a>` qui commence par l'attribut `href` contenant `'/team/'` à l'intérieur du nœud HTML donné, puis récupère son texte interne à l'aide de la propriété `InnerText`.

9. `string extractedWage = node.SelectSingleNode(".//td[@data-col='wg']").InnerText;`

   - Cette ligne extrait le salaire du joueur de la même manière que l'âge et la note globale.

10. `extractedWage = new string(extractedWage.Where(c=> (Char.IsDigit(c) || c=='.'|| c==',')).ToArray());`

    - Cette ligne filtre la chaîne `extractedWage` pour ne conserver que les caractères numériques, le point (`.`) et la virgule (`,`). Cela est nécessaire car le salaire peut contenir des symboles de devise ou d'autres caractères non numériques.

11. `double.TryParse(extractedWage, out double wage);`

    - Cette ligne tente de convertir la chaîne `extractedWage` en un nombre à virgule flottante (`double`). Si la conversion réussit, elle stocke la valeur dans la variable `wage`.

12. `var player = new Player(name, age, overall, contract, wage);`

    - Cette ligne crée une nouvelle instance de la classe `Player` avec les informations extraites du joueur.

13. `return player;`

    - Cette ligne retourne l'objet `Player` créé à partir des informations extraites.

En combinant toutes ces lignes, la méthode `ExtractPlayer` prend un nœud HTML représentant un joueur de football, extrait ses informations et retourne un objet `Player` contenant ces informations.



```C#
var players =  tbody
    .SelectNodes("//tr")
    .Skip(1)
    .Select(x => ExtractPlayer(x));
```

## Afficher les informations des joueurs


```C#
void Display(Player p) =>
    Console.WriteLine($@"
    Name: {p.Name} 
    Age: {p.Age}
    Overall Rating: {p.OverallRating}
    Team/Contract: {p.TeamContract}
    Wage: {p.Wage}");
```


```C#
players
    .Take(5)
    .ToList()
    .ForEach(p => Display(p))
```

    
        Name: M. Wieffer 
        Age: 23
        Overall Rating: 78
        Team/Contract: Feyenoord
        Wage: 15
    
        Name: Lucas Paquetá 
        Age: 25
        Overall Rating: 82
        Team/Contract: West Ham United
        Wage: 85
    
        Name: W. Odobert 
        Age: 18
        Overall Rating: 0
        Team/Contract: Burnley
        Wage: 10
    
        Name: M. Salah 
        Age: 31
        Overall Rating: 89
        Team/Contract: Liverpool
        Wage: 260
    
        Name: C. Palmer 
        Age: 21
        Overall Rating: 0
        Team/Contract: Chelsea
        Wage: 76


## Conclusion
Ce projet démontre comment utiliser le web scraping avec C# et HtmlAgilityPack pour extraire et analyser des données à partir de sites Web. Dans cet exemple, nous avons extrait les informations des joueurs de football à partir de Sofifa, en mettant en évidence le processus de collecte, de traitement et d'affichage des données. Cette approche peut être étendue pour analyser d'autres types de données sur le web, offrant ainsi un large éventail d'applications dans le domaine de l'analyse de données et de la science des données.

## Réutiliser le notebook Jupyter

J'ai rendu disponible le notebook Jupyter qui a servi dans cet article. 
Vous pouvez le trouver [**ici**](https://github.com/agailloty/cshap-notebooks/blob/main/fifa-webscraping.ipynb).