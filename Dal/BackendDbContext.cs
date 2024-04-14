
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Movement_be.Entities;
using Movement_be.Interfaces;
using Movement_be.Models.Dto;
using Newtonsoft.Json;
using System;

namespace Movement_be.Dal
{
    public class BackendDbContext : DbContext
    {
        private readonly string _requestUri;

        IHttpService _httpService;
        public DbSet<User> Users { get; set; }                

        public BackendDbContext(DbContextOptions<BackendDbContext> options, IConfiguration configuration, IHttpService httpService)
        : base(options)
        {
            _httpService = httpService;
            _requestUri = configuration["serverApiUri"];
        }

        #region FluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();           
           
        }
        #endregion

        public async Task SeedMockData()
        {
            Users.Add(new User
            {
                Id = 999,
                FirstName = "Admin",
                LastName = "1",
                Email = "admin@movement.com",
                Password = "1234",
                Avatar = string.Empty
            });

            var result = await GetResponseObject<DataArrayResponseDto>("api/users", HttpMethod.Get, string.Empty);
            
            if (result != null)
            {
                foreach (var item in result.data)
                {
                    try
                    {
                        Users.Add(new User()
                        {
                            Id = item.Id,
                            Email = item.Email,
                            Password = item.Password,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Avatar = item.Avatar
                        });
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

            try
            {
                SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<T> GetResponseObject<T>(string uri, HttpMethod httpMethod, string parameterString = null) where T : new()
        {
            try
            {
                string res = await _httpService.CallRestApi(httpMethod, _requestUri + uri, parameterString);
                T resObject = JsonConvert.DeserializeObject<T>(res);
                return resObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
