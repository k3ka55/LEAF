using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessEntities;
using BusinessEntities.Entity;

namespace BusinessServices
{
    public interface ILoginServices
    {
        loginReposnse login(UserDetailsEntity user);
        bool changePassword(UserDetailsEntity userEntity);
        bool forgetPassword(string userName);
        List<MobileDCLocations> GetLoginUserLocations(UserDetailsEntity user);
    }
}
