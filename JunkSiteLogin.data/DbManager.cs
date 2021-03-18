using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace JunkSiteLogin.data
{
    public class Ad
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }

    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHashed { get; set; }

    }
    public class DbManager
    {
        private string _connectionString;
        public DbManager(string constr)
        {
            _connectionString = constr;
        }
        public List<Ad> GetAllAds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Ads";
            connection.Open();
            List<Ad> list = new List<Ad>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Ad a = new Ad();

                a.Id = (int)reader["id"];
                a.PhoneNumber = (string)reader["phoneNumber"];
                a.Description = (string)reader["description"];
                a.UserId = (int)reader["userId"];
                a.UserName = GetUserName(a.Id);
                a.Date = (DateTime)reader["date"];
                list.Add(a);
          
            }
            return list;
        }
        private string GetUserName (int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT Name FROM users WHERE id=@id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            return (string)command.ExecuteScalar();
        }
        public User LogIn(string email, string password)
        {
            var user = GetByEmail(email);
            if(user == null)
            {
                return null;
            }
            bool verified = BCrypt.Net.BCrypt.Verify(password, user.PasswordHashed);
            return verified ? user : null;
        }
        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Users WHERE email = @email";
            command.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            else
            {
                return new User
                {
                    Id = (int)reader["id"],
                    Name = (string)reader["name"],
                    Email = (string)reader["email"],
                    PasswordHashed = (string)reader["passwordHashed"]
                };
            }
        }
        public void AddUser(string name, string email, string password)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Users(name, email, passwordHashed)
                                    VALUES (@name, @email, @passwordHashed)";
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@email", email);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            command.Parameters.AddWithValue("@passwordHashed", hashedPassword);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public void AddAd(int userId, string phoneNumber, string description)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Ads(userId, description, phoneNumber, date)
                                    VALUES(@userId, @description, @phoneNumber, @date)";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
            command.Parameters.AddWithValue("@date", DateTime.Now);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public void DeleteAd(int adId)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM Ads WHERE Id = @adId";
            command.Parameters.AddWithValue("@adId", adId);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public List<Ad> GetAdsForUser(int userId)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Ads WHERE userId = @userId";
            command.Parameters.AddWithValue("@userId", userId);
            connection.Open();
            var reader = command.ExecuteReader();
            var list = new List<Ad>();
       
            while (reader.Read())
            {
                list.Add(new Ad
                {
                    Id = (int)reader["id"],
                    Date = (DateTime)reader["date"],
                    Description = (string)reader["description"],
                    PhoneNumber = (string)reader["phoneNumber"],
                    UserId = (int)reader["userId"],
                    UserName = GetUserName(userId)
                });
            }
            return list;
        }
    }
    
}
