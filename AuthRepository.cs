using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VMSAPIWEB.Models;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace VMSAPIWEB
{

    public class AuthRepository
    {
        private VMS2021DBEntities db = new VMS2021DBEntities();

        public dynamic dynValidateUser(string userName, string passsword)
        {
            var User = ValidateUser(userName, passsword);
            if (User == null)
            {
                var AdminUser = ValidateAdmin(userName, passsword);
                return AdminUser;
            }
            else
            {
                return User;
            }

        }
        public User_tbl ValidateUser(string userName, string passsword)
        {
            User_tbl user = null;
            var checkUser = from d in db.User_tbl where d.UserName == userName && d.Password == passsword select d;
            if (checkUser != null && checkUser.Count() > 0)
            {
                user = new User_tbl();
                user.UserName = checkUser.First().UserName;
                user.UserType = checkUser.First().UserType;
                user.UserId = checkUser.First().UserId;
                user.CompanyId = checkUser.First().CompanyId;

                return user;
            }
            else
            {
                return user;
            }

        }
        public User_tbl ValidateAdmin(string userName, string passsword)
        {
            User_tbl user = null;
            var checkUser = from d in db.User_tbl where d.UserName == userName && d.Password == passsword select d;
            if (checkUser != null && checkUser.Count() > 0)
            {
                user = new User_tbl();
                user.UserName = checkUser.First().UserName;
                user.UserType = checkUser.First().UserType;
                user.UserId = checkUser.First().UserId;
                user.CompanyId = checkUser.First().CompanyId;
                return user;
            }
            else
            {
                return user;
            }

        }
    }
}