﻿using ShuffleLit.Models;

namespace ShuffleLit.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);
        ICollection<AppUser> GetAppUsers();
        AppUser GetAppUser(string appUserId);
        ICollection<AppUser> GetAppUserOfALiterature(int literatureId);
        ICollection<Literature> GetLiteratureByAppUserId(string appUserId);
        bool Update(AppUser user);
        bool Save();
    }
}
