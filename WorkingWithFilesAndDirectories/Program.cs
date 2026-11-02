
using Newtonsoft.Json;
using System.Text;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(
    Path.Combine(salesTotalDir, "totals.txt"),
    $"{salesTotal}{Environment.NewLine}"
);

// Generate the new sales summary report
GenerateSalesReport(salesFiles, storesDirectory);

Console.WriteLine("Sales summary report generated successfully!");


IEnumerable<string> FindFiles(string folderName)
{
    
    return Directory.EnumerateFiles(
        folderName,
        "sales.json",
        SearchOption.AllDirectories
    );
}


decimal CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    decimal salesTotal = 0;

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);

        SalesData? data =
            JsonConvert.DeserializeObject<SalesData?>(salesJson);

        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}


void GenerateSalesReport(
    IEnumerable<string> salesFiles,
    string storesDirectory)
{
    StringBuilder report = new StringBuilder();

    decimal totalSales = 0;

    report.AppendLine("Sales Summary");
    report.AppendLine("----------------------------");

    // Calculate and display each file's sales
    report.AppendLine();
    report.AppendLine("Details:");

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);

        SalesData? data =
            JsonConvert.DeserializeObject<SalesData?>(salesJson);

        decimal fileTotal = data?.Total ?? 0;

        totalSales += fileTotal;

        
        string fileName = Path.GetRelativePath(storesDirectory, file);

        report.AppendLine($"  {fileName}: {fileTotal:C}");
    }

    
    report.Insert(
        report.ToString().IndexOf("Details:"),
        $"Total Sales: {totalSales:C}\n\n"
    );

    // Save the report
    string reportPath = Path.Combine(
        storesDirectory,
        "SalesSummary.txt"
    );

    File.WriteAllText(reportPath, report.ToString());
}


record SalesData(decimal Total);
