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

    public class CommentController : Controller
    {

        public ActionResult ShowAll()
        {
            CommentViewModel vm = new CommentViewModel();

            using (SqlConnection connection = new SqlConnection(ConnectionStringHelper.GetDBConnectionString()))
            {
                string sql = $"SELECT * FROM Comments ORDER BY ID DESC";

                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Comment comment = new Comment();

                        comment.Content = reader["Content"].ToString();

                        vm.Comments.Add(comment);
                    }
                }
                connection.Close();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateComment(string commentContent)
        {

            using (SqlConnection connection = new SqlConnection(ConnectionStringHelper.GetDBConnectionString()))
            {

                connection.Open();
                SqlCommand command = new SqlCommand(null, connection);

                command.CommandText = "INSERT INTO Comments (Content) VALUES (@contentValue)";

                SqlParameter contentParam = new SqlParameter("@contentValue", SqlDbType.Text, 500);
                contentParam.Value = commentContent;
                command.Parameters.Add(contentParam);

                command.Prepare();
                command.ExecuteNonQuery();

                connection.Close();
            }

            return RedirectToAction("ShowAll");
        }

    }
}