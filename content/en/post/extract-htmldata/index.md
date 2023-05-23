
---
title: Extract data from HTML document using C#
subtitle: A brief introduction to the HtmlAgilityPack library

# Summary for listings and search engines
summary: This article provides an in-depth explanation of a C# code snippet that utilizes the HtmlAgilityPack library and Json serialization. The code focuses on extracting job information from HTML documents, processing and storing the data in different formats. Let's break down the code step by step to understand its functionality.

# Link this post with a project
projects: []

# Date published
date: '2023-05-10T00:00:00Z'

# Date updated
lastmod: '2023-05-10T00:00:00Z'

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

tags:
  - C#
  - .NET

categories:
  - webscraping
---
# Introduction:
In today's digital age, extracting and processing data from various sources is a common task. This is particularly true when it comes to job data, where information is scattered across multiple HTML documents. In this blog article, we will explore a C# code snippet that demonstrates how to efficiently extract job information from HTML files using the HtmlAgilityPack library. Additionally, we'll delve into the process of serializing the extracted data into both CSV and JSON formats for further analysis and integration with other systems.

# HTML Document Manipulation with HtmlAgilityPack:
The code snippet relies on the HtmlAgilityPack library, a powerful tool for parsing and manipulating HTML documents. Let's examine the key methods utilized in this code and their functionalities.

## ExtractJobInfo Method:
The ExtractJobInfo method is responsible for extracting job information from an HTML document. It takes an HtmlDocument object as input and returns a list of JobCard objects. Here's an overview of its functionality:

```csharp
private static IList<JobCard> ExtractJobInfo(HtmlDocument doc)
{
    var jobCards = doc.DocumentNode.SelectNodes("//div[starts-with(@class, 'job_seen_beacon')]");
    List<JobCard> jobInfos = new List<JobCard>();
    foreach (var job in jobCards)
        jobInfos.Add(ExtractInfo(job));
    return jobInfos;
}
```

Explanation:

The method utilizes XPath queries to select all <div> elements with a class attribute starting with "job_seen_beacon".
- The selected job card nodes are stored in the jobCards variable.
- It then iterates through each job node and calls the ExtractInfo method to extract the title, company, location, and description from the job node.
- The extracted information is used to create a new JobCard object, which is added to the jobInfos list.
- Finally, the method returns the list of job cards.

Here's the definition of the JobCard object :

```csharp
public record JobCard(string Title, 
                      string Company, 
                      string Location, 
                      string Description);
```

## ExtractInfo Method:
The ExtractInfo method is called by the ExtractJobInfo method to extract specific job details from a job card node. It takes an HtmlNode object as input and returns a JobCard object. Let's examine its implementation:

```csharp
private static JobCard ExtractInfo(HtmlNode node)
{
    string title = node.SelectSingleNode(".//span[starts-with(@id, 'jobTitle')]/text()").InnerText.Trim();
    string company = node.SelectSingleNode(".//span[@class='companyName']").InnerText.Trim();
    string location = node.SelectSingleNode(".//div[@class='companyLocation']").InnerText.Trim();
    string description = node.SelectSingleNode(".//div[@class='job-snippet']").InnerText.Trim();
    return new JobCard(title, company, location, description);
}
```

Explanation:

- The method uses XPath queries to extract specific elements within the job card node.
- It retrieves the job title by selecting the <span> element with an ID starting with "jobTitle" and extracting the inner text.
- The company name is obtained by selecting the <span> element with a class attribute equal to "companyName".
- The location is extracted by selecting the <div> element with a class attribute equal to "companyLocation".
- The description is obtained by selecting the <div> element with a class attribute equal to "job-snippet".
- Finally, a new JobCard object is created using the extracted information and returned.

# Serializing Job Data to CSV and JSON:
After extracting the job information from the HTML documents, the code snippet proceeds to serialize the data into both CSV and JSON formats. Let's explore the serialization process.

## Writing to CSV File:
The extracted job information is written to a CSV file named "jobs.csv" using the File.WriteAllLines method. Here's the code snippet that accomplishes this:

```csharp
File.WriteAllLines("jobs.csv", 
    res.Select(job => $"{job.Title},{job.Company},{job.Location},{job.Description}"));
```

Explanation:

- The `res` variable contains a flattened list of JobCard objects obtained by using LINQ's SelectMany on the allJobs list.
- The Select method is then used to transform each JobCard object into a string representation that includes the job title, company, location, and description.
- The resulting strings are written to the "jobs.csv" file using the File.WriteAllLines method, where each line corresponds to a single job entry in the CSV file.
Writing to JSON File:
- In addition to the CSV file, the code snippet also serializes the job data into a JSON file named "jobs.json". This process is performed in the Persist method. Let's take a closer look at its implementation:

```csharp
private static void Persist(string filepath, IList<JobCard> data) 
{
    var encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement, UnicodeRanges.GeneralPunctuation);
    var options = new JsonSerializerOptions { WriteIndented = true, Encoder = encoder };
    string jsonString = JsonSerializer.Serialize(data, options);
    File.WriteAllText(filepath, jsonString);
}
```

Explanation:

- The Persist method receives a file path and a list of JobCard objects as parameters.
It begins by creating a JavaScriptEncoder instance, specifying the Unicode character ranges to include in the encoding. In this case, it includes the Basic Latin, Latin-1 Supplement, and General Punctuation ranges.
- Next, a `JsonSerializerOptions` object is created, and the `WriteIndented` property is set to true to format the JSON output with indentation.
- The Encoder property of the options object is assigned the previously created encoder instance.
The `JsonSerializer.Serialize` method is called, passing the data list and the options object, which returns the JSON string representation of the job data.
- Finally, the JSON string is written to the specified file path using the `File.WriteAllText` method.

# Conclusion:
In this blog article, we explored a C# code snippet that showcases the usage of the HtmlAgilityPack library for HTML document manipulation and demonstrated how to extract job information from HTML files efficiently. Additionally, we examined the process of serializing the extracted data into both CSV and JSON formats for further analysis and integration. By leveraging XPath queries, LINQ operations, and the serialization capabilities of System.Text.Json, this code snippet provides a practical example of automating data extraction and transformation tasks. It equips developers with the knowledge and tools necessary to streamline job data processing workflows and opens doors to leveraging job data in a variety of applications.


# Annex

I have created a [Github repository](https://github.com/agailloty/webscraping-cs) for this project.

Full code snippet

```csharp
using HtmlAgilityPack;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

public class Program
{
    private static void Main(string[] args)
    {
        List<IList<JobCard>> allJobs = new();

        var files = Directory.GetFiles("files");
        foreach (var file in files)
        {
            var doc = new HtmlDocument();
            doc.Load(file, Encoding.UTF8, false);

            allJobs.Add(ExtractJobInfo(doc));
        }

        var res = allJobs.SelectMany(job => job).ToList();

        File.WriteAllLines("jobs.csv", 
                res.Select(job => $"{job.Title},{job.Company},{job.Location},{job.Description}"));

        Persist("jobs.json", allJobs.SelectMany(job => job).ToList());
    }

    private static IList<JobCard> ExtractJobInfo(HtmlDocument doc)
    {
        var jobCards = doc.DocumentNode
        .SelectNodes("//div[starts-with(@class, 'job_seen_beacon')]");

        List<JobCard> jobInfos = new();

        foreach(var job in jobCards)
            jobInfos.Add(ExtractInfo(job));

        return jobInfos;
    }

    private static JobCard ExtractInfo(HtmlNode node)
    {
        string title = node.SelectSingleNode(".//span[starts-with(@id, 'jobTitle')]/text()").InnerText.Trim();
        string company = node.SelectSingleNode(".//span[@class='companyName']").InnerText.Trim();
        string location = node.SelectSingleNode(".//div[@class='companyLocation']").InnerText.Trim();
        string description = node.SelectSingleNode(".//div[@class='job-snippet']").InnerText.Trim();
        return new JobCard(title, company, location, description);
    }

    private static void Persist(string filepath, IList<JobCard> data) 
    {
        var encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement, UnicodeRanges.GeneralPunctuation);
        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = encoder };
        string jsonString = JsonSerializer.Serialize(data, options);
        File.WriteAllText(filepath, jsonString);
    }
}

public record JobCard(string Title, string Company, string Location, string Description);

```