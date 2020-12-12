using Npgsql;
using FromLocalsToLocals.Utilities;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace FromLocalsToLocals.Models.Services
{
    public class VendorServiceADO : IVendorServiceADO
    {
        public VendorServiceADO(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task UpdateVendorAsync(Vendor vendor)
        {
            try 
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(Configuration.GetConnectionString("AppDbContext")))
                {
                    connection.Open();

                    NpgsqlCommand update = new NpgsqlCommand("UPDATE \"Vendors\" " +
                        "SET = \"Title\" = @Title, \"About\" = @About, \"Address\" = @Address, \"VendorType\" = @VendorType, \"Image\" = @Image, WHERE \"ID\" = @Id", connection);

                    update.Parameters.AddWithValue("@Id", vendor.ID);
                    update.Parameters.AddWithValue("@Title", vendor.Title);
                    update.Parameters.AddWithValue("@About", vendor.About);
                    update.Parameters.AddWithValue("@Address", vendor.Address);
                    update.Parameters.AddWithValue("@VendorType", vendor.VendorType);
                    update.Parameters.AddWithValue("@Image", vendor.Image);


                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT ID, Title, About, Address, VendorType, Image FROM \"Vendors\"", connection);
                    dataAdapter.UpdateCommand = update;

                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet, "\"Vendors\"");

                    DataTable dataTable = new DataTable();
                    dataTable = dataSet.Tables["\"Vendors\""];

                    dataAdapter.Update(dataTable);
                    dataAdapter.Dispose();
                }
            }

            catch(NpgsqlException e)
            {
                await e.ExceptionSender();
            }
        }
    }
}
