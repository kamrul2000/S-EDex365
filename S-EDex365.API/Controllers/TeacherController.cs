using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.API.Services;
using S_EDex365.Model.Model;
using System.Data;

namespace S_EDex365.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly ITeacherService _teacherService;
        public TeacherController(ITeacherService teacherService, IConfiguration configuration)
        {
            _teacherService = teacherService;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        [HttpGet("s/AllProblems/{userId}")]
        public async Task<ActionResult> GetProblems(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var queryFlagCheck = "SELECT UserBlocked FROM Users WHERE Id = @Id";
                var check = await connection.ExecuteScalarAsync<bool?>(queryFlagCheck, new { Id = userId });

                if (check == true)
                {
                    var query = "SELECT UnlockTime FROM RecivedProblem WHERE UserId = @UserId AND BlockFlag = 1";
                    var parametersType = new DynamicParameters();
                    parametersType.Add("UserId", userId, DbType.Guid);

                    // Read as TimeSpan
                    var unlockTime = await connection.QueryFirstOrDefaultAsync<TimeSpan?>(query, parametersType);

                    string unlockTimeStr = unlockTime.HasValue
                        ? DateTime.Today.Add(unlockTime.Value).ToString("hh:mm tt")
                        : null;



                    return Ok(new { message = "You have been temporarily blocked from taking new problems for 24 hours due to not resolving your assigned problem on time. Please ensure timely and proper responses to avoid future blocks.Your Unblock Time : "+ unlockTimeStr + "" });
                }
            }

            // Only call service if not blocked
            var subjectDetails = await _teacherService.GetAllPostByUserAsync(userId);

            return Ok(subjectDetails);
        }



        [HttpGet("s/GetAllAcceptProblem/{userId}")]
        public async Task<ActionResult<List<ProblemList>>> GetAllAcceptProblemsAsync(Guid userId)
        {
            var subjectDetails = await _teacherService.GetAllProblemsAsync(userId);

            if (subjectDetails.Count == 0)
                return Ok(subjectDetails);

            return Ok(subjectDetails);
        }

        [HttpGet("s/AllSolutionByTeacher/{userId}")]
        public async Task<ActionResult<List<SolutionShowAll>>> AllSolutionByTeacher(Guid userId)
        {
            var subjectDetails = await _teacherService.GetAllSolutionTeacherAsync(userId);
            if (subjectDetails.Count == 0)
                return Ok("[]");
            return Ok(subjectDetails);
        }
        [HttpGet("s/SolutionTeacherByPostId/{postId}")]
        public async Task<ActionResult<List<SolutionShowAll>>> SolutionTeacherByPostId(Guid postId)
        {
            var subjectDetails = await _teacherService.GetSolutionTeacherByPostIDAsync(postId);
            if (subjectDetails.Count == 0)
                return Ok();
            return Ok(subjectDetails);
        }

        [HttpPost("s/UpdateProblemFlag/{userId}/{postId}")]
        public async Task<ActionResult<List<ProblemPostAll>>> UpdateProblemFlag(Guid userId,Guid postId)
        {
            try
            {
                // Call the service method
                var updateDetails = await _teacherService.UpdateProblemFlagAsync(userId, postId);

                if (updateDetails == null)
                {
                    return Ok("Already Have a Task.");
                }

                if (updateDetails.Count == 0)
                    return Ok("You are Blocked For ");

                return Ok(updateDetails);
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
