using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RentalDemo.Models;
using RentalDemo.StaticOperations;
using System.Reflection;

namespace RentalDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PropertiesController> _logger;
        SqlConnectionStringBuilder _sqlBuilder;


        public PropertiesController(ILogger<PropertiesController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;

            _sqlBuilder = new SqlConnectionStringBuilder();
            _sqlBuilder.ConnectionString = _configuration["ConnectionStrings:RentalDemo.dbo"];
        }

        [HttpGet(Name = "GetAllProperties")]
        public IEnumerable<Property> Get()
        {
            List<Property> properties = new List<Property>();
            using (SqlConnection connection = new SqlConnection(_sqlBuilder.ConnectionString))
            {
                string sqlScript = "EXEC spSelectAllProperties";
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlScript, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Property property = new Property()
                            {
                                PropertyId = reader.GetInt32(0),
                                PropertyName = reader.GetString(1),
                                DailyRate = reader.GetDecimal(2),
                                MaximumOccupants = reader.GetInt32(3),
                                Location = reader.GetString(4),
                                NonStandardFeatures = reader.GetString(5)
                            };
                            properties.Add(property);
                        }
                    }
                }
            }
            return properties;
        }
        [HttpPut(Name = "Add Property")]
        public void Put(Property model)
        {
            if (model.DailyRate <= 0)
            {
                this.ModelState.AddModelError("model.DailyRate", "Rate Must Be Positive Value");
            }
            if (model.MaximumOccupants <= 0)
            {
                this.ModelState.AddModelError("model.MaximumOccupants", "Value Must Be Greater Than Zero (0)");
            }
            if (string.IsNullOrEmpty(model.PropertyName))
            {
                this.ModelState.AddModelError("model.PropertyName", "Name Cannot Be Empty");
            }
            if (string.IsNullOrEmpty(model.Location))
            {
                this.ModelState.AddModelError("model.Location", "Location Cannot Be Empty");
            }

            StringSanitizer.SanitizeSqlString(model.PropertyName);
            StringSanitizer.SanitizeSqlString(model.Location);
            if (!string.IsNullOrEmpty(model.NonStandardFeatures))
            {
                StringSanitizer.SanitizeSqlString(model.NonStandardFeatures);
            }

            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_sqlBuilder.ConnectionString))
                {
                    string sqlScript = "EXEC spAddProperty @name, @rate, @maxOccupant, @location, @feataures";
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlScript, connection))
                    {
                        command.Parameters.Add(new SqlParameter() { ParameterName = "name", SqlDbType = System.Data.SqlDbType.VarChar, Size = 50, Value = model.PropertyName});
                        command.Parameters.Add(new SqlParameter() { ParameterName = "rate", SqlDbType = System.Data.SqlDbType.Money, Value = model.DailyRate });
                        command.Parameters.Add(new SqlParameter() { ParameterName = "maxOccupant", SqlDbType = System.Data.SqlDbType.Int, Value = model.MaximumOccupants });
                        command.Parameters.Add(new SqlParameter() { ParameterName = "location", SqlDbType = System.Data.SqlDbType.VarChar, Size = 50, Value = model.Location });
                        command.Parameters.Add(new SqlParameter() { ParameterName = "feataures", SqlDbType = System.Data.SqlDbType.VarChar, Size = 2000, Value = model.NonStandardFeatures });

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

        [HttpGet("GetSearchByFeature/{searchFeatureString}", Name = "GetSearchByFeature")]
        public IEnumerable<Property> GetSearchByFeature(string searchFeatureString)
        {
            //validate
            if (searchFeatureString.Length > 2000)
            {
                ModelState.AddModelError("searchString", "Search String too Long");
                return null;
            }

            List<Property> properties = new List<Property>();
            using (SqlConnection connection = new SqlConnection(_sqlBuilder.ConnectionString))
            {
                string sqlScript = "EXEC spSearchPropertiesByFeature @feataure";
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlScript, connection))
                {
                    command.Parameters.Add(new SqlParameter() { ParameterName = "feataure", SqlDbType = System.Data.SqlDbType.VarChar, Size = 2000, Value = searchFeatureString });
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Property property = new Property()
                            {
                                PropertyId = reader.GetInt32(0),
                                PropertyName = reader.GetString(1),
                                DailyRate = reader.GetDecimal(2),
                                MaximumOccupants = reader.GetInt32(3),
                                Location = reader.GetString(4),
                                NonStandardFeatures = reader.GetString(5)
                            };
                            properties.Add(property);
                        }
                    }
                }
            }
            return properties;
        }

        [HttpGet("GetSearchByLocation/{searchLocationString}", Name = "GetSearchByLocation")]
        public IEnumerable<Property> GetSearchByLocation(string searchLocationString)
        {
            //validate
            if (searchLocationString.Length > 50)
            {
                ModelState.AddModelError("searchString", "Search String too Long");
                return null;
            }

            List<Property> properties = new List<Property>();
            using (SqlConnection connection = new SqlConnection(_sqlBuilder.ConnectionString))
            {
                string sqlScript = "EXEC spSearchPropertiesByLocation @location";
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlScript, connection))
                {
                    command.Parameters.Add(new SqlParameter() { ParameterName = "location", SqlDbType = System.Data.SqlDbType.VarChar, Size = 50, Value = searchLocationString });
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Property property = new Property()
                            {
                                PropertyId = reader.GetInt32(0),
                                PropertyName = reader.GetString(1),
                                DailyRate = reader.GetDecimal(2),
                                MaximumOccupants = reader.GetInt32(3),
                                Location = reader.GetString(4),
                                NonStandardFeatures = reader.GetString(5)
                            };
                            properties.Add(property);
                        }
                    }
                }
            }
            return properties;
        }

        [HttpGet("GetSearchByMaxOccupants/{searchMaxValue}", Name = "GetSearchByMaxOccupants")]
        public IEnumerable<Property> GetSearchByMaxOccupants(int searchMaxValue)
        {
            //validate
            if (searchMaxValue <= 0)
            {
                ModelState.AddModelError("searchValue", "Search Value must be a Positive Interger");
                return null;
            }

            List<Property> properties = new List<Property>();
            using (SqlConnection connection = new SqlConnection(_sqlBuilder.ConnectionString))
            {
                string sqlScript = "EXEC spSearchPropertiesByMaxOccupants @occupants";
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlScript, connection))
                {
                    command.Parameters.Add(new SqlParameter() { ParameterName = "occupants", SqlDbType = System.Data.SqlDbType.Int, Value = searchMaxValue });
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Property property = new Property()
                            {
                                PropertyId = reader.GetInt32(0),
                                PropertyName = reader.GetString(1),
                                DailyRate = reader.GetDecimal(2),
                                MaximumOccupants = reader.GetInt32(3),
                                Location = reader.GetString(4),
                                NonStandardFeatures = reader.GetString(5)
                            };
                            properties.Add(property);
                        }
                    }
                }
            }
            return properties;
        }

        [HttpGet("GetSearchByName/{searchString}", Name = "GetSearchByName")]
        public IEnumerable<Property> GetSearchByName(string searchString)
        {
            //validate
            if (searchString.Length > 50)
            {
                ModelState.AddModelError("searchString", "Search String too Long");
                return null;
            }

            List<Property> properties = new List<Property>();
            using (SqlConnection connection = new SqlConnection(_sqlBuilder.ConnectionString))
            {
                string sqlScript = "EXEC spSearchPropertiesByName @name";
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlScript, connection))
                {
                    command.Parameters.Add(new SqlParameter() { ParameterName = "name", SqlDbType = System.Data.SqlDbType.VarChar, Size = 50, Value = searchString });
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Property property = new Property()
                            {
                                PropertyId = reader.GetInt32(0),
                                PropertyName = reader.GetString(1),
                                DailyRate = reader.GetDecimal(2),
                                MaximumOccupants = reader.GetInt32(3),
                                Location = reader.GetString(4),
                                NonStandardFeatures = reader.GetString(5)
                            };
                            properties.Add(property);
                        }
                    }
                }
            }
            return properties;
        }


        [HttpGet("GetSearchByRate/{searchRateValue}", Name = "GetSearchByRate")]
        public IEnumerable<Property> GetSearchByRate(Decimal searchRateValue)
        {
            //validate
            if (searchRateValue <= 0)
            {
                ModelState.AddModelError("searchString", "Search String too Long");
                return null;
            }

            List<Property> properties = new List<Property>();
            using (SqlConnection connection = new SqlConnection(_sqlBuilder.ConnectionString))
            {
                string sqlScript = "EXEC spSearchPropertiesByRate @rate";
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlScript, connection))
                {
                    command.Parameters.Add(new SqlParameter() { ParameterName = "rate", SqlDbType = System.Data.SqlDbType.Money, Value = searchRateValue });
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Property property = new Property()
                            {
                                PropertyId = reader.GetInt32(0),
                                PropertyName = reader.GetString(1),
                                DailyRate = reader.GetDecimal(2),
                                MaximumOccupants = reader.GetInt32(3),
                                Location = reader.GetString(4),
                                NonStandardFeatures = reader.GetString(5)
                            };
                            properties.Add(property);
                        }
                    }
                }
            }
            return properties;
        }

        [HttpPost]
        public string Post(Property model)
        {
            string message = "";
            StringSanitizer.SanitizeSqlString(model.PropertyName);
            StringSanitizer.SanitizeSqlString(model.Location);
            if (!string.IsNullOrEmpty(model.NonStandardFeatures))
            {
                StringSanitizer.SanitizeSqlString(model.NonStandardFeatures);
            }

            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_sqlBuilder.ConnectionString))
                {
                    string sqlScript = "EXEC spUpdateProperty @id, @name, @rate, @maxOccupant, @location, @feataures";
                    connection.Open();
                    int count = 0;

                    string countScript = "SELECT COUNT(*) FROM Property WHERE PropertyId = @id";

                    using (SqlCommand command = new SqlCommand(countScript, connection))
                    {
                        command.Parameters.Add(new SqlParameter() { ParameterName = "id", SqlDbType = System.Data.SqlDbType.Int, Value = model.PropertyId });

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                count = reader.GetInt32(0);
                            }
                        }
                    }

                    if (count == 0)
                    {
                        return "Target record not found";
                    }
                    else
                        using (SqlCommand command = new SqlCommand(sqlScript, connection))
                        {
                            command.Parameters.Add(new SqlParameter() { ParameterName = "id", SqlDbType = System.Data.SqlDbType.Int, Value = model.PropertyId });
                            command.Parameters.Add(new SqlParameter() { ParameterName = "name", SqlDbType = System.Data.SqlDbType.VarChar, Size = 50, Value = model.PropertyName });
                            command.Parameters.Add(new SqlParameter() { ParameterName = "rate", SqlDbType = System.Data.SqlDbType.Money, Value = model.DailyRate });
                            command.Parameters.Add(new SqlParameter() { ParameterName = "maxOccupant", SqlDbType = System.Data.SqlDbType.Int, Value = model.MaximumOccupants });
                            command.Parameters.Add(new SqlParameter() { ParameterName = "location", SqlDbType = System.Data.SqlDbType.VarChar, Size = 50, Value = model.Location });
                            command.Parameters.Add(new SqlParameter() { ParameterName = "feataures", SqlDbType = System.Data.SqlDbType.VarChar, Size = 2000, Value = model.NonStandardFeatures });

                            string debug = command.ToString();

                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (SqlException e)
                            {
                                this.Response.StatusCode = 500;
                                return e.Message;
                                
                            }
                        }
                }
            }


            return "Success";
        }


    }
}