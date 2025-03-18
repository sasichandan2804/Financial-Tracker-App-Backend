using System.Data.Common;
using Auth_System_Project.Model;
using Microsoft.Data.SqlClient;

namespace Auth_System_Project.DataAccess
{
    public class AuthDL : IAuthDL
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _sqlConnection;
        public AuthDL(IConfiguration configuration)
        {
            _configuration = configuration;
            _sqlConnection = new SqlConnection(_configuration[key: "ConnectionStrings:DefaultConnection"]);
        }

        public async Task<SignInResponse> SignIn(SignInRequest request)
        {
            SignInResponse response = new SignInResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {

                if (_sqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _sqlConnection.OpenAsync();
                }

                string SqlQuery = @"select * from UserDetail where UserName=@UserName and PassWord=@PassWord";
                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;//seconds
                    sqlCommand.Parameters.AddWithValue(parameterName: "@UserName", request.UserName);
                    sqlCommand.Parameters.AddWithValue(parameterName: "@PassWord", request.Password);
                    using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (dataReader.HasRows)
                        {
                            response.Message = "Login Successful";
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "Login Failed";
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _sqlConnection.CloseAsync();
                await _sqlConnection.DisposeAsync();
            }
            return response;
        }

        public async Task<SignUpResponse> SignUp(SignUpRequest request)
        {
            SignUpResponse response = new SignUpResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (!request.Password.Equals(request.ConfirmPassword))
                {
                    response.IsSuccess = false;
                    response.Message = "Password And Confirm Password Not Match";
                    return response;
                }

                if (_sqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _sqlConnection.OpenAsync();
                }

                string SqlQuery = @"insert into UserDetail(UserName,PassWord) Values(@UserName,@PassWord)";
                using (SqlCommand sqlCommand=new SqlCommand(SqlQuery,_sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;//seconds
                    sqlCommand.Parameters.AddWithValue(parameterName: "@UserName", request.UserName);
                    sqlCommand.Parameters.AddWithValue(parameterName: "@PassWord", request.Password);
                    int Status = await sqlCommand.ExecuteNonQueryAsync();
                    if (Status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Something Wnet Worng";
                        return response;
                    }
                }
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _sqlConnection.CloseAsync();
                await _sqlConnection.DisposeAsync();
            }
            return response;
        }
    }
}
