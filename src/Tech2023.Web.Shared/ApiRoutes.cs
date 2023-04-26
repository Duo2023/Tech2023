namespace Tech2023.Web.Shared;

/// <summary>
/// Constants of the api routes provided by the web api project
/// </summary>
public sealed class ApiRoutes
{
    public const string Base = "/api";

    public static class Privacy
    {
        [RouteVisibility(Visiblity.Public)]
        public const string Base = $"{ApiRoutes.Base}/privacy";
    }

    public static class Users
    {
        public const string Base = $"{ApiRoutes.Base}/user";

        // api is not test ready yet
        //[RouteVisibility(Visiblity.Public, Base)]
        public const string Register = "register";

        //[RouteVisibility(Visiblity.Public, Base)]
        public const string Login = "login";

        //[RouteVisibility(Visiblity.Public, Base)]
        public const string ForgotPassword = "forgot_password";
    }

    public static class Statistics
    {
        public const string Base = $"{ApiRoutes.Base}/statistics";

        [RouteVisibility(Visiblity.Public, Base)]
        public const string Ping = "ping";
    }
}
