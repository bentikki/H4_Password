using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using ConsoleVersion.Entities;
using ConsoleVersion.Helpers;
using ConsoleVersion.Service;
using WebVersion.Service;
using System.Data.SqlClient;
using System.Data;

namespace WebVersion.Controllers
{

    public class ItemController : Controller
    {

        public ActionResult Search(List<Item> displayList = null)
        {
            SearchViewModel vm = new SearchViewModel();

            if (displayList != null)
                vm.Items = displayList;

            return View(vm);
        }

        [HttpPost]
        public ActionResult SearchItem(string searchString)
        {
            SearchViewModel vm = new SearchViewModel();

            using (SqlConnection connection = new SqlConnection(ConnectionStringHelper.GetDBConnectionString()))
            {
                string sql = $"SELECT * FROM Items WHERE Active = 1 and ItemName LIKE '%{ searchString }%'";

                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();


                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Item item = new Item();

                        item.ItemName = reader["ItemName"].ToString();
                        item.ItemPrice = Convert.ToDecimal(reader["ItemPrice"]);

                        vm.Items.Add(item);
                    }
                }
                connection.Close();
            }


            return View("Search", vm);
        }

        //[HttpPost]
        //public ActionResult SearchItem(string searchString)
        //{
        //    SearchViewModel vm = new SearchViewModel();

        //    using (SqlConnection connection = new SqlConnection(ConnectionStringHelper.GetDBConnectionString()))
        //    {
        //        connection.Open();
        //        SqlCommand command = new SqlCommand(null, connection);


        //        command.CommandText = $"SELECT * FROM Items WHERE Active = 1 and ItemName LIKE @searchString";

        //        // Add Param
        //        SqlParameter searchParam = new SqlParameter("@searchString", SqlDbType.Text, 100);
        //        searchParam.Value = "%" + searchString + "%";
        //        command.Parameters.Add(searchParam);

        //        command.Prepare();

        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            if (reader.HasRows)
        //            {
        //                while (reader.Read())
        //                {
        //                    Item item = new Item();

        //                    item.ItemName = reader["ItemName"].ToString();
        //                    item.ItemPrice = Convert.ToDecimal(reader["ItemPrice"]);

        //                    vm.Items.Add(item);
        //                }
        //            }
        //        }
                

        //    }


        //    return View("Search", vm);
        //}




    }
}