export module dialog {
    export function showDialog(id: string, asModal: boolean): void {
        let dialog = document.getElementById(id) as HTMLDialogElement;
        if (asModal) {
            dialog.showModal();
        } else {
            dialog.show();
        }

        // To auto hide the dialog if the user clicks outside the dialog element:
        dialog.addEventListener("click", (e) => {
            const dialogDimensions = dialog.getBoundingClientRect();

            // To avoid hiding the dialog when user clicks on a select element:
            if (e.clientX == 0 && e.clientY == 0) {
                return;
            }

            if (
                e.clientX < dialogDimensions.left ||
                e.clientX > dialogDimensions.right ||
                e.clientY < dialogDimensions.top ||
                e.clientY > dialogDimensions.bottom
            ) {
                dialog.close();
            }
        });
    }

    export function closeDialog(id: string): void {
        let dialog = document.getElementById(id) as HTMLDialogElement;
        dialog.close();
    }
}
