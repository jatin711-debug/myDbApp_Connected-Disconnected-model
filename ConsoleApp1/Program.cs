using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintProducts();
            //countTable();
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

        static string getConnectionString(string name)
        {
            ConfigurationBuilder builer = new ConfigurationBuilder();
            builer.SetBasePath(Directory.GetCurrentDirectory());
            builer.AddJsonFile("config.json");
            IConfiguration conf = builer.Build();
            return conf["ConnectionStrings:" + name];
        }
    }
}
