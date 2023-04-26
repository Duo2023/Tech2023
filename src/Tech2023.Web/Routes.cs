﻿using Tech2023.Web.Shared;

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
    }

    public static class User
    {
        [RouteVisibility(Visiblity.Public)]
        public const string Login = "/login";

        [RouteVisibility(Visiblity.Public)]
        public const string Register = "/register";

        [RouteVisibility(Visiblity.Public)]
        public const string ForgotPassword = "/forgot-password";

        [RouteVisibility(Visiblity.Public)]
        public const string ResetPassword = "/reset-password";
    }
}
