using System;
using System.Collections.Generic;
using System.Text;

namespace NuUserStoreManager
{
    public static class AppMessages
    {
        public static readonly string NuApplicationNull = "NuApplication user is null.";

        public static string UserNotFound(string username) { return $"User not found. Username: {username}."; }
        public static string NotSupported(string methodName) { return $"{methodName} is not supported."; }
        public static string DuplicateUsername(string username) {return $"Username: {username} is duplicated in the database."; }
        public static string EmptyString(string str) { return $"{str} is empty string."; }
        public static string Error(string methodName, string username) { return $"Error invoking {methodName}. Username: {username}."; }
    }
}
