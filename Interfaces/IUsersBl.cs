using Movement_be.AuthorizeHelpers;
using Movement_be.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movement_be.Interfaces
{
    public interface IUsersBl
    {
        Task<IList<User>> GetUsers(int page);

        Task<User> GetUserById(int id);

        Task<User> AddUser(User user);

        Task<User> UpdateUser(User user);

        Task<User> DeleteUserById(int id);
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest request);
    }
}
