using Npgsql;
using FromLocalsToLocals.Utilities;
using System.Threading.Tasks;
using System.Data;

namespace FromLocalsToLocals.Models.Services
{
    public class VendorServiceADO : IVendorServiceADO
    {
        const string connectionString = "Host=fromlocals-paulius-e731.aivencloud.com;Port=18477;UserId=avnadmin;Password=it2clu8afgd3uosp;SSLMode=Require;Trust Server Certificate=true;Database=defaultdb;";

        public async Task UpdateVendorAsync(Vendor vendor)
        {
            try 
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
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
