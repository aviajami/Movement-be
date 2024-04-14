using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Movement_be.AuthorizeHelpers;
using Movement_be.Bl;
using Movement_be.Entities;
using Movement_be.Interfaces;
using Movement_be.Models.Dto;

namespace Movement_be.Dal
{
    public class UsersDal : IUsersDal
    {
        private readonly ILogger<UsersDal> _logger;
        private readonly BackendDbContext _dbContext;

        public UsersDal(ILogger<UsersDal> logger, BackendDbContext dbContext)
        {
            _logger = logger;
           _dbContext = dbContext;
        }

        public async Task<User> AddUser(User user)
        {
            _logger.LogInformation($"AddUser - Enter");
            
            var addedEntity = _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            addedEntity.State = EntityState.Detached;
            _logger.LogInformation($"AddUser - Exit");
            return addedEntity.Entity;
        }

        public async Task<User> DeleteUserById(int id)
        {
            _logger.LogInformation($"DeleteUserById - Enter");
            var userToDelete = new User { Id = id };
            var deletedEntity = _dbContext.Users.Remove(userToDelete);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"DeleteUserById - Exit");
            return deletedEntity.Entity;
        }

        public async Task<IList<User>> GetUsers(int page)
        {
            _logger.LogInformation($"GetUsers - Enter");
            //var retValue = await _dbContext.Users.AsNoTracking().ToListAsync();
            var result = await _dbContext.GetResponseObject<DataArrayResponseDto>($"api/users?page={page}", HttpMethod.Get, string.Empty);
            var missingRecords = result.data.Where(x => !_dbContext.Users.Any(user => user.Id == x.Id)).ToList();
            _dbContext.Users.AddRange(missingRecords);
            _dbContext.SaveChanges();

            _logger.LogInformation($"GetUsers - Exit");
            return result.data;
        }

        public Task<User?> GetUser(string userEmail, string password)
        {
            _logger.LogInformation($"GetUser - Enter");
            return _dbContext.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == userEmail.ToLower() && user.Password == password);
        }

        public async Task<User> GetUserById(int id)
        {
            _logger.LogInformation($"GetUserById - Enter");

            var user = _dbContext.Users.FirstOrDefault(user => user.Id == id);

            if (user == null)
            {
                _logger.LogInformation($"GetUserById - Get from API");

                var result = await _dbContext.GetResponseObject<DataResponseDto>($"api/users/{id}", HttpMethod.Get, string.Empty);
                user = result.data;
            }
            _logger.LogInformation($"GetUserById - Exit");
            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            _logger.LogInformation($"UpdateUser - Enter");
            var updatedEntity = _dbContext.Users.Update(user);            
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"UpdateUser - Exit");
            return updatedEntity.Entity;
        }
    }
}
