Week 01 Assignment: Build .NET Applications with C#
1. Evidence of Existing and Additional Pizzas

The ContosoPizza API originally contained two pizzas:

ID 1: Classic Italian
ID 2: Veggie

I added an additional pizza to the seed data in Services/PizzaService.cs:

ID 3: BBQ Chicken

I verified the pizzas by sending a GET request to retrieve all pizzas.

GET Request
http
GET http://localhost:5095/pizza/
Accept: application/json

Response – 200 OK
json
[
  {
    "id": 1,
    "name": "Classic Italian",
    "isGlutenFree": false
  },
  {
    "id": 2,
    "name": "Veggie",
    "isGlutenFree": true
  },
  {
    "id": 3,
    "name": "BBQ Chicken",
    "isGlutenFree": false
  },
  {
    "id": 5,
    "name": "Hawaii",
    "isGlutenFree": false
  },
  {
    "id": 6,
    "name": "Hawaii",
    "isGlutenFree": false
  }
]


This confirms that the original pizzas and my additional seeded pizza are available through the API.

2. CRUD Operations – Request and Response 

All four CRUD operations were tested using the REST Client extension in Visual Studio Code.

A. GET – Retrieve All Pizzas

Request:

http
GET http://localhost:5095/pizza/
Accept: application/json


Status Code: 200 OK

Result: Successfully retrieved all available pizzas.

B. POST – Create a New Pizza

Request:

http
POST http://localhost:5095/pizza/
Content-Type: application/json

{
    "name": "Hawaii",
    "isGlutenFree": false
}


Status Code: 201 Created

Response:

json
{
  "id": 6,
  "name": "Hawaii",
  "isGlutenFree": false
}


Result: Successfully created a new pizza with ID 6.

C. PUT – Update an Existing Pizza

Request:

http
PUT http://localhost:5095/pizza/6
Content-Type: application/json

{
    "id": 6,
    "name": "Hawaiian Deluxe",
    "isGlutenFree": false
}


Status Code: 204 No Content

Result: Successfully updated pizza ID 6.

D. GET – Verify Updated Pizza

Request:

http
GET http://localhost:5095/pizza/6
Accept: application/json


Status Code: 200 OK

Response:

json
{
  "id": 6,
  "name": "Hawaiian Deluxe",
  "isGlutenFree": false
}


Result: Confirmed that the pizza was successfully updated.

E. DELETE – Delete a Pizza

Request:

http
DELETE http://localhost:5095/pizza/6


Status Code: 204 No Content

Result: Successfully deleted pizza ID 6.

F. GET – Verify Pizza Was Deleted

Request:

http
GET http://localhost:5095/pizza/6
Accept: application/json


Status Code: 404 Not Found

Response:

json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Not Found",
  "status": 404
}


Result: Confirmed that the deleted pizza no longer exists.

3. Sales Summary Function – Work with Files and Directories in a .NET App

The following is my working sales summary function from the .NET module.

It reads the sales JSON files, calculates the total sales, formats the amounts as currency, and generates a SalesSummary.txt report.

GenerateSalesReport Function
csharp
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

Sales Data Record
csharp
record SalesData(decimal Total);


Result: The function successfully generates a sales summary report containing the overall sales total and individual sales totals for each file.


Sales Summary
----------------------------

Total Sales: $2,012.20

Details:
  sales.json: $88.88
  201\sales.json: $501.22
  202\sales.json: $1,234.22
  203\sales.json: $99.00
  204\sales.json: $88.88
