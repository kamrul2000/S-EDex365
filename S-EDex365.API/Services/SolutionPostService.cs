using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Models.Payment;
using S_EDex365.Model.Model;
using System.Data;

namespace S_EDex365.API.Services
{
    public class SolutionPostService : ISolutionPostService
    {
        private readonly string _connectionString;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SolutionPostService(IConfiguration configuration, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<SolutionShowAll>> GetAllSolutionAsync(Guid postId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryString = "select t1.Id as Id,t1.Photo from SolutionPost t1 join ProblemsPost t2 on t1.ProblemPostId=t2.id where t2.id='" + postId +"' ";
                var query = string.Format(queryString);
                var SolutionShowList = await connection.QueryAsync<SolutionShowAll>(query);
                connection.Close();

                var baseUrl = "https://api.edex365.com/solutionImage/";

                // Update the Photo property with the full URL
                foreach (var problem in SolutionShowList)
                {
                    if (!string.IsNullOrEmpty(problem.Photo))
                    {
                        problem.Photo = baseUrl + problem.Photo;
                    }
                }

                return SolutionShowList.ToList();
            }
        }
        //public async Task<SolutionPostResponse> InsertSolutionPostAsync(SolutionPostDto solutionPost, Guid postId)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(_connectionString))
        //        {
        //            connection.Open();

        //            //// Validate if postId exists in the database
        //            string queryString1 = "SELECT UserId FROM ProblemsPost WHERE id = @id";
        //            var parameters1 = new DynamicParameters();
        //            parameters1.Add("id", postId, DbType.Guid);

        //            var userId = await connection.QuerySingleOrDefaultAsync<Guid>(queryString1, parameters1);

        //            if (userId == Guid.Empty)
        //            {
        //                throw new Exception("Invalid ProblemPostId.");
        //            }

        //            var query = "UPDATE RecivedProblem SET SolutionPending = 0,S_LastTime=NULL WHERE ProblemsPostId = @ProblemsPostId";
        //            await connection.ExecuteScalarAsync<Guid>(query, new { ProblemsPostId = postId });


        //            var queryProblemPost = "UPDATE ProblemsPost SET ForWallet = 1,TaskPending=0 WHERE Id = @Id";
        //            await connection.ExecuteScalarAsync<Guid>(queryProblemPost, new { Id = postId });

        //            // Save the uploaded photo
        //            string uniqueFileName = null;
        //            if (solutionPost.Photo != null && solutionPost.Photo.Length > 0)
        //            {
        //                string uploadsFolder = Path.Combine(_environment.WebRootPath, "solutionImage");
        //                Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

        //                uniqueFileName = Guid.NewGuid().ToString() + "_" + solutionPost.Photo.FileName;
        //                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //                using (var fileStream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await solutionPost.Photo.CopyToAsync(fileStream);
        //                }
        //            }
        //            else
        //            {
        //                throw new Exception("No file uploaded.");
        //            }

        //            // Insert the solution post into the database
        //            var queryString = @"
        //    INSERT INTO SolutionPost 
        //    (id, TeacherId, Photo, StudentId, Status, GetDateby, Updateby, ProblemPostId,PaymentTime,PaymentBlock) 
        //    VALUES 
        //    (@id, @TeacherId, @Photo, @StudentId, @Status, @GetDateby, @Updateby, @ProblemPostId,@PaymentTime,@PaymentBlock)";

        //            var parameters = new DynamicParameters();
        //            var solutionPostId = Guid.NewGuid();
        //            parameters.Add("id", solutionPostId, DbType.Guid);
        //            parameters.Add("TeacherId", solutionPost.TeacherId, DbType.Guid);
        //            parameters.Add("Photo", uniqueFileName, DbType.String); // Save the file name in DB
        //            parameters.Add("StudentId", userId, DbType.Guid);
        //            parameters.Add("Status", 1, DbType.Boolean);
        //            parameters.Add("GetDateby", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //            parameters.Add("Updateby", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //            parameters.Add("PaymentTime", DateTime.Now.AddMinutes(715).ToString("HH:mm"), DbType.String);
        //            parameters.Add("ProblemPostId", postId, DbType.Guid);
        //            parameters.Add("PaymentBlock", 1, DbType.Boolean);

        //            var success = await connection.ExecuteAsync(queryString, parameters);

        //            // Prepare the response
        //            SolutionPostResponse solutionPostResponse = new SolutionPostResponse
        //            {
        //                Photo = $"https://api.edex365.com/api/solutionImage/{uniqueFileName}"
        //            };

        //            return solutionPostResponse;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Failed to save solution post: {ex.Message}");
        //    }
        //}


        public async Task<List<SolutionPostResponse>> InsertSolutionPostAsync(SolutionPostDto solutionPost, Guid postId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Validate if postId exists in the database
                    string queryString1 = "SELECT UserId FROM ProblemsPost WHERE id = @id";
                    var parameters1 = new DynamicParameters();
                    parameters1.Add("id", postId, DbType.Guid);

                    var userId = await connection.QuerySingleOrDefaultAsync<Guid>(queryString1, parameters1);

                    if (userId == Guid.Empty)
                    {
                        throw new Exception("Invalid ProblemPostId.");
                    }

                    // Update related records
                    var updateRecivedProblem = "UPDATE RecivedProblem SET SolutionPending = 0, S_LastTime = NULL WHERE ProblemsPostId = @ProblemsPostId";
                    await connection.ExecuteScalarAsync<Guid>(updateRecivedProblem, new { ProblemsPostId = postId });

                    var updateProblemPost = "UPDATE ProblemsPost SET ForWallet = 1, TaskPending = 0 WHERE Id = @Id";
                    await connection.ExecuteScalarAsync<Guid>(updateProblemPost, new { Id = postId });

                    // Validate and process multiple files
                    if (solutionPost.Photos == null || !solutionPost.Photos.Any())
                    {
                        throw new Exception("No files uploaded.");
                    }

                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "solutionImage");
                    Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

                    var responses = new List<SolutionPostResponse>();

                    foreach (var photo in solutionPost.Photos)
                    {
                        if (photo != null && photo.Length > 0)
                        {
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await photo.CopyToAsync(fileStream);
                            }

                            // Insert each photo into SolutionPost table
                            var queryString = @"
INSERT INTO SolutionPost 
(id, TeacherId, Photo, StudentId, Status, GetDateby, Updateby, ProblemPostId, PaymentTime, PaymentBlock) 
VALUES 
(@id, @TeacherId, @Photo, @StudentId, @Status, @GetDateby, @Updateby, @ProblemPostId, @PaymentTime, @PaymentBlock)";

                            var parameters = new DynamicParameters();
                            var solutionPostId = Guid.NewGuid();
                            parameters.Add("id", solutionPostId, DbType.Guid);
                            parameters.Add("TeacherId", solutionPost.TeacherId, DbType.Guid);
                            parameters.Add("Photo", uniqueFileName, DbType.String);
                            parameters.Add("StudentId", userId, DbType.Guid);
                            parameters.Add("Status", 1, DbType.Boolean);
                            parameters.Add("GetDateby", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            parameters.Add("Updateby", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            parameters.Add("PaymentTime", DateTime.Now.AddMinutes(715).ToString("HH:mm"), DbType.String);
                            parameters.Add("ProblemPostId", postId, DbType.Guid);
                            parameters.Add("PaymentBlock", 1, DbType.Boolean);

                            await connection.ExecuteAsync(queryString, parameters);

                            responses.Add(new SolutionPostResponse
                            {
                                Photo = $"https://api.edex365.com/api/solutionImage/{uniqueFileName}"
                            });
                        }
                    }

                    return responses;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save solution post: {ex.Message}");
            }
        }

    }
}
