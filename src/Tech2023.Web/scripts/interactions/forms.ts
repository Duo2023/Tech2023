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

    export module addSubjects {
        export function updateOptionsBasedOnCurriculum(selectSubjectsId: string, selectCurriculumId: string) {
            let selectSubjects = document.getElementById(selectSubjectsId) as HTMLSelectElement;
            let selectSubjectOptions = Array.from(selectSubjects.options) as Array<HTMLOptionElement>;
            let selectCurriculum = document.getElementById(selectCurriculumId) as HTMLSelectElement;
            let selectedCurriculumData = selectCurriculum.selectedOptions[0].dataset.curriculum;

            selectSubjectOptions.forEach((option) => {
                if (option.dataset.curriculum == selectedCurriculumData) {
                    option.classList.remove("hidden");
                } else {
                    option.classList.add("hidden");
                }
            });
        }
    }
}
