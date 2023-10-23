﻿using FairPlayImporter.Model;

namespace FairPlayImporter.Repository
{
    public interface IUserRepo
    {
        Task<IList<User>> GetUsersByName(string name);
        Task<User> CreateUser(string name);
        Task<User> UpdateUser(User user);
    }
}
