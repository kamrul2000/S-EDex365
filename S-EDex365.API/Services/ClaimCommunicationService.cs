using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using System.Data;

namespace S_EDex365.API.Services
{
    public class ClaimCommunicationService : IClaimCommunicationService
    {
        private readonly string _connectionString;
        public ClaimCommunicationService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public Task<List<ClaimCommunicationResponse>> GetAllClaimCommunicationAsync()
        {
            throw new NotImplementedException();
        }

        public async Task InsertClaimCommunicationAsync(ClaimCommunicationDto claimCommunicationDto, Guid userId, Guid problempostId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var QueryUserType = "select t2.RoleId from Roles t1 JOIN UserRole t2 on t2.RoleId=t1.Id where t2.UserId='" + userId + "' ";
                    var typeId = await connection.QueryFirstOrDefaultAsync<Guid>(QueryUserType);



                    string voiceFileName = null;

                    if (claimCommunicationDto.VoiceUrl != null && claimCommunicationDto.VoiceUrl.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "claimRecording");
                        Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

                        voiceFileName = Guid.NewGuid().ToString() + Path.GetExtension(claimCommunicationDto.VoiceUrl.FileName);
                        var filePath = Path.Combine(uploadsFolder, voiceFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await claimCommunicationDto.VoiceUrl.CopyToAsync(stream);
                        }
                    }

                    var queryString = @"
            INSERT INTO Communication 
            (Id,VoiceUrl,Text,UserId,ProblemPostId,UserType,GetDate) 
            VALUES 
            (@Id,@VoiceUrl,@Text,@UserId,@ProblemPostId,@UserType,@GetDate)";

                    var parameters = new DynamicParameters();
                    var chatId = Guid.NewGuid();
                    parameters.Add("id", chatId, DbType.Guid);
                    parameters.Add("Text", claimCommunicationDto.Text, DbType.String);
                    parameters.Add("UserId", userId, DbType.Guid);
                    parameters.Add("ProblemPostId", problempostId, DbType.Guid);
                    parameters.Add("VoiceUrl", voiceFileName, DbType.String);
                    parameters.Add("UserType", typeId, DbType.Guid);
                    parameters.Add("GetDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    await connection.ExecuteAsync(queryString, parameters);
                }
            }

            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
