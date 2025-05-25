using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.Model.Model;
using System.Data;

namespace S_EDex365.API.Services
{
    public class SubjectUpdateService : ISubjectUpdateService
    {
        private readonly string _connectionString;
        public SubjectUpdateService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> DeleteUserAsync(Guid userId, List<Guid> subIds)
        {
            if (subIds == null || subIds.Count == 0)
            {
                return false;
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Create parameterized IN clause dynamically
                    var subIdParams = string.Join(",", subIds.Select((id, index) => $"@SubId{index}"));

                    var queryString = $"DELETE FROM TeacherSkill WHERE UserId = @UserId AND SubjectId IN ({subIdParams})";

                    var parameters = new DynamicParameters();
                    parameters.Add("UserId", userId, DbType.Guid);

                    // Add each GUID individually as a parameter
                    for (int i = 0; i < subIds.Count; i++)
                    {
                        parameters.Add($"SubId{i}", subIds[i], DbType.Guid);
                    }

                    var success = await connection.ExecuteAsync(queryString, parameters);

                    return success > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }



        public async Task<List<SubjectResponseUpdate>> GetAllSubjectResponseUpdateAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Query to select Id and SubjectName from the Subject table for the given userId
                var queryString = "SELECT t2.Id, t2.SubjectName FROM TeacherSkill t1 JOIN Subject t2 ON t1.SubjectId = t2.Id WHERE t1.UserId = @UserId and t1.Status=1";
                var skillList = await connection.QueryAsync<(Guid Id, string SubjectName)>(queryString, new { UserId = userId });

                // Create a list to hold the SubjectResponseUpdate objects
                var responseList = new List<SubjectResponseUpdate>();

                // Populate the SubjectResponseUpdate list
                foreach (var skill in skillList)
                {
                    responseList.Add(new SubjectResponseUpdate
                    {
                        Id = skill.Id, // Assign the correct Id
                        Subject = new List<string> { skill.SubjectName } // Assign the SubjectName correctly
                    });
                }

                return responseList;
            }
        }








        public async Task<SubjectResponseUpdate> UpdateSubjectAsync(SubjectDtoUpdate subjectDtoUpdate)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Ensure the UserId is valid (not Guid.Empty)
                    Guid? userId = subjectDtoUpdate.userId != Guid.Empty ? (Guid?)subjectDtoUpdate.userId : null;
                    if (userId == null)
                    {
                        throw new ArgumentException("Invalid UserId");
                    }

                    // Iterate over each SubjectId in the list and process it
                    foreach (var subjectIdStr in subjectDtoUpdate.Subject)
                    {
                        // Convert SubjectId to Guid, skip if invalid
                        if (!Guid.TryParse(subjectIdStr, out Guid subjectId))
                        {
                            continue; // Skip invalid SubjectId entries
                        }

                        // Check if a record already exists for the UserId and SubjectId
                        string selectQuery = "SELECT COUNT(1) FROM TeacherSkill WHERE UserId = @UserId AND SubjectId = @SubjectId";
                        SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
                        selectCommand.Parameters.AddWithValue("@UserId", userId);
                        selectCommand.Parameters.AddWithValue("@SubjectId", subjectId);

                        var exists = (int)(await selectCommand.ExecuteScalarAsync()) > 0;

                        if (exists)
                        {
                            // If the record exists, update it (although there's no real need to update if the SubjectId is the same)
                            string updateQuery = "UPDATE TeacherSkill SET SubjectId = @SubjectId,Updateby=@Updateby,Status=@Status WHERE UserId = @UserId AND SubjectId = @SubjectId";
                            SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                            updateCommand.Parameters.AddWithValue("@SubjectId", subjectId);
                            updateCommand.Parameters.AddWithValue("@UserId", userId);
                            updateCommand.Parameters.AddWithValue("@Status", 0);
                            updateCommand.Parameters.AddWithValue("@Updateby", DateTime.Now.ToString("yyyy-MM-dd"));

                            await updateCommand.ExecuteNonQueryAsync();
                        }
                        else
                        {
                            // If no record exists, insert a new one
                            string insertQuery = "INSERT INTO TeacherSkill (Id, UserId, SubjectId,GetDateby,Updateby,Status) VALUES (@Id, @UserId, @SubjectId,@GetDateby,@Updateby,@Status)";
                            SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                            Guid newId = Guid.NewGuid(); // Generate new ID for each row
                            insertCommand.Parameters.AddWithValue("@Id", newId);
                            insertCommand.Parameters.AddWithValue("@UserId", userId);
                            insertCommand.Parameters.AddWithValue("@SubjectId", subjectId);
                            insertCommand.Parameters.AddWithValue("@Status", 0);
                            insertCommand.Parameters.AddWithValue("@GetDateby", DateTime.Now.ToString("yyyy-MM-dd"));
                            insertCommand.Parameters.AddWithValue("@Updateby", DateTime.Now.ToString("yyyy-MM-dd"));

                            await insertCommand.ExecuteNonQueryAsync();
                        }
                    }

                    // Return the updated subjects in the response
                    SubjectResponseUpdate subjectResponseUpdate = new SubjectResponseUpdate
                    {
                        Subject = subjectDtoUpdate.Subject // Return the list of subjects updated/inserted
                    };

                    return subjectResponseUpdate;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as necessary
                throw;
            }
        }








    }
}
