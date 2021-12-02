using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using myDbApp;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            CrudOperation crud = new CrudOperation();

            do
            {
                int choice = DisplayMenu();

                switch (choice)
                {
                    case 1: // Get All Products
                        crud.GetAllProducts();
                        break;

                    case 2: // Get Product by ID
                        Console.Write("\nEnter Product ID: ");
                        int id = int.Parse(Console.ReadLine());
                        crud.GetProductById(id);
                        break;

                    case 3: // Insert Product
                        Console.Write("\nEnter Product Name: ");
                        string name = Console.ReadLine();

                        Console.Write("Enter Product Price: ");
                        int price = int.Parse(Console.ReadLine());

                        Console.Write("Enter Product Quantity: ");
                        short quantity = short.Parse(Console.ReadLine());

                        crud.InsertProduct(name, price, quantity);
                        break;

                    case 4: // Update Product
                        Console.Write("\nEnter Product Id: ");
                        id = int.Parse(Console.ReadLine());
                        crud.GetProductById(id);

                        Console.Write("\nEnter Product Name: ");
                        name = Console.ReadLine();

                        Console.Write("Enter Product Price: ");
                        price = int.Parse(Console.ReadLine());

                        Console.Write("Enter Product Quantity: ");
                        quantity = short.Parse(Console.ReadLine());

                        crud.UpdateProduct(id, name, price, quantity);
                        break;

                    case 5: // Delete Product
                        Console.Write("\nEnter Product Id: ");
                        id = int.Parse(Console.ReadLine());

                        crud.DeleteProduct(id);
                        break;

                    case 6: // Exit
                        Environment.Exit(0);
                        break;
                }
            } while (true);
        }

    static int DisplayMenu()
    {
        Console.WriteLine("\n\n+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+\n");
        Console.WriteLine("\t1 - Get all Products");
        Console.WriteLine("\t2 - Get Product by ID");
        Console.WriteLine("\t3 - Insert Product");
        Console.WriteLine("\t4 - Update Product");
        Console.WriteLine("\t5 - Delete Product");
        Console.WriteLine("\t6 - Exit");

        Console.Write("\nEnter your choice: ");
        return int.Parse(Console.ReadLine());
    }

    static string getConnectionString(string name)
        {
            ConfigurationBuilder builer = new ConfigurationBuilder();
            builer.SetBasePath(Directory.GetCurrentDirectory());
            builer.AddJsonFile("config.json");
            IConfiguration conf = builer.Build();
            return conf["ConnectionStrings:" + name];
        }
        static void PrintProducts()
        {
            string cs = getConnectionString("db");
            SqlConnection conn = new SqlConnection(cs);
            string query = "Select ProductID,ProductName from Products";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query,conn);
            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "Products");
            DataTable dataTable = dataSet.Tables["Products"];
            foreach(DataRow data in dataTable.Rows)
            {
                Console.WriteLine($"{data["ProductID"]} {data["ProductName"]}");
            }
        }
        static void countTable()
        {
            string cs = getConnectionString("db");
            string query = "Select Count(*) from Employees";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                int val = (int)cmd.ExecuteScalar();
                Console.WriteLine(val);
            }
        }
        static void printEmp()
        {
            string cs = getConnectionString("StandloneSQLServer");
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = "Select EmployeeID from Employees";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    int id = (int)rd["EmployeeID"];
                    Console.WriteLine(id);
                }
            }
        }
        static void GetEmp()
        {
            string cs = getConnectionString("StandloneSQLServer");
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string name = Console.ReadLine();
                string query = "Select EmployeeID from Employees where Firstname= @First";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("First", name);
                conn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    int id = (int)rd["EmployeeID"];
                    Console.WriteLine(id);
                }
            }
        }

    }
}
