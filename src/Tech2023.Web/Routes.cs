using Tech2023.Web.Shared;

namespace Tech2023.Web;

public static class Routes
{
    [PublicRoute]
    public const string Home = "/";

    [PublicRoute]
    public const string Privacy = "/privacy";

    public static class Application
    {
        public const string Home = "/app";
    }

    public static class User
    {
        [PublicRoute]
        public const string Login = "/login";

        [PublicRoute]
        public const string Register = "/register";

        [PublicRoute]
        public const string ForgotPassword = "/forgot-password";

        public const string ResetPassword = "/reset-password";
    }
}
