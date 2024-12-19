using System;
using System.Data;
using System.Data.SqlClient;

namespace FirmCanctovaryApp
{
    class Program
    {
        private static readonly string connectionString = @"Server=StationeryCompany;Database=Firm_Canctovary;Trusted_Connection=True;";

        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("1.Connect to the database");
                Console.WriteLine("2.Disconnect from the database");
                Console.WriteLine("3.Display of all data");
                Console.WriteLine("4.Display of all types of stationery");
                Console.WriteLine("5.Display all sales managers");
                Console.WriteLine("6.Show stationery with the maximum number of units");
                Console.WriteLine("7.Show stationery with a minimum number of units");
                Console.WriteLine("8.Show stationery of a given type");
                Console.WriteLine("9.Show stationery sold by a specific manager");
                Console.WriteLine("10.Show stationery purchased by a certain company");
                Console.WriteLine("11.Show information about the last sale");
                Console.WriteLine("12.Show the average number of products for each type");
                Console.WriteLine("0.Exit!");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ConnectToDatabase();
                        break;
                    case "2":
                        DisconnectFromDatabase();
                        break;
                    case "3":
                        DisplayAllData();
                        break;
                    case "4":
                        DisplayItemTypes();
                        break;
                    case "5":
                        DisplayManagers();
                        break;
                    case "6":
                        DisplayMaxQuantityItems();
                        break;
                    case "7":
                        DisplayMinQuantityItems();
                        break;
                    case "8":
                        DisplayItemsByType();
                        break;
                    case "9":
                        DisplayItemsByManager();
                        break;
                    case "10":
                        DisplayItemsByBuyer();
                        break;
                    case "11":
                        DisplayLatestSale();
                        break;
                    case "12":
                        DisplayAverageQuantityByType();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Error");
                        break;
                }
            }
        }
        static void ConnectToDatabase()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connection successful!");
                    Console.WriteLine($"Info: {connection.DataSource}");
                    Console.WriteLine($"Info: {connection.Database}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(ex.Message);
            }
        }
        static void DisconnectFromDatabase()
        {
            Console.WriteLine("Connection done.");
        }
        static void DisplayAllData()
        {
            ExecuteAndDisplayQuery("SELECT * FROM Items");
        }
        static void DisplayItemTypes()
        {
            ExecuteAndDisplayQuery("SELECT DISTINCT ItemType FROM Items");
        }
        static void DisplayManagers()
        {
            ExecuteAndDisplayQuery("SELECT DISTINCT Manager FROM Sales");
        }
        static void DisplayMaxQuantityItems()
        {
            ExecuteAndDisplayQuery("SELECT TOP 1 * FROM Items ORDER BY Quantity DESC");
        }
        static void DisplayMinQuantityItems()
        {
            ExecuteAndDisplayQuery("SELECT TOP 1 * FROM Items ORDER BY Quantity ASC");
        }
        static void DisplayItemsByType()
        {
            Console.Write("Input type: ");
            string type = Console.ReadLine();
            ExecuteAndDisplayQuery($"SELECT * FROM Items WHERE ItemType = '{type}'");
        }
        static void DisplayItemsByManager()
        {
            Console.Write("Input meneger name: ");
            string manager = Console.ReadLine();
            ExecuteAndDisplayQuery($"SELECT * FROM Sales WHERE Manager = '{manager}'");
        }
        static void DisplayItemsByBuyer()
        {
            Console.Write("Input buyer name: ");
            string buyer = Console.ReadLine();
            ExecuteAndDisplayQuery($"SELECT * FROM Sales WHERE Buyer = '{buyer}'");
        }
        static void DisplayLatestSale()
        {
            ExecuteAndDisplayQuery("SELECT TOP 1 * FROM Sales ORDER BY SaleDate DESC");
        }
        static void DisplayAverageQuantityByType()
        {
            ExecuteAndDisplayQuery("SELECT ItemType, AVG(Quantity) AS AverageQuantity FROM Items GROUP BY ItemType");
        }
        static void ExecuteAndDisplayQuery(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                Console.Write($"{column.ColumnName}\t");
                            }
                            Console.WriteLine();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                foreach (var item in row.ItemArray)
                                {
                                    Console.Write($"{item}\t");
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}