using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Models.Payment;
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
                var queryString = "select t1.Photo from SolutionPost t1 join ProblemsPost t2 on t1.ProblemPostId=t2.id where t2.id='"+ postId +"' ";
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

        //public async Task<List<SolutionShowAll>> GetAllSolutionAsync(Guid userId)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();

        //        //// Fetch the SubjectId based on userId
        //        //var query = "SELECT CAST(SubjectId AS VARCHAR(36)) AS SubjectId FROM Users WHERE Id = @UserId";
        //        //var subjectId = await connection.QueryFirstOrDefaultAsync<string>(query, new { UserId = userId });

        //        // Query to get the problem posts
        //        var queryString = @" SELECT t1.id,t1.Topic, t1.Description, t1.Photo, t2.SubjectName AS Subject, t3.ClassName AS Class FROM ProblemsPost t1 JOIN Subject t2 ON t2.Id = t1.SubjectId JOIN Class t3 ON t3.Id = t1.ClassId WHERE t1.SubjectId = @SubjectId";

        //        var parameters = new { SubjectId = subjectId };
        //        var problemPostList = (await connection.QueryAsync<ProblemPostAll>(queryString, parameters)).ToList();

        //        // Add base URL to the Photo property
        //        foreach (var post in problemPostList)
        //        {
        //            post.Photo = $"{_basePhotoUrl}{post.Photo}";
        //        }

        //        return problemPostList;
        //    }
        //}

        public async Task<SolutionPostResponse> InsertSolutionPostAsync(SolutionPostDto solutionPost, Guid postId)
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

                    var query = "UPDATE RecivedProblem SET SolutionPending = 0,S_LastTime=NULL WHERE ProblemsPostId = @ProblemsPostId";
                    await connection.ExecuteScalarAsync<Guid>(query, new { ProblemsPostId = postId });


                    var queryProblemPost = "UPDATE ProblemsPost SET ForWallet = 1 WHERE Id = @Id";
                    await connection.ExecuteScalarAsync<Guid>(queryProblemPost, new { Id = postId });

                    var queryExisting = "SELECT Amount FROM Balance WHERE UserId = @UserId";
                    var parametersExisting = new DynamicParameters();
                    parametersExisting.Add("UserId", userId, DbType.Guid);

                    decimal existingAmount = await connection.QueryFirstOrDefaultAsync<decimal>(queryExisting, parametersExisting);


                    // 1. Get the existing amount
                    var queryProblemsPost = "SELECT Amount FROM ProblemsPost WHERE Id = @Id";
                    var parametersProblemsPost = new DynamicParameters();
                    parametersProblemsPost.Add("Id", postId, DbType.Guid);

                    decimal existingProblemsPost = await connection.QueryFirstOrDefaultAsync<decimal>(queryProblemsPost, parametersProblemsPost);

                    // 2. Add the new amount to the existing one
                    decimal updatedAmount = existingAmount - existingProblemsPost ;

                    // 3. Update the Balance table with the new total
                    var queryBalance = "UPDATE Balance SET Amount = @Amount, GatDate = @GatDate WHERE UserId = @UserId";
                    var parametersBalance = new DynamicParameters();
                    parametersBalance.Add("UserId", userId);
                    parametersBalance.Add("Amount", updatedAmount);
                    parametersBalance.Add("GatDate", DateTime.Now.ToString("yyyy-MM-dd"));

                    var successs = await connection.ExecuteAsync(queryBalance, parametersBalance);


                    var queryExistingAmountTeacher = "SELECT Amount FROM TeacherBalance WHERE UserId = @UserId";
                    var parametersExistingAmountTeacher = new DynamicParameters();
                    parametersExistingAmountTeacher.Add("UserId", userId, DbType.Guid);

                    decimal existingAmountTeacher = await connection.QueryFirstOrDefaultAsync<decimal>(queryExistingAmountTeacher, parametersExistingAmountTeacher);

                    decimal updatedTeacherAmount = existingAmountTeacher + existingProblemsPost;

                    if (updatedTeacherAmount > 0)
                    {
                        var queryTeacherBalance = "insert into TeacherBalance (id,UserId,Amount,GatDate) values ";
                        queryTeacherBalance += "(@id,@UserId,@Amount,@GatDate)";
                        var parametersTeacherBalance = new DynamicParameters();
                        var IdBalance = Guid.NewGuid().ToString();
                        parametersTeacherBalance.Add("id", IdBalance, DbType.String);
                        parametersTeacherBalance.Add("UserId", userId);
                        parametersTeacherBalance.Add("Amount", updatedTeacherAmount);
                        parametersTeacherBalance.Add("GatDate", DateTime.Now.ToString("yyyy-MM-dd"));
                        var successsTeacherBalance = await connection.ExecuteAsync(queryTeacherBalance, parametersTeacherBalance);
                    }
                    else
                    {
                        // 3. Update the Balance table with the new total
                        var queryTeacherBalanceUpdate = "UPDATE TeacherBalance SET Amount = @Amount, GatDate = @GatDate WHERE UserId = @UserId";
                        var parametersTeacherBalanceUpdate = new DynamicParameters();
                        parametersTeacherBalanceUpdate.Add("UserId", userId);
                        parametersTeacherBalanceUpdate.Add("Amount", updatedAmount);
                        parametersTeacherBalanceUpdate.Add("GatDate", DateTime.Now.ToString("yyyy-MM-dd"));

                        var successsTeacherBalanceUpdate = await connection.ExecuteAsync(queryTeacherBalanceUpdate, parametersTeacherBalanceUpdate);
                    }

                    


                    // Save the uploaded photo
                    string uniqueFileName = null;
                    if (solutionPost.Photo != null && solutionPost.Photo.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "solutionImage");
                        Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

                        uniqueFileName = Guid.NewGuid().ToString() + "_" + solutionPost.Photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await solutionPost.Photo.CopyToAsync(fileStream);
                        }
                    }
                    else
                    {
                        throw new Exception("No file uploaded.");
                    }

                    // Insert the solution post into the database
                    var queryString = @"
            INSERT INTO SolutionPost 
            (id, TeacherId, Photo, StudentId, Status, GetDateby, Updateby, ProblemPostId) 
            VALUES 
            (@id, @TeacherId, @Photo, @StudentId, @Status, @GetDateby, @Updateby, @ProblemPostId)";

                    var parameters = new DynamicParameters();
                    var solutionPostId = Guid.NewGuid();
                    parameters.Add("id", solutionPostId, DbType.Guid);
                    parameters.Add("TeacherId", solutionPost.TeacherId, DbType.Guid);
                    parameters.Add("Photo", uniqueFileName, DbType.String); // Save the file name in DB
                    parameters.Add("StudentId", userId, DbType.Guid);
                    parameters.Add("Status", 1, DbType.Boolean);
                    parameters.Add("GetDateby", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    parameters.Add("Updateby", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    parameters.Add("ProblemPostId", postId, DbType.Guid);

                    var success = await connection.ExecuteAsync(queryString, parameters);

                    // Prepare the response
                    SolutionPostResponse solutionPostResponse = new SolutionPostResponse
                    {
                        Photo = $"http://192.168.0.159:81/api/solutionImage/{uniqueFileName}"
                    };

                    return solutionPostResponse;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save solution post: {ex.Message}");
            }
        }
    }
}
