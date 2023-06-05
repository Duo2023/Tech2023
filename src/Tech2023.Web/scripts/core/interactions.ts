export module interactions {
    export function toggleHiddenElement(id: string): void {
        let element = document.getElementById(id) as HTMLElement;

        if (element.classList.contains("hidden")) {
            element.classList.remove("hidden");
        } else {
            element.classList.add("hidden");
        }
    }

    export function showDialog(id: string, asModal: boolean): void {
        let dialog = document.getElementById(id) as HTMLDialogElement;
        if (asModal) {
            dialog.showModal();
        } else {
            dialog.show();
        }
    }

    export function closeDialog(id: string): void {
        let dialog = document.getElementById(id) as HTMLDialogElement;
        dialog.close();
    }
}
