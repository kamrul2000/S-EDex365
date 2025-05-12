using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.Model.Model;
using System;
using System.Data;

namespace S_EDex365.API.Services
{
    public class ClaimCommunicationService : IClaimCommunicationService
    {
        private readonly string _connectionString;
        private readonly IWebHostEnvironment _environment;
        public ClaimCommunicationService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _environment = environment;
        }

        public async Task<List<SolutionChatResponse>> GetSolutionChatAsync(Guid solutionId)
        {
            var result = new List<SolutionChatResponse>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var queryType = @"
            SELECT DISTINCT t1.UserId, t2.Name AS Type 
            FROM ClaimCommunication t1 
            JOIN Roles t2 ON t2.Id = t1.UserType 
            WHERE t1.SolutionId = @SolutionId";

                var userTypes = await connection.QueryAsync<UserTypeInfo>(queryType, new { SolutionId = solutionId });

                foreach (var user in userTypes)
                {
                    var queryChat = @"
                SELECT 
                    VoiceUrl, 
                    ImageUrl,
                    Text, 
                    GetDate,
                    CASE 
                        WHEN VoiceUrl IS NULL THEN Text
                        WHEN Text IS NULL THEN VoiceUrl
                        WHEN ImageUrl IS NULL THEN Text + ' ' + VoiceUrl
                        ELSE Text + ' ' + VoiceUrl + ' ' + ImageUrl 
                    END AS Message
                FROM ClaimCommunication 
                WHERE UserId = @UserId AND SolutionId = @SolutionId";

                    var chats = (await connection.QueryAsync<ClaimCommunicationResponse>(
                        queryChat, new { UserId = user.UserId, SolutionId = solutionId }
                    )).ToList();

                    foreach (var chat in chats)
                    {
                        if (!string.IsNullOrEmpty(chat.VoiceUrl))
                        {
                            chat.VoiceUrl = "http://192.168.0.238:81/recording/" + chat.VoiceUrl;
                        }

                        if (!string.IsNullOrEmpty(chat.ImageUrl))
                        {
                            chat.ImageUrl = "http://192.168.0.238:81/claimImage/" + chat.ImageUrl;
                        }

                        chat.Message = $"{chat.Text ?? ""} {(chat.VoiceUrl ?? "")} {(chat.ImageUrl ?? "")}".Trim();
                    }

                    // Now add to result after updating all messages
                    result.Add(new SolutionChatResponse
                    {
                        // UserId = user.UserId,
                        // UserType = user.Type,
                        Chats = chats
                    });

                }
            }

            return result;
        }
    public async Task InsertClaimCommunicationAsync(ClaimCommunicationDto claimCommunicationDto, Guid userId, Guid solutionId)
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

                    // Save the uploaded photo
                    string uniqueFileName = null;
                    if (claimCommunicationDto.ImageUrl != null && claimCommunicationDto.ImageUrl.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "claimImage");
                        Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

                        uniqueFileName = Guid.NewGuid().ToString() + "_" + claimCommunicationDto.ImageUrl.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await claimCommunicationDto.ImageUrl.CopyToAsync(fileStream);
                        }
                    }

                    var query = "SELECT COUNT(1) FROM ClaimCommunication WHERE SolutionId = '"+solutionId+"' and UserId=@UserId";
                    var count = await connection.ExecuteScalarAsync<int>(query, new { UserId = userId });

                    if (count <= 0)
                    {
                        var queryString = @"
            INSERT INTO ClaimCommunication 
            (Id,VoiceUrl,ImageUrl,Text,UserId,SolutionId,UserType,GetDate) 
            VALUES 
            (@Id,@VoiceUrl,@ImageUrl,@Text,@UserId,@SolutionId,@UserType,@GetDate)";

                        var parameters = new DynamicParameters();
                        var chatId = Guid.NewGuid();
                        parameters.Add("id", chatId, DbType.Guid);
                        parameters.Add("Text", claimCommunicationDto.Text, DbType.String);
                        parameters.Add("UserId", userId, DbType.Guid);
                        parameters.Add("SolutionId", solutionId, DbType.Guid);
                        parameters.Add("VoiceUrl", voiceFileName, DbType.String);
                        parameters.Add("ImageUrl", uniqueFileName, DbType.String);
                        parameters.Add("UserType", typeId, DbType.Guid);
                        parameters.Add("GetDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        await connection.ExecuteAsync(queryString, parameters);
                    }
                    else
                    {
                        var queryTId = "SELECT TeacherId FROM SolutionPost WHERE Id = @SolutionId ";
                        var teacherId = await connection.ExecuteScalarAsync<Guid>(queryTId, new { SolutionId = solutionId });
                        
                        //
                        var queryPId = "SELECT ProblemPostId FROM SolutionPost WHERE Id = @SolutionId";
                        var problemId = await connection.ExecuteScalarAsync<Guid>(queryPId, new { SolutionId = solutionId });
                        //
                        var ImageUrlPId = "SELECT ImageUrl FROM ClaimCommunication WHERE SolutionId = '" + solutionId + "' and UserId=@UserId";
                        var sImageId = await connection.ExecuteScalarAsync<string>(ImageUrlPId, new { UserId = teacherId });
                        //
                        var taskImagelPId = "SELECT Photo FROM SolutionPost WHERE Id = '" + solutionId + "' and ProblemPostId=@ProblemPostId";
                        var taskimageId = await connection.ExecuteScalarAsync<string>(taskImagelPId, new { ProblemPostId = problemId });


                        var queryString = @"
            INSERT INTO ClaimTask 
            (Id,TeacherId,StudentId,ProblemPostId,TaskImage,SolutionImage,GetDateby) 
            VALUES 
            (@Id,@TeacherId,@StudentId,@ProblemPostId,@TaskImage,@SolutionImage,@GetDateby)";

                        var parameters = new DynamicParameters();
                        var chatId = Guid.NewGuid();
                        parameters.Add("id", chatId, DbType.Guid);
                        parameters.Add("TeacherId", teacherId, DbType.Guid);
                        parameters.Add("StudentId", userId, DbType.Guid);
                        parameters.Add("ProblemPostId", problemId, DbType.Guid);
                        parameters.Add("TaskImage", taskimageId, DbType.String);
                        parameters.Add("SolutionImage", sImageId, DbType.String);
                        parameters.Add("GetDateby", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        await connection.ExecuteAsync(queryString, parameters);
                    }


                    
                }
            }

            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
