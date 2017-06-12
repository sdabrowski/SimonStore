using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimonStore.Controllers
{
    public class ERPController : Controller
    {
        // GET: ERP
        /// <summary>
        /// Calls the "sp_GetStores" stored procedure, and displays the results in the view.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string connectionString = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = SimonStore; Integrated Security = True; Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString);

            connection.Open();

            System.Data.SqlClient.SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_GetStores";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();
            List<Models.ERPStoreModel> stores = new List<Models.ERPStoreModel>();

            while (reader.Read())
            {
                Models.ERPStoreModel store = new Models.ERPStoreModel();
                //the reader is "untyped" - I need to look at the
                //stored procedure definition to see that the 1st column returned is an int
                store.ID = reader.GetInt32(0);
                store.Name = reader.GetString(1);
                //Adds the newly created store to the list
                stores.Add(store);
            }

            connection.Close();

            return View(stores);
        }

        /// <summary>
        /// Calls the "sp_GetStoreRevenue" stored procedure
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Revenues(int? id)
        {
            string connectionString = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = SimonsStore; Integrated Security = True; Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString);

            connection.Open();

            System.Data.SqlClient.SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_GetStoreRevenue";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            System.Data.SqlClient.SqlParameter parameter = command.CreateParameter();
            parameter.Direction = System.Data.ParameterDirection.Input;
            parameter.ParameterName = "@store";
            parameter.SqlDbType = System.Data.SqlDbType.Int;
            parameter.SqlValue = id;

            command.Parameters.Add(parameter);

            List<Models.ERPRevenueModel> revenues = new List<Models.ERPRevenueModel>();
            System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Models.ERPRevenueModel revenue = new Models.ERPRevenueModel();
                revenue.Revenue = reader.GetDecimal(0);
                revenue.Date = new DateTime(reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3));

                revenues.Add(revenue);
            }
            connection.Close();

            return View(revenues);
        }

        /// <summary>
        /// Calls the "sp_EmployeeSchedule" stored procedure
        /// </summary>
        /// <param name="id"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public ActionResult Schedule(int? id, DateTime? day)
        {
            if (!day.HasValue)
            {
                day = DateTime.Today;
            }
            //TODO: call the sp_GetEmployeesWorking procedure

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString);

            connection.Open();

            System.Data.SqlClient.SqlCommand command = connection.CreateCommand();
            command.CommandText = "sp_GetStoreRevenue";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            System.Data.SqlClient.SqlParameter parameter = command.CreateParameter();
            parameter.Direction = System.Data.ParameterDirection.Input;
            parameter.ParameterName = "@store";
            parameter.SqlDbType = System.Data.SqlDbType.Int;
            parameter.SqlValue = id;

            command.Parameters.Add(parameter);

            List<Models.ERPScheduleModel> schedules = new List<Models.ERPScheduleModel>();
            System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Models.ERPScheduleModel schedule = new Models.ERPScheduleModel();
                schedule.EmployeeName = reader.GetString(reader.GetOrdinal("Name"));
                schedule.StartTime = reader.GetDateTime(1);
                schedule.EndTime = reader.GetDateTime(2);

                schedules.Add(schedule);
            }
            connection.Close();

            return View(schedules);
        }

        public ActionResult Add()
        {
            //TODO: Create a view that includes a "Form" and use the HTML helper to render editable fields
            return View();
        }

        [HttpPost]
        public ActionResult Add(Models.ERPStoreModel model)
        {
            //TODO: Insert the data from the model into the database using the "ExecuteNonQuery" method on SQLCommand
            return RedirectToAction("Index");
        }
    }
}