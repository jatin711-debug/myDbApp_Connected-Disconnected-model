using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace myDbApp
{
    class CrudOperation
    {
        private SqlConnection sqlConnection;
        private SqlDataAdapter sqlDataAdapter;
        private SqlCommandBuilder sqlCommandBuilder;
        private DataSet dataSet;
        private DataTable dataTable;

        internal CrudOperation()
        {
            string cs = getConnectionString("db");
            string query = "Select ProductID,ProductName,UnitPrice,UnitsInStock from Products";
            sqlConnection = new SqlConnection(cs);
            sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
            sqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapter);
            dataSet = new DataSet();
            FillDataSet();
        }

        private void FillDataSet()
        {
            dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet, "Products");
            dataTable = dataSet.Tables["Products"];
        }

        internal void GetAllProducts()
        {
            foreach(DataRow dataRow in dataTable.Rows)
            {
                Console.WriteLine($"{dataRow["ProductID"]}  {dataRow["ProductName"]}");
            }
        }

        internal void GetProductById(int id)
        {
            DataRow dataRow = dataTable.Rows.Find(id);
            if(dataRow != null) Console.WriteLine($"{dataRow["ProductID"]}  {dataRow["ProductName"]}");
            else
            {
                Console.WriteLine("Invalid Product ID");
            }
        }

        internal void InsertProduct(string name,int price,int quantity)
        {
            DataRow row = dataTable.NewRow();
            row["ProductID"] = 0;
            row["ProductName"] = name;
            row["UnitPrice"] = price;
            row["UnitsInStock"] = quantity;
            dataTable.Rows.Add(row);
            sqlDataAdapter.InsertCommand = sqlCommandBuilder.GetInsertCommand();
            sqlDataAdapter.Update(dataTable);
            FillDataSet();
        }

        internal void UpdateProduct(int id,string name, int price, int quantity)
        {
            DataRow row = dataTable.Rows.Find(id);
            row["ProductName"] = name;
            row["UnitPrice"] = price;
            row["UnitsInStock"] = quantity;
            sqlDataAdapter.UpdateCommand = sqlCommandBuilder.GetUpdateCommand();
            sqlDataAdapter.Update(dataTable);
            FillDataSet();
        }

        internal void DeleteProduct(int id)
        {
            DataRow row = dataTable.Rows.Find(id);
            row.Delete();
            sqlDataAdapter.DeleteCommand = sqlCommandBuilder.GetDeleteCommand();
            sqlDataAdapter.Update(dataTable);
            FillDataSet();
        }

        private string getConnectionString(string name)
        {
            ConfigurationBuilder builer = new ConfigurationBuilder();
            builer.SetBasePath(Directory.GetCurrentDirectory());
            builer.AddJsonFile("config.json");
            IConfiguration conf = builer.Build();
            return conf["ConnectionStrings:" + name];
        }
    }
}
