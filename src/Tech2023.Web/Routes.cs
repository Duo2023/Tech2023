using Tech2023.Web.Shared;

namespace Tech2023.Web;

/*
 * This collection of route constants are all mapped to routes and these are annotated accordingly to what the controller does
 * The visibly dictates what level of visiblity the user needs to access the page
 * 
 * When you add another controller method create the route and then tag the controller method with [Route(Routes.*)] and add the level of visibility
 * These attributes get used in integration tests to test routability and authorization
 */

public sealed class Routes
{
    [Obsolete] // this class should never be instantiated
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
        
        // [RouteVisibility(Visiblity.Authenticated)]
        public const string PaperBrowser = "/browse/{curriculum}/{subject}";
    }

    public static class Settings
    {
        [RouteVisibility(Visiblity.Public)]
        public const string Home = "/settings";
    }

    public static class Admin
    {
        [RouteVisibility(Visiblity.Adminstrator)]
        public const string Home = "/admin";
    }

    public static class Account
    {
        [RouteVisibility(Visiblity.Public)]
        public const string Login = "/account/login";

        [RouteVisibility(Visiblity.Public)]
        public const string Register = "/account/register";

        [RouteVisibility(Visiblity.Public)]
        public const string ConfirmEmail = "/account/confirm_email";

        [RouteVisibility(Visiblity.Public)]
        public const string ResendEmailConfirmation = "/account/resend_email_confirmation";

        [RouteVisibility(Visiblity.Public)]
        public const string RegisterConfirmation = "/account/register_confirmation";

        [RouteVisibility(Visiblity.Public)]
        public const string ForgotPassword = "/account/forgot_password";

        [RouteVisibility(Visiblity.Public)]
        public const string Lockout = "/account/lockout";
    }
}
