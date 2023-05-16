export module user.forms {
    export function togglePasswordVisibility() {
        const showPasswordCheckbox = document.getElementById("show-password-checkbox") as HTMLInputElement;
        let passwordFields = [
            document.getElementById("Password"),
            document.getElementById("ConfirmPassword"),
        ] as HTMLInputElement[];

        if (showPasswordCheckbox.checked) {
            passwordFields.forEach((field) => {
                if (field != null) {
                    field.type = "text";
                }
            });
        } else {
            passwordFields.forEach((field) => {
                if (field != null) {
                    field.type = "password";
                }
            });
        }
    }
}
