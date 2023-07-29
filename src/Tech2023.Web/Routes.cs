using Tech2023.Web.Shared;

namespace Tech2023.Web;

// this is a public sealed class so it can be used in generics
public sealed class Routes
{
    private Routes() { }

    [RouteVisibility(Visiblity.Public)]
    public const string Home = "/";

    [RouteVisibility(Visiblity.Public)]
    public const string Privacy = "/privacy";

    public static class Application
    {
        [RouteVisibility(Visiblity.Authenticated)]
        public const string Home = "/app";

        [RouteVisibility(Visiblity.Authenticated)]
        public const string PaperViewer = "/paper";
    }

    public static class Settings
    {
        public const string Home = "/settings";
    }

    public static class Admin
    {
        public const string Home = "/admin";
    }

    public static class Account
    {
        public const string Login = "/account/login";
        public const string Register = "/account/register";
        public const string ConfirmEmail = "/account/confirm_email";
        public const string ResendEmailConfirmation = "/account/resend_email_confirmation";
        public const string RegisterConfirmation = "/account/register_confirmation";
        public const string ForgotPassword = "/account/forgot_password";
        public const string Lockout = "/account/lockout";
    }

    // replace when routing is sorted

    //public static class User
    //{
    //    //[RouteVisibility(Visiblity.Public)]
    //    public const string Login = "/login";

    //    [RouteVisibility(Visiblity.Public)]
    //    public const string Register = "/register";

    //    [RouteVisibility(Visiblity.Public)]
    //    public const string ForgotPassword = "/forgot-password";

    //    [RouteVisibility(Visiblity.Public)]
    //    public const string ResetPassword = "/reset-password";
    //}
}
