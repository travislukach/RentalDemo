using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RentalDemo.Models;
using RentalDemo.StaticOperations;

namespace RentalDemo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RenterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RenterController> _logger;
        SqlConnectionStringBuilder _sqlBuilder;


        public RenterController(ILogger<RenterController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;

            _sqlBuilder = new SqlConnectionStringBuilder();
            _sqlBuilder.ConnectionString = _configuration["ConnectionStrings:RentalDemo.dbo"];
        }

        [HttpGet(Name = "Get All Renters")]
        public IEnumerable<Renter> Get()
        {
            List<Renter> renters = new List<Renter>();
            using (SqlConnection connection = new SqlConnection(_sqlBuilder.ConnectionString))
            {
                string sqlScript = "EXEC spSelectAllRenters";
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlScript, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Renter renter = new Renter()
                            {
                                RenterId = reader.GetInt32(0),
                                PropertyId = reader.GetInt32(1),
                                NumberOfOccupants = reader.GetInt32(2),
                                LastName = reader.GetString(3),
                                FirstName = reader.GetString(4),
                                PrimaryPhoneNumber = reader.GetString(5),
                                StartDate = reader.GetDateTime(6),
                                EndDate = reader.GetDateTime(7)
                            };
                            renters.Add(renter);
                        }
                    }
                }
            }
            return renters;
        }

        [HttpPut(Name = "Add Renter")]
        public void Put(Renter model)
        {


            StringSanitizer.SanitizeSqlString(model.FirstName);
            StringSanitizer.SanitizeSqlString(model.LastName);
            StringSanitizer.SanitizeSqlString(model.PrimaryPhoneNumber);
            

            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_sqlBuilder.ConnectionString))
                {
                    string sqlScript = "EXEC spAddRenter @PropertyId, @NumberOfOccupants, @LastName, @FirstName, @PrimaryPhoneNumber, @StartDate, @EndDate";
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlScript, connection))
                    {
                        command.Parameters.Add(new SqlParameter() { ParameterName = "PropertyId", SqlDbType = System.Data.SqlDbType.Int, Value = model.PropertyId });
                        command.Parameters.Add(new SqlParameter() { ParameterName = "NumberOfOccupants", SqlDbType = System.Data.SqlDbType.Int, Value = model.NumberOfOccupants });
                        command.Parameters.Add(new SqlParameter() { ParameterName = "LastName", SqlDbType = System.Data.SqlDbType.VarChar, Size = 50, Value = model.LastName });
                        command.Parameters.Add(new SqlParameter() { ParameterName = "FirstName", SqlDbType = System.Data.SqlDbType.VarChar, Size = 50, Value = model.FirstName });
                        command.Parameters.Add(new SqlParameter() { ParameterName = "PrimaryPhoneNumber", SqlDbType = System.Data.SqlDbType.VarChar, Size = 50, Value = model.PrimaryPhoneNumber });
                        command.Parameters.Add(new SqlParameter() { ParameterName = "StartDate", SqlDbType = System.Data.SqlDbType.DateTime, Value = model.StartDate });
                        command.Parameters.Add(new SqlParameter() { ParameterName = "EndDate", SqlDbType = System.Data.SqlDbType.DateTime, Value = model.EndDate });

                        string debug = command.ToString();

                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (SqlException e)
                        {
                            ModelState.AddModelError("SQL Execute", e.Message);
                        }
                    }
                }
            }
        }
    }
}
