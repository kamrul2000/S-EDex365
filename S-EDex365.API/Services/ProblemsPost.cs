using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.Data.Services;
using S_EDex365.Model.Model;
using System.Data;

namespace S_EDex365.API.Services
{
    public class ProblemsPost : IProblemsPost
    {
        private readonly string _connectionString;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProblemsPost(IConfiguration configuration, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        //public async Task<List<ProblemPostAll>> GetAllUserAsync(Guid userId)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        var queryString = "select t1.Id,t2.SubjectName,t1.Topic,t3.ClassName,Description,Photo from ProblemsPost t1 join Subject t2 on t2.Id=t1.SubjectId join Class t3 on t3.Id=t1.ClassId where UserId=@UserId ";
        //        var query = string.Format(queryString);
        //        var parameters = new { UserId = userId };
        //        var ProblemPostList = await connection.QueryAsync<ProblemPostAll>(query, parameters);
        //        connection.Close();
        //        return ProblemPostList.ToList();
        //    }
        //}



        //Correct

        //public async Task<List<ProblemPostAll>> GetAllUserAsync(Guid userId)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();

        //        var query = @" SELECT t1.Id, t1.Topic, t1.Description, t1.Photo, t2.SubjectName, t3.ClassName FROM ProblemsPost t1 JOIN Subject t2 ON t2.Id = t1.SubjectId JOIN Class t3 ON t3.Id = t1.ClassId WHERE t1.UserId = @UserId";

        //        var lookup = new Dictionary<Guid, ProblemPostAll>();

        //        var result = await connection.QueryAsync<ProblemPostAll, string, string, ProblemPostAll>(
        //            query,
        //            (problem, subjectName, className) =>
        //            {
        //                if (!lookup.TryGetValue(problem.Id, out var problemPost))
        //                {
        //                    problemPost = problem;
        //                    lookup.Add(problem.Id, problemPost);
        //                }

        //                if (!string.IsNullOrEmpty(subjectName) && !problemPost.Subject.Contains(subjectName))
        //                {
        //                    problemPost.Subject.Add(subjectName);
        //                }

        //                if (!string.IsNullOrEmpty(className) && !problemPost.Class.Contains(className))
        //                {
        //                    problemPost.Class.Add(className);
        //                }

        //                return problemPost;
        //            },
        //            new { UserId = userId },
        //            splitOn: "SubjectName,ClassName"
        //        );

        //        return lookup.Values.ToList();
        //    }
        //}


        public async Task<List<ProblemPostAll>> GetAllUserAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @" SELECT t1.Id, t2.SubjectName AS Subject, t1.Topic, t3.ClassName AS sClass, t1.Description, t1.Photo,FORMAT(t1.GetDateby, 'yyyy-MM-dd') AS GetDateby FROM ProblemsPost t1 JOIN Subject t2 ON t2.Id = t1.SubjectId JOIN Class t3 ON t3.Id = t1.ClassId WHERE t1.UserId = @UserId";

                var parameters = new { UserId = userId };

                var result = await connection.QueryAsync<ProblemPostAll>(query, parameters);

                var baseUrl = "https://api.edex365.com/uploads/";

                // Update the Photo property with the full URL
                foreach (var problem in result)
                {
                    if (!string.IsNullOrEmpty(problem.Photo))
                    {
                        problem.Photo = baseUrl + problem.Photo;
                    }

                }

                return result.ToList();
            }
        }

        public async Task<List<ProblemPostAll>> GetPendingUserAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @" SELECT t1.Id, t2.SubjectName AS Subject, t1.Topic, t3.ClassName AS sClass, t1.Description, t1.Photo,FORMAT(t1.GetDateby, 'yyyy-MM-dd') AS GetDateby FROM ProblemsPost t1 JOIN Subject t2 ON t2.Id = t1.SubjectId JOIN Class t3 ON t3.Id = t1.ClassId WHERE t1.Id NOT IN (SELECT ProblemPostId FROM SolutionPost) and t1.UserId =@UserId ";

                var parameters = new { UserId = userId };

                var result = await connection.QueryAsync<ProblemPostAll>(query, parameters);

                var baseUrl = "https://api.edex365.com/uploads/";

                // Update the Photo property with the full URL
                foreach (var problem in result)
                {
                    if (!string.IsNullOrEmpty(problem.Photo))
                    {
                        problem.Photo = baseUrl + problem.Photo;
                    }
                }

                return result.ToList();
            }
        }

        public async Task<ProblemPostAll> GetPostDetailsUserAsync(Guid postId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"SELECT t1.Id, t2.SubjectName AS Subject, t1.Topic, t3.ClassName AS sClass, t1.Description, t1.Photo,FORMAT(t1.GetDateby, 'yyyy-MM-dd') AS GetDateby FROM ProblemsPost t1 JOIN Subject t2 ON t2.Id = t1.SubjectId JOIN Class t3 ON t3.Id = t1.ClassId WHERE t1.id = @Id";

                var parameters = new { Id = postId };

                var result = await connection.QueryFirstOrDefaultAsync<ProblemPostAll>(query, parameters);
                if (result != null && !string.IsNullOrEmpty(result.Photo))
                {
                    result.Photo = "https://api.edex365.com/uploads/" + result.Photo;
                }

                return result;
            }
        }

        public async Task<List<ProblemPostAll>> GetSolutionUserAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"select t2.Id, t3.SubjectName AS Subject, t2.Topic, t4.ClassName AS sClass, t2.Description, t2.Photo,FORMAT(t1.GetDateby, 'yyyy-MM-dd') AS GetDateby from SolutionPost t1 JOIN ProblemsPost t2 on t1.ProblemPostId=t2.Id JOIN Subject t3 on t3.Id=t2.SubjectId JOIN Class t4 ON t4.Id = t2.ClassId where StudentId= @UserId";

                var parameters = new { UserId = userId };

                var result = await connection.QueryAsync<ProblemPostAll>(query, parameters);

                var baseUrl = "https://api.edex365.com/uploads/";

                // Update the Photo property with the full URL
                foreach (var problem in result)
                {
                    if (!string.IsNullOrEmpty(problem.Photo))
                    {
                        problem.Photo = baseUrl + problem.Photo;
                    }
                }

                return result.ToList();
            }
        }





        public async Task<(Guid postId, ProblemsPostResponse response)> InsertProblemPostAsync(ProblemsPostDto problemsPost)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Parsing the subjectId and classId from the DTO
                    Guid? subjectId = !string.IsNullOrEmpty(problemsPost.Subject.FirstOrDefault())
                        ? Guid.Parse(problemsPost.Subject.FirstOrDefault())
                        : (Guid?)null;

                    Guid? classId = !string.IsNullOrEmpty(problemsPost.sClass.FirstOrDefault())
                        ? Guid.Parse(problemsPost.sClass.FirstOrDefault())
                        : (Guid?)null;

                    // Save the uploaded photo
                    string uniqueFileName = null;
                    if (problemsPost.Photo != null)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads"); // Ensure "uploads" directory exists
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + problemsPost.Photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await problemsPost.Photo.CopyToAsync(fileStream);
                        }
                    }

                    // Insert the problem post into the database
                    var queryString = @"
            INSERT INTO ProblemsPost 
            (id, subjectId, topic, classId, Description, Photo, UserId, Status, Flag,GetDateby, Updateby) OUTPUT INSERTED.Id 
            VALUES 
            (@id, @subjectId, @topic, @classId, @Description, @Photo, @UserId, @Status, @Flag,@GetDateby, @Updateby)";

                    var parameters = new DynamicParameters();
                    var problemsPostId = Guid.NewGuid();
                    parameters.Add("id", problemsPostId, DbType.Guid);
                    parameters.Add("subjectId", subjectId, DbType.Guid);
                    parameters.Add("topic", problemsPost.Topic, DbType.String);
                    parameters.Add("classId", classId, DbType.Guid);
                    parameters.Add("Description", problemsPost.Description, DbType.String);
                    parameters.Add("Photo", uniqueFileName, DbType.String); // Save the filename to the database
                    parameters.Add("UserId", problemsPost.UserId, DbType.Guid);
                    parameters.Add("status", 1, DbType.Boolean);
                    parameters.Add("flag", 0, DbType.Boolean);
                    parameters.Add("GetDateby", DateTime.Now.ToString("yyyy-MM-dd"));
                    parameters.Add("Updateby", DateTime.Now.ToString("yyyy-MM-dd"));

                    var postId = await connection.ExecuteScalarAsync<Guid>(queryString, parameters);

                    // Prepend the base URL to the photo filename
                    string fullPhotoUrl = uniqueFileName != null
                        ? $"https://api.edex365.com/uploads/{uniqueFileName}"
                        : null;

                    // Prepare the response
                    ProblemsPostResponse problemsPostResponse = new ProblemsPostResponse
                    {
                        Subject = problemsPost.Subject,
                        Topic = problemsPost.Topic,
                        sClass = problemsPost.sClass,
                        Description = problemsPost.Description,
                        Photo = fullPhotoUrl, // Return the full URL
                        UserId = problemsPost.UserId
                    };

                    return (postId, problemsPostResponse);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }




    }
}
