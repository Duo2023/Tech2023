export module forms {
    export function togglePasswordVisibility(fieldIDs: string[]) {
        const showPasswordCheckbox = document.getElementById("show-password-checkbox") as HTMLInputElement;

        let passwordFields = fieldIDs.map((fieldID) => {
            return document.getElementById(fieldID) as HTMLInputElement;
        });

        if (showPasswordCheckbox.checked) {
            passwordFields.forEach((field) => {
                field.type = "text";
            });
        } else {
            passwordFields.forEach((field) => {
                field.type = "password";
            });
        }
    }
}
