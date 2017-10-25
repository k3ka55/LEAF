using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessEntities;

namespace BusinessServices
{
    public interface IUserDetailsServices
    {
        IEnumerable<UserDetailsEntity> GetAllUsers(int? roleId, string Url);
        List<UserDetailsEntity> GetUserById(int userId);
        int CreateUser(UserDetailsEntity userEntity);
        bool UpdateUser(int userId, UserDetailsEntity userEntity);
        bool DeleteUser(int userId);
        bool ValidateUser(string userName);
        IEnumerable<UserModel> GetAll();
    }
}
