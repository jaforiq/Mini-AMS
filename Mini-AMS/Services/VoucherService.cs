using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Mini_AMS.Models;
using System.Data;
using System.Threading.Tasks;

namespace Mini_AMS.Services
{
    public class VoucherService
    {
        private readonly IConfiguration _configuration;

        public VoucherService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SaveVoucherAsync(VoucherHeader voucher)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand("sp_SaveVoucher", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Date", voucher.Date);
                command.Parameters.AddWithValue("@ReferenceNo", voucher.ReferenceNo);
                command.Parameters.AddWithValue("@VoucherType", voucher.VoucherType);

                var table = new DataTable();
                table.Columns.Add("AccountId", typeof(int));
                table.Columns.Add("Debit", typeof(decimal));
                table.Columns.Add("Credit", typeof(decimal));
                table.Columns.Add("Narration", typeof(string));
                foreach (var line in voucher.Lines)
                {
                    table.Rows.Add(line.AccountId, line.Debit, line.Credit, line.Narration ?? "");
                }
                var linesParam = command.Parameters.AddWithValue("@Lines", table);
                linesParam.SqlDbType = SqlDbType.Structured;
                linesParam.TypeName = "dbo.VoucherLineType"; // You need to create this user-defined table type in SQL Server

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
