using System.ComponentModel.DataAnnotations;
  
namespace WebApplicationNeoPharm.Authenticate
{
    public class ClsTokenManager
    {
        private static string Api_Key = "d3468ca2-c0cd-4a39-947d-94f77bf205c5";

        private static string Secret = "ERMN05OPLoDvbTTa/QkqLNMI7cPLguaRyHzyg7n5qNBVjQmtBhz4SzYh4NBVCXi3KJHlSXKP+oi2+bXr6CUYTR==";
        

    }
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }


    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
       
    }


    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}