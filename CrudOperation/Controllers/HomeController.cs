using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CrudOperation.Models;

namespace CrudOperation.Controllers
{
    public class HomeController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyCon"].ConnectionString;

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(MyModel MM)
        {
            SqlConnection sqlcon = new SqlConnection(connectionString);
            string Query = "sp_InsertEmployeeDetails";
            SqlCommand sqlcmd = new SqlCommand(Query, sqlcon);
            sqlcmd.Parameters.AddWithValue("@Firstname", MM.Firstname);
            sqlcmd.Parameters.AddWithValue("@Middlename", MM.Middlename);
            sqlcmd.Parameters.AddWithValue("@Lastname", MM.Lastname);
            sqlcmd.Parameters.AddWithValue("@MobileNo", MM.MobileNo);
            sqlcmd.Parameters.AddWithValue("@Gender", MM.Gender);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcon.Open();
            sqlcmd.ExecuteNonQuery();
            sqlcon.Close();
            return RedirectToAction("EmployeeLogs", "Home");

        }
        [HttpGet]
        public ActionResult EmployeeLogs()
        {

            DataTable EmpList = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "EXEC sp_EmployeeDetailsLogs";
                SqlDataAdapter sqlRepairCodeMaster = new SqlDataAdapter(query, sqlCon);

                sqlRepairCodeMaster.Fill(EmpList);
                sqlCon.Close();
            }
            return View(EmpList);
        }

        [HttpGet]
        public ActionResult DeleteEmployee(string No)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("sp_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SrNo", No);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("EmployeeLogs", "Home");

        }

        [HttpGet]
        public ActionResult EditEmployee(string uniqueid)
        {
            DataTable MyTable = new DataTable();
            SqlConnection connection = new SqlConnection(connectionString);
            string MyQuery = "EXEC sp_GetEmployeeDetails'" + uniqueid + "'";
            connection.Open();
            SqlDataAdapter sd = new SqlDataAdapter(MyQuery, connection);
            sd.Fill(MyTable);
            connection.Close();
            return View(MyTable);
            
        }

        [HttpPost]
        public ActionResult EditEmployee(MyModel MM)
        {
            SqlConnection sqlcon = new SqlConnection(connectionString);
            string Query = "sp_Edit";
            SqlCommand sqlcmd = new SqlCommand(Query, sqlcon);
            sqlcmd.Parameters.AddWithValue("@SrNo", MM.SrNo);
            sqlcmd.Parameters.AddWithValue("@Firstname", MM.Firstname);
            sqlcmd.Parameters.AddWithValue("@Middlename", MM.Middlename);
            sqlcmd.Parameters.AddWithValue("@Lastname", MM.Lastname);
            sqlcmd.Parameters.AddWithValue("@MobileNo", MM.MobileNo);
            sqlcmd.Parameters.AddWithValue("@Gender", MM.Gender);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcon.Open();
            sqlcmd.ExecuteNonQuery();
            sqlcon.Close();
            return RedirectToAction("EmployeeLogs", "Home");
            
        }
    }
}
