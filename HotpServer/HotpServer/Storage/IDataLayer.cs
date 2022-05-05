using HotpServer.Storage.Models;

namespace HotpServer.Storage
{
    public interface IDataLayer
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByLoginAsync(string login);
        Task AddOrUpdateUserAsync(User user);
    }
}
