using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SurfsUpAPI.Data
{
    public class SurfsUpContext : IdentityDbContext<IdentityUser> // IdentityDbContext<IdentityUser>
    {
        string connectionString = "Server = 10.56.8.36; Database=PEDB14;User Id = PE-14; Password=OPENDB_14;";
        //"Server=(localdb)\\mssqllocaldb;Database=SurfsUp;Trusted_Connection=True;MultipleActiveResultSets=true";
        public SurfsUpContext(DbContextOptions<SurfsUpContext> options)
            : base(options)
        {
        }

        public DbSet<SurfsUpAPI.Models.Board> Board { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public void SaveRenting(DateTime startDate, DateTime endDate, string userId, Guid boardId)
        {
            // Prepare a premade sql stored procedure query with all the right data
            string storedSql = "EXEC InsertRenting @StartDate,@EndDate,@UserId,@BoardId";

            // Create the connection (and be sure to dispose it at the end)
            using (SqlConnection cnn = new(connectionString))
            {
                try
                {
                    cnn.Open();
                    using (SqlCommand cmd = new(storedSql, cnn))
                    {
                        // Create and set the parameters values 
                        // We use "Parameters.Add" instead of "Parameters.AddWithValue" because this way, the method will check if the datatype maches 
                        // before the program runs, so it won't crash. "Parameters.AddWithValue" will try to guess the datatype, and isn't always correct.
                        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime2).Value = DateTime.Now;
                        cmd.Parameters.Add("@EndDate", SqlDbType.DateTime2).Value = endDate;
                        cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = userId;
                        cmd.Parameters.Add("@BoardId", SqlDbType.UniqueIdentifier).Value = boardId;

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public void DeleteUserRentings(string userId)
        {
            // Prepare a premade sql stored procedure query with all the right data
            string storedSql = "EXEC RemoveRentingsForUser @UserId";

            // Create the connection (and be sure to dispose it at the end)
            using (SqlConnection cnn = new(connectionString))
            {
                try
                {
                    cnn.Open();
                    using (SqlCommand cmd = new(storedSql, cnn))
                    {
                        // Create and set the parameters values 
                        // We use "Parameters.Add" instead of "Parameters.AddWithValue" because this way, the method will check if the datatype maches 
                        // before the program runs, so it won't crash. "Parameters.AddWithValue" will try to guess the datatype, and isn't always correct.
                        cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = userId;


                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public DbSet<SurfsUpAPI.Models.Rentals> Rentings { get; set; }
    }
}
