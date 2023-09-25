export module dialog {
    export function showDialog(id: string, asModal: boolean, inputString: string = ""): void {
        let dialog = document.getElementById(id) as HTMLDialogElement;
        if (asModal) {
            try {
                document.getElementById(`${id}-input-target`).innerText = inputString;
            } catch (e) {
                console.error(`Input target for dialog ${id} was not found`);
            }
            dialog.showModal();
        } else {
            dialog.show();
        }

        // To auto hide the dialog if the user clicks outside the dialog element:
        dialog.addEventListener("click", (e) => {
            const dialogDimensions = dialog.getBoundingClientRect();
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
