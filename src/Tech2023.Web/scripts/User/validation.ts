export module user.validation {
    export function validateLoginForm() {
        let loginForm = document.getElementById("login-form");
        let usernameField = document.getElementById("username") as HTMLInputElement;
        let passwordField = document.getElementById("password") as HTMLInputElement;

        if (usernameField.value === "") {
            loginForm.dataset.invalid = "empty-username";
        } else if (passwordField.value === "") {
            loginForm.dataset.invalid = "empty-password";
        } else {
            /* For now deny all login attempts
             *  Also check the onsubmit= attribute in the form as its set to return false;
             *   aka not submitting the form
             */
            loginForm.dataset.invalid = "all";
        }
    }
    export function validateRegisterForm() {
        let registerForm = document.getElementById("register-form") as HTMLFormElement;
        let usernameField = document.getElementById("username") as HTMLInputElement;
        let passwordField = document.getElementById("password") as HTMLInputElement;
        let confirmPasswordField = document.getElementById("confirm-password") as HTMLInputElement;

        console.log(passwordField.pattern);

        if (usernameField.value === "") {
            registerForm.dataset.invalid = "empty-username";
        } else if (!passwordField.value.match(passwordField.pattern)) {
            registerForm.dataset.invalid = "invalid-password";
        } else if (passwordField.value !== confirmPasswordField.value) {
            registerForm.dataset.invalid = "passwords-do-not-match";
        } else {
            registerForm.dataset.invalid = "none";
        }
        registerForm.requestSubmit();
    }
}
