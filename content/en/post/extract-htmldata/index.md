
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
This article provides an in-depth explanation of a C# code snippet that utilizes the HtmlAgilityPack library and Json serialization. The code focuses on extracting job information from HTML documents, processing and storing the data in different formats. Let's break down the code step by step to understand its functionality.

# Libraries and Namespace Imports:

```csharp
using HtmlAgilityPack;
using System.Text.Json;
using System.IO;
```
The code begins with the import of several namespaces required for the program's functionality. These include the HtmlAgilityPack for HTML document manipulation, System.Text.Json for JSON serialization, and System.IO for file input/output operations.

# Main Method:

```csharp
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
```

The entry point of the program is the Main method. It performs the following tasks:

## Declaration and Initialization:

- allJobs: A list that stores a collection of job cards. Each job card represents a list of JobCard objects.
- files: Retrieves the list of file paths within the "files" directory.

### File Processing Loop:
The code iterates through each file in the "files" directory.

- Loads the HTML document using HtmlDocument and the file path.
- Extracts job information using the ExtractJobInfo method.
- Appends the extracted job cards to the allJobs list.

Flattening Job Cards:

- Using LINQ's SelectMany, the code flattens the allJobs list into a single list of JobCard objects, stored in the res variable.
### Writing to CSV File:

- The File.WriteAllLines method is used to write the job information to a CSV file named "jobs.csv".
Each line in the file corresponds to a single job card, containing the title, company, location, and description.
### Writing to JSON File:

The Persist method is called to persist the job information to a JSON file named "jobs.json".
The Persist method serializes the job cards into a JSON string using the JsonSerializer class and writes it to the file.
ExtractJobInfo Method:
The ExtractJobInfo method receives an HtmlDocument as input and returns a list of JobCard objects. It performs the following tasks:

# Extracting Job Cards:

Utilizing XPath, the method selects all <div> elements with a class attribute starting with "job_seen_beacon".
The selected job card nodes are stored in the jobCards variable.
Job Card Extraction:

A list named jobInfos is created to store the extracted job information.
The method iterates through each job node in jobCards.
The ExtractInfo method is called to extract the title, company, location, and description from the job node.
The extracted information is used to create a new JobCard object, which is added to the jobInfos list.
Return Result:

The method returns the jobInfos list containing the extracted job cards.
ExtractInfo Method:
The ExtractInfo method receives an HtmlNode representing a job card and returns a JobCard object. It performs the following tasks:

Extraction Using XPath:

The method utilizes XPath to select specific elements within the job card node.
It extracts the job title, company name, location, and description using the SelectSingleNode method.
The extracted values are stored in respective string variables.
Creating a JobCard Object:

Using the extracted values, a new `JobCard` object is created by passing the title, company, location, and description as parameters.

Return Result:
The method returns the created JobCard object.
Persist Method:
The Persist method is responsible for persisting the job card data to a JSON file. It takes a file path and a list of JobCard objects as parameters. The method performs the following tasks:

JSON Encoding Configuration:

It creates an instance of JavaScriptEncoder by specifying the Unicode character ranges to include in the encoding. In this case, it includes the Basic Latin, Latin-1 Supplement, and General Punctuation ranges.
JSON Serialization Options:

The method creates a JsonSerializerOptions object and sets the WriteIndented property to true to format the JSON output with indentation.
It also sets the Encoder property of the options object to the previously created encoder instance.
JSON Serialization:

The JsonSerializer.Serialize method is used to convert the data list into a JSON string representation.
The method passes the data list and the options object to the Serialize method, which returns the JSON string.
Writing to File:

The JSON string is written to the specified file path using the File.WriteAllText method.
JobCard Class:
The JobCard class is a record that represents a job card with four properties: Title, Company, Location, and Description. It provides a concise way to define immutable data objects.

# Conclusion:
The provided C# code utilizes the HtmlAgilityPack library to extract job information from HTML documents and demonstrates the usage of Json serialization to store the data in both CSV and JSON formats. By leveraging XPath queries and LINQ operations, the code efficiently processes multiple HTML files, extracts relevant data, and persists it in different formats for further analysis or integration with other systems. This code serves as a practical example of how to leverage external libraries and serialization techniques to automate data extraction and transformation tasks.