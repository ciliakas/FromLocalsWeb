using System.Data;
using System.Threading.Tasks;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Utilities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FromLocalsToLocals.Services.Ado
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
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("AppDbContext")))
                {
                    connection.Open();

                    var update = new NpgsqlCommand("UPDATE \"Vendors\" " +
                                                   "SET = \"Title\" = @Title, \"About\" = @About, \"Address\" = @Address, \"VendorType\" = @VendorType, \"Image\" = @Image, WHERE \"ID\" = @Id",
                        connection);

                    update.Parameters.AddWithValue("@Id", vendor.ID);
                    update.Parameters.AddWithValue("@Title", vendor.Title);
                    update.Parameters.AddWithValue("@About", vendor.About);
                    update.Parameters.AddWithValue("@Address", vendor.Address);
                    update.Parameters.AddWithValue("@VendorType", vendor.VendorType);
                    update.Parameters.AddWithValue("@Image", vendor.Image);


                    var dataAdapter =
                        new NpgsqlDataAdapter("SELECT ID, Title, About, Address, VendorType, Image FROM \"Vendors\"",
                            connection);
                    dataAdapter.UpdateCommand = update;

                    var dataSet = new DataSet();
                    dataAdapter.Fill(dataSet, "\"Vendors\"");

                    var dataTable = new DataTable();
                    dataTable = dataSet.Tables["\"Vendors\""];

                    dataAdapter.Update(dataTable);
                    dataAdapter.Dispose();
                }
            }

            catch (NpgsqlException e)
            {
                await e.ExceptionSender();
            }
        }

        public async Task InsertWorkHoursAsync(WorkHours workHours)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("AppDbContext")))
                {
                    connection.Open();

                    using (var insert = new NpgsqlCommand(
                        "INSERT INTO \"VendorWorkHours\" (\"ID\", \"VendorID\", \"IsWorking\", \"Day\", \"OpenTime\", \"CloseTime\")" +
                        "VALUES (DEFAULT, @VendorID, @IsWorking, @Day, @OpentTime, @CloseTime)", connection))
                    {
                        insert.Parameters.AddWithValue("@VendorID", workHours.VendorID);
                        insert.Parameters.AddWithValue("@IsWorking", workHours.IsWorking);
                        insert.Parameters.AddWithValue("@Day", workHours.Day);
                        insert.Parameters.AddWithValue("@OpentTime", workHours.OpenTime);
                        insert.Parameters.AddWithValue("@CloseTime", workHours.CloseTime);

                        insert.ExecuteNonQuery();
                    }
                }
            }

            catch (NpgsqlException e)
            {
                await e.ExceptionSender();
            }
        }

        public async Task DeleteVendorAsync(Vendor vendor)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Configuration.GetConnectionString("AppDbContext")))
                {
                    connection.Open();

                    using (var delete = new NpgsqlCommand("DELETE FROM \"Vendors\" WHERE \"ID\" = @Id", connection))
                    {
                        delete.Parameters.AddWithValue("@Id", vendor.ID);
                        delete.ExecuteNonQuery();
                    }
                }
            }

            catch (NpgsqlException e)
            {
                await e.ExceptionSender();
            }
        }
    }
}