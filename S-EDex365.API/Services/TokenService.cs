using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.Model.Model;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace S_EDex365.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public TokenService(IConfiguration configuration)
        {
            _config = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<string> CreateToken(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryString = "select (select name from roles where id=ur.roleid) from UserRole ur where userid=@UserId";
                var parameters = new DynamicParameters();
                parameters.Add("UserId", user.Id.ToString(), DbType.String);
                var type = await connection.QueryFirstOrDefaultAsync<string>(queryString, parameters);

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Email),
                     new Claim(ClaimTypes.Name,user.Name),
                     new Claim(ClaimTypes.SerialNumber,user.Id.ToString()),
                     new Claim(ClaimTypes.Anonymous,Guid.NewGuid().ToString()),
                     new Claim(ClaimTypes.Actor,type.ToString())
                 };
                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: credentials);
                connection.Close();

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
}
