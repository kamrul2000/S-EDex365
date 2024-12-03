using Dapper;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;

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
                connection.Open();
                var queryString = "select p.id,p.classId,p.SubjectId,p.Topic,p.Description,p.Photo from ProblemsPost p inner join Users u on p.Subjectid=u.Subjectid ";
                var query = string.Format(queryString);
                var ProblemPostList = await connection.QueryAsync<ProblemPostAll>(query);
                connection.Close();
                return ProblemPostList.ToList();
            }
        }


        //public async Task<List<ProblemPostAll>> GetAllPostByUserAsync(Guid userId)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        var query = "select CAST(SubjectId AS VARCHAR(36)) AS SubjectId from Users where id='" + userId + "'";
        //        var subjectid = await connection.QueryFirstOrDefaultAsync<string>(query);





        //        var queryString = @"
        //    SELECT p.id, p.classId, p.SubjectId, p.Topic, p.Description, p.Photo 
        //    FROM ProblemsPost p 
        //    INNER JOIN Users u ON u.SubjectId = @Subjectid ";

        //        var parameters = new { Subjectid = subjectid };
        //        var problemPostList = await connection.QueryAsync<ProblemPostAll>(queryString, parameters);

        //        connection.Close();
        //        return problemPostList.ToList();
        //    }
        //}








        //private readonly string _basePhotoUrl = "https://n373hk41-61214.asse.devtunnels.ms/uploads/";

        //public async Task<List<ProblemPostAll>> GetAllPostByUserAsync(Guid userId)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();


        //        var query = "SELECT CAST(SubjectId AS VARCHAR(36)) AS SubjectId FROM Users WHERE Id = @UserId";
        //        var subjectId = await connection.QueryFirstOrDefaultAsync<string>(query, new { UserId = userId });

        //         query = "SELECT t1.UserId FROM TeacherSkill t1 JOIN Subject t2 ON t1.SubjectId = t2.Id JOIN Users t3 ON t1.UserId = t3.Id WHERE t1.SubjectId = @SubjectId";
        //        var userId1 = await connection.QueryFirstOrDefaultAsync<string>(query, new { SubjectId = subjectId });

        //        foreach (var userid in userId1)
        //        {
        //            query = $@" SELECT t1.Id, t1.Topic, t1.Description, t1.Photo, t2.SubjectName AS Subject, t3.ClassName AS sClass FROM ProblemsPost t1 JOIN Subject t2 ON t1.SubjectId = t2.Id JOIN Class t3 ON t3.Id = t1.ClassId JOIN TeacherSkill t4 ON t4.SubjectId = t1.SubjectId WHERE t1.SubjectId = 'A63AD0C6-5A81-4390-B17C-7B35E2130609' AND t4.UserId = '{userId}'";
        //            //var result = await ExecuteQuery(query);
        //            var problemPostList = (await connection.QueryAsync<ProblemPostAll>(query)).ToList();
        //            foreach (var post in problemPostList)
        //            {
        //                post.Photo = $"{_basePhotoUrl}{post.Photo}";
        //            }
        //            //return problemPostList;
        //        }
        //        return problemPostList;
        //    }
        //}



        private readonly string _basePhotoUrl = "https://www.edex365.com/uploads/";

        public async Task<List<ProblemPostAll>> GetAllPostByUserAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Get all SubjectIds for the specified UserId
                var query = @"SELECT t1.SubjectId FROM TeacherSkill t1 JOIN Subject t2 ON t1.SubjectId = t2.Id JOIN Users t3 ON t1.UserId = t3.Id  WHERE t3.Id = @Id";
                //var subjectIds = (await connection.QueryAsync<string>(query, new { Id = userId })).ToList();
                var subjectIds = (await connection.QueryAsync<Guid>(query, new { Id = userId })).ToList();


                if (!subjectIds.Any())
                {
                    return new List<ProblemPostAll>(); // Return an empty list if no SubjectIds are found
                }

                // Initialize an empty list to collect all problem posts
                var problemPostList = new List<ProblemPostAll>();

                // Iterate over each SubjectId
                foreach (var subjectId in subjectIds)
                {
                    // Get the list of UserIds who have the same SubjectId
                    query = @"SELECT t1.UserId FROM TeacherSkill t1 JOIN Subject t2 ON t1.SubjectId = t2.Id JOIN Users t3 ON t1.UserId = t3.Id WHERE t1.SubjectId = @SubjectId";
                    var userIdList = (await connection.QueryAsync<Guid>(query, new { SubjectId = subjectId })).ToList();

                    // Define the query for getting the problem posts for each user and each SubjectId
                    query = @"SELECT t1.Id, t1.Topic, t1.Description, t1.Photo, t2.SubjectName AS Subject, t3.ClassName AS sClass FROM ProblemsPost t1 JOIN Subject t2 ON t1.SubjectId = t2.Id JOIN Class t3 ON t3.Id = t1.ClassId JOIN TeacherSkill t4 ON t4.SubjectId = t1.SubjectId WHERE t1.SubjectId = @SubjectId AND t4.UserId = @UserId";

                    // Retrieve problem posts for each user and add them to the list
                    foreach (var currentUserId in userIdList)
                    {
                        if (currentUserId==userId)
                        {
                            var userPosts = (await connection.QueryAsync<ProblemPostAll>(query, new { SubjectId = subjectId, UserId = currentUserId })).ToList();
                            foreach (var post in userPosts)
                            {
                                post.Photo = $"{_basePhotoUrl}{post.Photo}";
                            }
                            problemPostList.AddRange(userPosts);
                        }
                        
                    }
                }

                return problemPostList;
            }
        }






    }
}
