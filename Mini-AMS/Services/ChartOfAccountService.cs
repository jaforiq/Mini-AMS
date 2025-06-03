using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Mini_AMS.Models;
using System.Threading.Tasks;

namespace Mini_AMS.Services
{
    public class ChartOfAccountService
    {
        private readonly IConfiguration _configuration;

        public ChartOfAccountService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task CreateAccountAsync(string name, int? parentId, string type)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand("sp_ManageChartOfAccounts", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Action", "CREATE");
                command.Parameters.AddWithValue("@Id", DBNull.Value);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@ParentId", (object?)parentId ?? DBNull.Value);
                command.Parameters.AddWithValue("@Type", type ?? (object)DBNull.Value);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAccountAsync(int id, string name, int? parentId, string type)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand("sp_ManageChartOfAccounts", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Action", "UPDATE");
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@ParentId", (object?)parentId ?? DBNull.Value);
                command.Parameters.AddWithValue("@Type", type ?? (object)DBNull.Value);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<ChartOfAccount>> GetAllAccountsAsync()
        {
            var accounts = new List<ChartOfAccount>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand("SELECT Id, Name, ParentId, [Type] FROM ChartOfAccounts", connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    accounts.Add(new ChartOfAccount
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        ParentId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                        Type = reader.IsDBNull(3) ? null : reader.GetString(3)
                    });
                }
            }
            return accounts;
        }

        public async Task DeleteAccountAsync(int id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand("sp_ManageChartOfAccounts", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Action", "DELETE");
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", DBNull.Value);
                command.Parameters.AddWithValue("@ParentId", DBNull.Value);
                command.Parameters.AddWithValue("@Type", DBNull.Value);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
} 