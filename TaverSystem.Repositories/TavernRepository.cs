using Microsoft.Data.SqlClient;
using TavernSystem.Models.DTO;

namespace TaverSystem.Repositories;

public class TavernRepository : ITavernRepository
{
    private readonly string _connectionString;

    public TavernRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<object>> GetAllAdventurersAsync()
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            try
            {
                await conn.OpenAsync();
                
                var sqlQuery = "Select Id, Nickname From Adventurer";

                using (var cmd = new SqlCommand(sqlQuery, conn))
                {
                    var reader = await cmd.ExecuteReaderAsync();

                    var result = new List<object>();
                    
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new {Id = reader.GetInt32(0), Nickname = reader.GetString(1)});
                        }
                    }
                    
                    await reader.CloseAsync();
                    return result;
                }
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
    }

    public async Task<object> GetAdventurerByIdAsync(int id)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            try
            {
                await conn.OpenAsync();

                var sqlQuery =
                    @"Select ad.Id, ad.Nickname, r.Name, e.Name, p.Id, p.FirstName, p.MiddleName, p.LastName, p.HasBounty
                                 From Adventurer ad
                                 join Race r on ad.RaceId = r.Id
                                 join ExperienceLevel e on ad.ExperienceId = e.Id
                                 join Person p on ad.PersonId = p.Id
                                 WHERE ad.Id = @Id";

                object result = null;

                using (var cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    
                    var reader = await cmd.ExecuteReaderAsync();
                    
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();

                        result = new
                        {
                            Id = reader.GetInt32(0),
                            Nickname = reader.GetString(1),
                            Race = reader.GetString(2),
                            ExperienceLevel = reader.GetString(3),
                            PersonData = new
                            {
                                Id = reader.GetInt32(4),
                                FirstName = reader.GetString(5),
                                MiddleName = reader.GetString(6),
                                LastName = reader.GetString(7),
                                HasBounty = reader.GetBoolean(8),
                            }
                        };
                    }
                    await reader.CloseAsync();
                }

                return result;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
    }

    public async Task<bool> AddAdventurerAsync(AdventurerDTO request)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var transaction = conn.BeginTransaction();
            try
            {
                var queryMax = "select coalesce(max(id), 0) from Adventurer";

                int id = -1;
                
                using (var cmd = new SqlCommand(queryMax, conn, transaction))
                {
                    var reader = await cmd.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    id = reader.GetInt32(0) + 1;
                    await reader.CloseAsync();
                }
                
                var queryInsert = "INSERT INTO ADVENTURER (Id, Nickname, RaceId, ExperienceId, PersonId) values (@Id, @Nickname, @RaceId, @ExperienceId, @PersonId)";

                using (var cmd = new SqlCommand(queryInsert, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nickname", request.Nickname);
                    cmd.Parameters.AddWithValue("@RaceId", request.RaceId);
                    cmd.Parameters.AddWithValue("@ExperienceId", request.ExperienceId);
                    cmd.Parameters.AddWithValue("PersonId", request.PersonDataId);
                    
                    var result = await cmd.ExecuteNonQueryAsync();
                    if (result == 0)
                        throw new Exception("No records were affected.");
                    
                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
    }
}