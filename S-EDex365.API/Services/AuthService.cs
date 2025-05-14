

using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Models;
using S_EDex365.Data.Interfaces;
using S_EDex365.Data.Services;
using S_EDex365.Model.Model;
using System;
using System.Data;
using System.Net;
using static System.Net.WebRequestMethods;

namespace S_EDex365.API.Services
{
    public class AuthService: IAuthService
    {
        private readonly string _connectionString;
        private readonly ITokenService _tokenService;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IConfiguration configuration, ITokenService tokenService, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _tokenService = tokenService;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> UpdatePasswordserAsync(Guid userId, string oldPassWord, string newPassword)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var query = "select Password from Users where Id=@Id";
                     var parameters1 = new DynamicParameters();
                    parameters1.Add("Id", userId.ToString(), DbType.String);
                    var password = await connection.QuerySingleOrDefaultAsync<string>(query, parameters1);
                    if (password == oldPassWord)
                    {
                        var queryString = "update Users set Password=@Password where Id=@Id";
                        var parameters = new DynamicParameters();
                        parameters.Add("Password", newPassword.ToString(), DbType.String);
                        parameters.Add("Id", userId, DbType.Guid);
                        var success = await connection.ExecuteAsync(queryString, parameters);
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

        public async Task<List<UserAllInformation>> GetAllUserInformationAsync(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //var queryString = "select t1.Name,t1.MobileNo,t1.Email,t1.Password,t1.School,STRING_AGG(t2.ClassName, ',') as ClassNames,t1.Dob, STRING_AGG(t3.SubjectName, ',') as SubjectNames,t1.Image from Users t1 join Class t2 on t2.Id=t1.ClassId join Subject t3 on t3.Id=t1.SubjectId where t1.Id='" + userId + "' group by t1.Name, t1.MobileNo, t1.Email, t1.Password, t1.School, t1.Dob, t1.Image; ";
                //var queryString = "select t1.Name,t1.MobileNo,t1.Email,t1.Password,t1.School,t1.ClassId, t1.Dob,t1.SubjectId,t1.Image from Users t1 where t1.Id='"+ userId + "' group by t1.Name, t1.MobileNo, t1.Email, t1.Password, t1.School, t1.Dob, t1.Image,t1.ClassId,t1.SubjectId";

                var queryString = "SELECT t1.Name AS Name, t1.MobileNo AS MobileNo, t1.Email AS Email, t1.Password AS Password, t1.School AS School, t1.Dob AS Dob, t1.Image AS Image, t2.ClassName AS classNames, t3.SubjectName AS subjectNames FROM Users t1 LEFT JOIN Class t2 ON t2.Id = t1.ClassId LEFT JOIN Subject t3 ON t3.Id = t1.SubjectId WHERE t1.Id = '" + userId + "' ";

                var query = string.Format(queryString);
                var UserAllInformationList = await connection.QueryAsync<UserAllInformation>(query);
                connection.Close();

                var baseUrl = "https://api.edex365.com/profileImage/";

                // Update the Photo property with the full URL
                foreach (var users in UserAllInformationList)
                {
                    if (!string.IsNullOrEmpty(users.Image))
                    {
                        users.Image = baseUrl + users.Image;
                    }
                }


                return UserAllInformationList.ToList();
            }
        }


        //public async Task<List<UserAllInformation>> GetAllUserInformationAsync(Guid userId)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();

        //        // Parameterized query to prevent SQL injection
        //        var queryString = @"SELECT 
        //                        t1.Name, 
        //                        t1.MobileNo, 
        //                        t1.Email, 
        //                        t1.Password, 
        //                        t1.School, 
        //                        t1.Dob, 
        //                        t1.Image, 
        //                        t2.ClassName, 
        //                        t3.SubjectName 
        //                    FROM Users t1 
        //                    LEFT JOIN Class t2 ON t2.Id = t1.ClassId 
        //                    LEFT JOIN Subject t3 ON t3.Id = t1.SubjectId 
        //                    WHERE t1.Id = @UserId";

        //        var userInformationList = (await connection.QueryAsync<UserAllInformation>(queryString, new { UserId = userId })).ToList();

        //        // Update the Image property with the full URL
        //        var baseUrl = "https://mpr7d8bq-61214.inc1.devtunnels.ms/profileImage/";

        //        foreach (var user in userInformationList)
        //        {
        //            if (!string.IsNullOrEmpty(user.Image))
        //            {
        //                user.Image = baseUrl + user.Image;
        //            }
        //        }

        //        return userInformationList;
        //    }
        //}


        public async Task<UserResponse> InsertUserAsync(UserDto user)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "SELECT COUNT(1) FROM Users WHERE MobileNo = @MobileNo";
                    var count = await connection.ExecuteScalarAsync<int>(query, new { MobileNo = user.MobileNo });

                    if (count <= 0)
                    {
                        var random = new Random();
                        var otp = random.Next(100000, 999999).ToString();

                        //var query = "select RName from Roles where id='" + user.Role + "'";
                        //var type = await connection.QueryFirstOrDefaultAsync<string>(query);

                        var roleId = Guid.Parse(user.Role.FirstOrDefault());
                        //var query = "SELECT RName FROM Roles WHERE id = @RoleId";
                        //var type = await connection.QueryFirstOrDefaultAsync<string>(query, new { RoleId = roleId });

                        //var subjectId = Guid.Parse(user.Subject.FirstOrDefault());
                        //var subjectQuery = "SELECT Id FROM Subject WHERE id = @SubjectId";
                        //var subject = await connection.QueryFirstOrDefaultAsync<string>(subjectQuery, new { SubjectId = subjectId });

                        //var ClassId = Guid.Parse(user.Class.FirstOrDefault());

                        //    Guid? roleId = !string.IsNullOrEmpty(user.Role.FirstOrDefault()) 
                        //? Guid.Parse(user.Role.FirstOrDefault()) 
                        //: (Guid?)null;

                        //        Guid? subjectId = !string.IsNullOrEmpty(user.Subject.FirstOrDefault()) 
                        //    ? Guid.Parse(user.Subject.FirstOrDefault()) 
                        //    : (Guid?)null;

                        //Guid? classId = !string.IsNullOrEmpty(user.Class.FirstOrDefault()) 
                        //    ? Guid.Parse(user.Class.FirstOrDefault()) 
                        //    : (Guid?)null;


                        //var queryString = "select email from users where lower(email)='{0}' ";
                        //var query = string.Format(queryString, user.Email.ToLower());
                        //var userObj = await connection.QueryFirstOrDefaultAsync<string>(query);
                        //if (userObj != null && userObj.Length > 0)
                        //    return false;

                        string uniqueImageFileName = null;

                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "profileImage"); // Ensure that "uploads" directory exists
                        uniqueImageFileName = Guid.NewGuid().ToString() + "_" + user.Image.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueImageFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await user.Image.CopyToAsync(fileStream);
                        }

                        string uniqueAcademicImageFileName = null;


                        if (_environment != null && user?.AcademicImage != null)
                        {
                            string uploadsAcademicImageFolder = Path.Combine(_environment.WebRootPath, "academicImage"); // Ensure that "uploads" directory exists
                            uniqueAcademicImageFileName = Guid.NewGuid().ToString() + "_" + user.AcademicImage.FileName;
                            string filePathAcademicImage = Path.Combine(uploadsAcademicImageFolder, uniqueAcademicImageFileName);
                            using (var fileStream = new FileStream(filePathAcademicImage, FileMode.Create))
                            {
                                await user.AcademicImage.CopyToAsync(fileStream);
                            }
                        }
                        else
                        {
                            uniqueAcademicImageFileName = "Null";
                        }

                        string uniqueCvFileName = null;

                        if (_environment != null && user?.CV != null)
                        {
                            string uploadsCvFolder = Path.Combine(_environment.WebRootPath, "cvFile"); // Ensure that "uploads" directory exists
                            uniqueCvFileName = Guid.NewGuid().ToString() + "_" + user.CV.FileName;
                            string filePathCv = Path.Combine(uploadsCvFolder, uniqueCvFileName);
                            using (var fileStream = new FileStream(filePathCv, FileMode.Create))
                            {
                                await user.CV.CopyToAsync(fileStream);
                            }
                        }
                        else
                        {
                            uniqueCvFileName = "Null";
                        }

                        





                        var queryString = "insert into Users (id,name,mobileno,email,status,password,Dob,GetDateby,Updateby,UserTypeId,otp,Aproval,cv,academicImage,image) values ";
                        queryString += "( @id,@name,@mobileno,@email,@status,@password,@Dob,@GetDateby,@Updateby,@UserTypeId,@otp,@Aproval,@cv,@academicImage,@image)";
                        var parameters = new DynamicParameters();
                        var userId = Guid.NewGuid();
                        parameters.Add("id", userId, DbType.Guid);
                        parameters.Add("name", user.Name, DbType.String);
                        parameters.Add("mobileno", user.MobileNo, DbType.String);
                        parameters.Add("email", user.Email, DbType.String);
                        parameters.Add("status", 0, DbType.Boolean);
                        parameters.Add("password", user.Password, DbType.String);
                        //parameters.Add("School", user.School, DbType.String);
                        //parameters.Add("ClassId", classId, DbType.Guid);
                        parameters.Add("Dob", user.Dob, DbType.String);
                        parameters.Add("OTP", otp, DbType.String);
                        parameters.Add("GetDateby", DateTime.Now.ToString("yyyy-MM-dd"));
                        parameters.Add("Updateby", DateTime.Now.ToString("yyyy-MM-dd"));
                        parameters.Add("UserTypeId", roleId, DbType.Guid);
                        parameters.Add("Aproval", 0, DbType.Boolean);
                        parameters.Add("cv", uniqueCvFileName, DbType.String);
                        parameters.Add("academicImage", uniqueAcademicImageFileName, DbType.String);
                        parameters.Add("image", uniqueImageFileName, DbType.String);
                        //parameters.Add("subjectId", subjectId, DbType.Guid);
                        var success = await connection.ExecuteAsync(queryString, parameters);

                        //foreach (var item in user.Subject)
                        //{
                        //    // If no record exists, insert a new one
                        //    string insertQuery = "INSERT INTO TeacherSkill (Id, UserId, SubjectId,GetDateby,Updateby) VALUES (@Id, @UserId, @SubjectId,@GetDateby,@Updateby)";
                        //    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                        //    Guid newId = Guid.NewGuid(); // Generate new ID for each row
                        //    insertCommand.Parameters.AddWithValue("@Id", newId);
                        //    insertCommand.Parameters.AddWithValue("@UserId", userId);
                        //    insertCommand.Parameters.AddWithValue("@SubjectId", user.Subject);
                        //    insertCommand.Parameters.AddWithValue("@GetDateby", DateTime.Now.ToString("yyyy-MM-dd"));
                        //    insertCommand.Parameters.AddWithValue("@Updateby", DateTime.Now.ToString("yyyy-MM-dd"));

                        //    //await insertCommand.ExecuteNonQueryAsync();
                        //    var successs = await connection.ExecuteAsync(insertQuery, insertCommand);
                        //}


                        foreach (var item in user.Subject)
                        {
                            string insertQuery = @"
        INSERT INTO TeacherSkill (Id, UserId, SubjectId, GetDateby, Updateby) 
        VALUES (@Id, @UserId, @SubjectId, @GetDateby, @Updateby)";

                            Guid newId = Guid.NewGuid(); // Generate a new ID for each row

                            var parameterss = new
                            {
                                Id = newId,
                                UserId = userId,
                                SubjectId = item, // Use the current subject ID
                                GetDateby = DateTime.Now.ToString("yyyy-MM-dd"),
                                Updateby = DateTime.Now.ToString("yyyy-MM-dd")
                            };

                            await connection.ExecuteAsync(insertQuery, parameterss);
                        }



                        foreach (var item in user.Role)
                        {
                            queryString = "insert into UserRole (UserId,RoleId) values ";
                            queryString += "( @UserId,@RoleId)";
                            parameters = new DynamicParameters();
                            parameters.Add("UserId", (userId), DbType.Guid);
                            parameters.Add("RoleId", item, DbType.String);
                            success = await connection.ExecuteAsync(queryString, parameters);
                        }
                        if (success > 0)
                        {

                            using (WebClient client = new WebClient())
                            {
                                var domain = "https://";
                                var message = "Edex365 OTP Number is " + otp + " ";
                                string url = "" + domain + "bulksmsbd.net/api/smsapi?api_key=AQVhdExRIVugmhs0fsNE&type=text&number=" + user.MobileNo + "&senderid=8809617620256&message=" + message + "";
                                //client.DownloadString(url);
                            }
                        }


                        string fullImageUrl = uniqueImageFileName != null
                            ? $"https://api.edex365.com/profileImage/{uniqueImageFileName}"
                            : null;
                        string fullAcademicImageUrl = uniqueAcademicImageFileName != null
                            ? $"https://api.edex365.com/academicImage/{uniqueAcademicImageFileName}"
                            : null;
                        string fullCvUrl = uniqueCvFileName != null
                            ? $"https://api.edex365.com/cvFile/{uniqueCvFileName}"
                            : null;

                        UserResponse userResponse = new UserResponse();
                        userResponse.Name = user.Name;
                        userResponse.MobileNo = user.MobileNo;
                        //userResponse.Status = user.Status;
                        userResponse.Email = user.Email;
                        userResponse.Password = user.Password;
                        userResponse.Dob = user.Dob;
                        //userResponse.School = user.School;
                        //userResponse.Class = user.Class;
                        userResponse.Subject = user.Subject;
                        userResponse.Role = user.Role;
                        userResponse.Id = userId;
                        userResponse.Image = fullImageUrl;
                        userResponse.AcademicImage = fullAcademicImageUrl;
                        userResponse.CV = fullCvUrl;
                        return userResponse;
                    }
                    else
                    {
                        return null;
                    }
                    

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<LoginResponse> Login(LoginDto loginDto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = "select t3.Name from Users t1 join UserRole t2 on t1.Id=t2.UserId join Roles t3 on t2.RoleId=t3.Id where t1.MobileNo=@MobileNo and t1.Password=@Password";
                var parameters = new DynamicParameters();
                parameters.Add("MobileNo", loginDto.MobileNo, DbType.String);
                parameters.Add("Password", loginDto.Password, DbType.String);
                var type = await connection.QueryFirstOrDefaultAsync<string>(query, parameters);

                LoginResponse loginResponse = new LoginResponse();
                if (type.Trim() == "Student")
                {
                    query = string.Format("select t1.Id,t1.Name,t1.Email,t3.Name as Type from Users t1  join UserRole t2 on t1.Id=t2.UserId  join Roles t3 on t2.RoleId=t3.Id where t1.MobileNo=@MobileNo and t1.Password=@Password");
                    parameters = new DynamicParameters();
                    parameters.Add("MobileNo", loginDto.MobileNo, DbType.String);
                    parameters.Add("password", loginDto.Password, DbType.String);
                    var userObj = await connection.QueryFirstOrDefaultAsync<User>(query, parameters);

                    string token = await _tokenService.CreateToken(userObj);

                    var deviceQueryString = "update Users set DeviceToken=@deviceToken where id=@id";
                    parameters = new DynamicParameters();
                    parameters.Add("deviceToken", loginDto.DeviceToken, DbType.String);
                    parameters.Add("id", userObj.Id, DbType.Guid);
                    var success = await connection.ExecuteAsync(deviceQueryString, parameters);


                    loginResponse.Token = token;
                    

                    loginResponse.Id = userObj.Id;
                    loginResponse.Name = userObj.Name;
                    loginResponse.Email = userObj.Email;
                    loginResponse.Type = type;
                    return loginResponse;
                }
                else if(type.Trim() == "Teacher")
                {
                    query = string.Format("select t1.Id,t1.Name,t1.Email,t3.Name as Type from Users t1  join UserRole t2 on t1.Id=t2.UserId  join Roles t3 on t2.RoleId=t3.Id where t1.MobileNo=@MobileNo and t1.Password=@Password and Aproval=1");
                    parameters = new DynamicParameters();
                    parameters.Add("MobileNo", loginDto.MobileNo, DbType.String);
                    parameters.Add("Password", loginDto.Password, DbType.String);
                    var userObj = await connection.QueryFirstOrDefaultAsync<User>(query, parameters);

                    string token = await _tokenService.CreateToken(userObj);

                    var deviceQueryString = "update Users set DeviceToken=@deviceToken where id=@id";
                    parameters = new DynamicParameters();
                    parameters.Add("deviceToken", loginDto.DeviceToken, DbType.String);
                    parameters.Add("id", userObj.Id, DbType.Guid);
                    var success = await connection.ExecuteAsync(deviceQueryString, parameters);

                    loginResponse.Token = token;
                    

                    loginResponse.Id = userObj.Id;
                    loginResponse.Name = userObj.Name;
                    loginResponse.Email = userObj.Email;
                    loginResponse.Type = type;
                    return loginResponse;
                }
                else
                {
                    query = string.Format("select t1.Id,t1.Name,t1.Email,t3.Name as Type from Users t1  join UserRole t2 on t1.Id=t2.UserId  join Roles t3 on t2.RoleId=t3.Id where t1.MobileNo=@MobileNo and t1.Password=@Password");
                    parameters = new DynamicParameters();
                    parameters.Add("MobileNo", loginDto.MobileNo, DbType.String);
                    parameters.Add("Password", loginDto.Password, DbType.String);
                    var userObj = await connection.QueryFirstOrDefaultAsync<User>(query, parameters);

                    string token = await _tokenService.CreateToken(userObj);
                    var deviceQueryString = "update Users set DeviceToken=@deviceToken where id=@id";
                    parameters = new DynamicParameters();
                    parameters.Add("deviceToken", loginDto.DeviceToken, DbType.String);
                    parameters.Add("id", userObj.Id, DbType.Guid);
                    var success = await connection.ExecuteAsync(deviceQueryString, parameters);


                    loginResponse.Token = token;
                    

                    loginResponse.Id = userObj.Id;
                    loginResponse.Name = userObj.Name;
                    loginResponse.Email = userObj.Email;
                    loginResponse.Type = type;
                    return loginResponse;
                }
                connection.Close();
                return loginResponse;
            }
        }
        public async Task<RefreshTokenModel> RefreshToken(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = string.Format("select t1.Id,t1.Name,t1.Email,t3.Name from Users t1  join UserRole t2 on t1.Id=t2.UserId  join Roles t3 on t2.RoleId=t3.Id where t1.Email=@Email");

                var parameters = new DynamicParameters();
                parameters.Add("Email", email, DbType.String);
                var userObj = await connection.QueryFirstOrDefaultAsync<User>(query, parameters);

                RefreshTokenModel refreshToken = new RefreshTokenModel();
                if (userObj != null)
                {
                    refreshToken.Token = await _tokenService.CreateToken(userObj);
                }
                
                connection.Close();
                return refreshToken;
            }
        }

        public async Task<UserResponseUpdate> UpdateUserAsync(UserDtoUpdate userDtoUpdate)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    Guid? userId = userDtoUpdate.Id != Guid.Empty ? (Guid?)userDtoUpdate.Id : null;



                //    Guid? subjectId = !string.IsNullOrEmpty(userDtoUpdate.Subject.FirstOrDefault())
                //? Guid.Parse(userDtoUpdate.Subject.FirstOrDefault())
                //: (Guid?)null;

                    //Guid? classId = !string.IsNullOrEmpty(userDtoUpdate.sClass.FirstOrDefault())
                    //    ? Guid.Parse(userDtoUpdate.sClass.FirstOrDefault())
                    //    : (Guid?)null;


                    // Save the uploaded photo
                    string uniqueFileName = null;
                    if (userDtoUpdate.Image != null)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "profileImage"); // Ensure that "uploads" directory exists
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + userDtoUpdate.Image.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await userDtoUpdate.Image.CopyToAsync(fileStream);
                        }

                        var queryString = "update Users Set name=@name,email=@email,password=@password,School=@School,Dob=@Dob,Updateby=@Updateby, Image=@Image where id='" + userId + "'";
                        var parameters = new DynamicParameters();
                        parameters.Add("name", userDtoUpdate.Name, DbType.String);
                        parameters.Add("email", userDtoUpdate.Email, DbType.String);
                        parameters.Add("password", userDtoUpdate.Password, DbType.String);
                        parameters.Add("School", userDtoUpdate.School, DbType.String);
                        parameters.Add("Dob", userDtoUpdate.Dob, DbType.String);
                        parameters.Add("Updateby", DateTime.Now.ToString("yyyy-MM-dd"));
                        parameters.Add("Image", uniqueFileName, DbType.String); // Save the filename to the database
                        var success = await connection.ExecuteAsync(queryString, parameters);

                        string fullPhotoUrl = uniqueFileName != null
                            ? $"https://api.edex365.com/uploads/{uniqueFileName}"
                            : null;

                        UserResponseUpdate userResponseUpdate = new UserResponseUpdate
                        {
                            Name = userDtoUpdate.Name,
                            Email = userDtoUpdate.Email,
                            Password = userDtoUpdate.Password,
                            Dob = userDtoUpdate.Dob,
                            School = userDtoUpdate.School,
                            //sClass = userDtoUpdate.sClass,
                            //Subject = userDtoUpdate.Subject,// Return the full URL
                            Image = fullPhotoUrl
                        };
                        return userResponseUpdate;
                    }
                    else
                    {
                        var queryString = "update Users Set name=@name,email=@email,password=@password,School=@School,Dob=@Dob,Updateby=@Updateby where id='" + userId + "'";
                        var parameters = new DynamicParameters();
                        parameters.Add("name", userDtoUpdate.Name, DbType.String);
                        parameters.Add("email", userDtoUpdate.Email, DbType.String);
                        parameters.Add("password", userDtoUpdate.Password, DbType.String);
                        parameters.Add("School", userDtoUpdate.School, DbType.String);
                        parameters.Add("Dob", userDtoUpdate.Dob, DbType.String);
                        parameters.Add("Updateby", DateTime.Now.ToString("yyyy-MM-dd"));
                        //parameters.Add("Image", uniqueFileName, DbType.String); // Save the filename to the database
                        var success = await connection.ExecuteAsync(queryString, parameters);


                        var imgUrl = "select image from Users where id=@Id ";
                        var parameters1=new DynamicParameters();
                        parameters1.Add("Id",userId, DbType.Guid);

                        var url=await connection.ExecuteScalarAsync(imgUrl, parameters1);

                        string fullPhotoUrl = url != null
                            ? $"https://api.edex365.com/uploads/{url}"
                            : null;

                        UserResponseUpdate userResponseUpdate = new UserResponseUpdate
                        {
                            Name = userDtoUpdate.Name,
                            Email = userDtoUpdate.Email,
                            Password = userDtoUpdate.Password,
                            Dob = userDtoUpdate.Dob,
                            School = userDtoUpdate.School,
                            //sClass = userDtoUpdate.sClass,
                            //Subject = userDtoUpdate.Subject,// Return the full URL
                            Image = fullPhotoUrl
                        };
                        return userResponseUpdate;
                    }

                    


                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> ForgotPasswordSendOTPAsync(ForgotPasswordSendOTP forgotPasswordSendOTP)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    

                    var query = "SELECT COUNT(1) FROM Users WHERE MobileNo = @MobileNo";
                    var count = await connection.ExecuteScalarAsync<int>(query, new { MobileNo = forgotPasswordSendOTP.MobileNo });

                    if (count > 0)
                    {
                        connection.Open();
                        var random = new Random();
                        var otp = random.Next(100000, 999999).ToString();

                        var deviceQueryString = "update Users set OTP=@OTP where MobileNo=@MobileNo";
                        var parameters = new DynamicParameters();
                        parameters.Add("OTP", otp, DbType.String);
                        parameters.Add("MobileNo", forgotPasswordSendOTP.MobileNo, DbType.String);
                        var success = await connection.ExecuteAsync(deviceQueryString, parameters);
                        if (success > 0)
                        {

                            using (WebClient client = new WebClient())
                            {
                                var domain = "https://";
                                var message = "Edex365 Forgot Password OTP Number is " + otp + " ";
                                string url = "" + domain + "bulksmsbd.net/api/smsapi?api_key=AQVhdExRIVugmhs0fsNE&type=text&number=" + forgotPasswordSendOTP.MobileNo + "&senderid=8809617620256&message=" + message + "";
                                client.DownloadString(url);
                            }
                        }
                        return true;

                    }
                    
                    return false;



                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> ForgotPasswordVerifyOTPAsync(ForgotPasswordUpdateOTP forgotPasswordUpdateOTP)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {


                    var query = "SELECT OTP FROM Users WHERE MobileNo = @MobileNo";
                    var OTP = await connection.ExecuteScalarAsync<string>(query, new { MobileNo = forgotPasswordUpdateOTP.MobileNo });

                    if (OTP== forgotPasswordUpdateOTP.OTP)
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

        public async Task<bool> ForgotPasswordConfirmAsync(ForgotPasswordConfirm forgotPasswordConfirm)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {



                        var deviceQueryString = "update Users set Password=@Password where MobileNo=@MobileNo";
                        var parameters = new DynamicParameters();
                        parameters.Add("Password", forgotPasswordConfirm.Password, DbType.String);
                        parameters.Add("MobileNo", forgotPasswordConfirm.MobileNo, DbType.String);
                        var success = await connection.ExecuteAsync(deviceQueryString, parameters);
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
