using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using S_EDex365.Data.Interfaces;
using S_EDex365.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_EDex365.Data.Services
{
    public class BanglaClassService : IBanglaClassService
    {
        private readonly string _connectionString;
        public BanglaClassService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString));
        }
        public async Task<bool> DeleteBanglaClassAsync(Guid classId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "delete from Class where id=@id";
                    var parameters = new DynamicParameters();
                    parameters.Add("id", classId.ToString(), DbType.String);
                    var success = await connection.ExecuteAsync(queryString, parameters);
                    if (success > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<BanglaClass>> GetAllBanglaClassAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = @" SELECT Id, ClassName as ClassName, Amount as Amount, S_ClassName as S_ClassName, Status, (CASE WHEN Status = 1 THEN 'Active' ELSE 'InActive' END) AS StatusName FROM Class ORDER BY S_ClassName;
            ";
                    var classList = await connection.QueryAsync<BanglaClass>(query);
                    return classList.ToList();
                }
            }
            catch (Exception ex)
            {
                // Consider logging the error
                throw;
            }
        }



        public async Task<BanglaClass> GetBanglaClassByIdAsync(Guid classId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var queryString = "select * from Class where id='{0}' order by S_ClassName  ";
                    var query = string.Format(queryString, classId);
                    var classType = await connection.QueryFirstOrDefaultAsync<BanglaClass>(query);
                    return classType;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> InsertBanglaClassAsync(BanglaClass banglaClass)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var queryString = "select ClassName from Class where lower(ClassName)='{0}' ";
                    var query = string.Format(queryString, banglaClass.ClassName.ToLower());
                    var roleObj = await connection.QueryFirstOrDefaultAsync<string>(query);
                    if (roleObj != null && roleObj.Length > 0)
                        return false;
                    queryString = "insert into Class (id,ClassName,status,Amount,S_ClassName,UpdateAt) values ";
                    queryString += "( @id,@ClassName,@status,@Amount,@S_ClassName,@UpdateAt)";
                    var parameters = new DynamicParameters();
                    parameters.Add("id", Guid.NewGuid().ToString(), DbType.String);
                    parameters.Add("ClassName", banglaClass.ClassName, DbType.String);
                    parameters.Add("status", banglaClass.Status, DbType.Int32);
                    parameters.Add("Amount", banglaClass.Amount, DbType.Decimal);
                    parameters.Add("S_ClassName", banglaClass.S_ClassName, DbType.Int64);
                    parameters.Add("UpdateAt", DateTime.Now.ToString("yyyy-MM-dd"), DbType.String);
                    var success = await connection.ExecuteAsync(queryString, parameters);
                    if (success > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateBanglaClassAsync(BanglaClass banglaClass)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var queryString = "update Class set ClassName=@ClassName,Amount=@Amount,S_ClassName=@S_ClassName,status=@status where id=@id";
                    var parameters = new DynamicParameters();
                    parameters.Add("ClassName", banglaClass.ClassName, DbType.String);
                    parameters.Add("Amount", banglaClass.Amount, DbType.Decimal);
                    parameters.Add("S_ClassName", banglaClass.S_ClassName, DbType.Int64);
                    parameters.Add("status", banglaClass.Status, DbType.Int32);
                    parameters.Add("id", banglaClass.Id.ToString(), DbType.String);
                    var success = await connection.ExecuteAsync(queryString, parameters);
                    if (success > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
