
using Microsoft.Extensions.Logging;
using Movement_be.AuthorizeHelpers;
using Movement_be.Entities;
using Movement_be.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movement_be.Bl
{
    public class UsersBl : IUsersBl
    {
        private readonly ILogger<UsersBl> _logger;
        private readonly IUsersDal _usersDal;

        public UsersBl(ILogger<UsersBl> logger, IUsersDal usersDal)
        {
            _logger = logger;
            _usersDal = usersDal;
        }

        public async Task<IList<User>> GetUsers(int page)
        {
            _logger.LogInformation($"GetAllUser - Enter");
            var retValue = await _usersDal.GetUsers(page);
            _logger.LogInformation($"GetAllUser - Exit");
            return retValue;
        }

        public async Task<User> GetUserById(int id)
        {
            _logger.LogInformation($"GetUserById - Enter");
            var retValue = await _usersDal.GetUserById(id);
            _logger.LogInformation($"GetUserById - Exit");
            return retValue;
        }

        public async Task<User> AddUser(User user)
        {
            _logger.LogInformation($"AddUser - Enter");
            var retValue = await _usersDal.AddUser(user);
            _logger.LogInformation($"AddUser - Exit");
            return retValue;
        }

        public async Task<User> UpdateUser(User user)
        {
            _logger.LogInformation($"UpdateUser - Enter");
            var retValue = await _usersDal.UpdateUser(user);
            _logger.LogInformation($"UpdateUser - Exit");
            return retValue;
        }

        public async Task<User> DeleteUserById(int id)
        {
            _logger.LogInformation($"DeleteUserById - Enter");
            var retValue = await _usersDal.DeleteUserById(id);
            _logger.LogInformation($"DeleteUserById - Exit");
            return retValue;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            _logger.LogInformation($"GetUser - Enter");
            var user = await _usersDal.GetUser(request.Username, request.Password);
            if (user == null)
                return null;
        

            var token = JwtMiddleware.GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }
    }
}
