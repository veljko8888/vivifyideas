using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testVeljkara.ApplicationConstants
{
    public static class AppConstants
    {
        public const string WrongUsernameOrPassword = "Wrong Username or Password.";
        public const string ErrorSavingUser = "There was an error while saving a user.";
        public const string ErrorSavingLink = "There was an error while saving a Link.";
        public const string UsernameAlreadyExist = "Username already exists.";
        public const string LinkAlreadyExists = "Link already exists.";
        public const string LinkDoesNotExist = "Link does not exist.";
        public const string ErrorRemovingLink = "Error while removing Link.";
        public const string LinkNotValid = "Link is not valid.";
        public const string ErrorCountingUsers = "Error while counting users with the same link.";
        
    }
}
