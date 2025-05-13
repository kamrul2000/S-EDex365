using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.Model.Model;
using System.Collections.Generic;
using System.Data;

namespace S_EDex365.API.Services
{
    public class TeacherService: ITeacherService
    {
        private readonly string _connectionString;
        public TeacherService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<ProblemPostAll>> GetAllPostAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryString = @"
            SELECT 
                p.Id,
                s.SubjectName AS Subject,
                c.ClassName AS sClass,
                p.Topic,
                p.Description,
                p.Photo,FORMAT(t1.GetDateby, 'yyyy-MM-dd') AS GetDateby 
            FROM ProblemsPost p
            INNER JOIN Users u ON p.SubjectId = u.SubjectId
            INNER JOIN Subject s ON p.SubjectId = s.Id
            INNER JOIN Class c ON p.ClassId = c.Id";

                var problemPostList = await connection.QueryAsync<ProblemPostAll>(queryString);

                var baseUrl = "http://192.168.0.238:81/uploads/";
                foreach (var problem in problemPostList)
                {
                    if (!string.IsNullOrEmpty(problem.Photo))
                    {
                        problem.Photo = baseUrl + problem.Photo;
                    }
                }

                return problemPostList.ToList();
            }
        }

        public async Task<List<ProblemList>> GetAllPostByUserAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                //var queryFlagCheck = "select BlockFlag from RecivedProblem where UserId= @UserId";
                //var check = await connection.ExecuteScalarAsync<bool?>(queryFlagCheck, new { UserId = userId });

                //if (check == true)
                //{
                //    return Enumerable.Empty<ProblemList>().ToList();
                //}

                var query = @" select t1.Id, t1.Topic, t1.Description, t1.Photo , t2.SubjectName AS Subject, t3.ClassName AS sClass,Flag from ProblemsPost t1 JOIN Subject t2 ON t1.SubjectId = t2.Id LEFT JOIN Class t3 ON t3.Id = t1.ClassId Left JOIN EnglishMediumClass t5 on t5.Id=t1.ClassId JOIN TeacherSkill t4 on t4.SubjectId=t1.SubjectId where Flag = 0 and t4.UserId='" + userId + "'";

                    var result = await connection.QueryAsync<ProblemList>(query);
                    var baseUrl = "http://192.168.0.238:81/uploads/";

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

        public async Task<List<ProblemList>> GetAllProblemsAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @" SELECT t1.Id, t1.Topic, t1.Description, t1.Photo, t2.SubjectName AS Subject, t3.ClassName AS sClass,t1.Flag FROM ProblemsPost t1 JOIN Subject t2 ON t1.SubjectId = t2.Id LEFT JOIN Class t3 ON t3.Id = t1.ClassId LEFT JOIN EnglishMediumClass t5 on t5.Id=t1.ClassId JOIN RecivedProblem t4 on t4.ProblemsPostId=t1.Id where t1.Flag=1 and t4.SolutionPending=1 and t4.UserId='" + userId +"' ";

                var result = await connection.QueryAsync<ProblemList>(query);
                var baseUrl = "http://192.168.0.238:81/uploads/";

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

        public async Task<List<SolutionShowAll>> GetAllSolutionTeacherAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryString = "Select t2.Id,t2.Photo,t3.SubjectName AS Subject ,t2.Topic,COALESCE(t4.ClassName, t5.ClassName) AS sClass,t2.Description, FORMAT(t1.GetDateby, 'yyyy-MM-dd') AS GetDateby from RecivedProblem t1 join ProblemsPost t2 on t1.ProblemsPostId=t2.id join Subject t3 on t3.Id=t2.SubjectId LEFT JOIN Class t4 on t4.Id=t2.ClassId LEFT JOIN EnglishMediumClass t5 on t5.Id=t2.ClassId where t1.UserId='"+ userId + "' and t1.SolutionPending=0 ";
                var query = string.Format(queryString);
                var SolutionShowList = await connection.QueryAsync<SolutionShowAll>(query);
                connection.Close();

                var baseUrl = "http://192.168.0.238:81/solutionImage/";

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

        public async Task<List<SolutionShowAll>> GetSolutionTeacherByPostIDAsync(Guid postId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryString = "select t2.Id,t1.Photo,t3.SubjectName AS Subject ,t2.Topic,COALESCE(t4.ClassName, t5.ClassName) AS sClass,t2.Description, FORMAT(t1.GetDateby, 'yyyy-MM-dd') AS GetDateby from SolutionPost t1 join ProblemsPost t2 on t1.ProblemPostId=t2.id join Subject t3 on t3.Id=t2.SubjectId LEFT JOIN Class t4 on t4.Id=t2.ClassId LEFT JOIN EnglishMediumClass t5 on t5.Id=t2.ClassId where t2.Id='" + postId + "' ";
                var query = string.Format(queryString);
                var SolutionShowList = await connection.QueryAsync<SolutionShowAll>(query);
                connection.Close();

                var baseUrl = "http://192.168.0.238:81/solutionImage/";

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

        public async Task<List<ProblemList>> UpdateProblemFlagAsync(Guid userId, Guid postId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var queryFlagCheck = "select BlockFlag from RecivedProblem where UserId= @UserId";
                    var check = await connection.ExecuteScalarAsync<int>(queryFlagCheck, new { UserId = userId });
                    if (check != 1)
                    {
                        var queryCount = "select COUNT(Id) from RecivedProblem where SolutionPending=1 and UserId=@UserId  ";
                        var count = await connection.ExecuteScalarAsync<int>(queryCount, new { UserId = userId });

                        if (count == 0)
                        {
                            var query = "UPDATE ProblemsPost SET Flag = 1,TaskPending=1 WHERE Id = @PostId";
                            await connection.ExecuteScalarAsync<Guid>(query, new { PostId = postId });

                            // Insert into RecivedProblem
                            var queryString = @"
                INSERT INTO RecivedProblem (id, UserId, ProblemsPostId, GetDateby, Updateby,SolutionPending,S_LastTime) VALUES (@id, @UserId, @ProblemsPostId, @GetDateby, @Updateby,@SolutionPending,@S_LastTime)"
                            ;

                            //INSERT INTO RecivedProblem(id, UserId, ProblemsPostId, GetDateby, Updateby) OUTPUT INSERTED.Id VALUES(@id, @UserId, @ProblemsPostId, @GetDateby, @Updateby)";

                            var parameters = new DynamicParameters();
                            var newId = Guid.NewGuid();
                            parameters.Add("id", newId, DbType.Guid);
                            parameters.Add("UserId", userId, DbType.Guid);
                            parameters.Add("ProblemsPostId", postId, DbType.Guid);
                            parameters.Add("GetDateby", DateTime.Now, DbType.DateTime);
                            parameters.Add("Updateby", DateTime.Now, DbType.DateTime);
                            parameters.Add("SolutionPending", 1, DbType.Boolean);
                            //parameters.Add("S_LastTime", DateTime.Now.AddMinutes(20), DbType.DateTime);
                            parameters.Add("S_LastTime", DateTime.Now.AddMinutes(20).ToString("HH:mm"), DbType.String);


                            // Execute the insertion
                            await connection.ExecuteScalarAsync<Guid>(queryString, parameters);

                            // Query to fetch updated data for PostReceived
                            var fetchQuery = @" SELECT t1.Id, t1.Topic, t1.Description, t1.Photo, t2.SubjectName AS Subject, t3.ClassName AS sClass,t1.Flag as Flag FROM ProblemsPost t1 JOIN Subject t2 ON t1.SubjectId = t2.Id LEFT JOIN Class t3 ON t3.Id = t1.ClassId LEFT JOIN EnglishMediumClass t5 on t5.Id=t1.ClassId join RecivedProblem t4 on t1.Id=t4.ProblemsPostId WHERE t4.SolutionPending=1 and t4.UserId = @UserId";

                            // Fetch and map results to PostReceived
                            var result = await connection.QueryAsync<ProblemList>(
                                fetchQuery,
                                new { UserId = userId });

                            return result.ToList(); // Convert to List<PostReceived>
                        }
                        return null;
                    }
                    else
                    {
                        return Enumerable.Empty<ProblemList>().ToList();
                    }


                        

                    
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as necessary
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}
