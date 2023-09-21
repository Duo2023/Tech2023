export module element {
    function setupAutoHide(element: HTMLElement, marginOfError: number = 30): void {
        let MarginOfError: number = marginOfError;
        document.getElementById("main").addEventListener("click", (e) => {
            const dialogDimensions = element.getBoundingClientRect();
            if (
                e.clientX < dialogDimensions.left - MarginOfError ||
                e.clientX > dialogDimensions.right + MarginOfError ||
                e.clientY < dialogDimensions.top - MarginOfError ||
                e.clientY > dialogDimensions.bottom + MarginOfError
            ) {
                element.classList.add("hidden");
            }
        });
    }

    export function toggleHiddenElement(id: string, autoHide: boolean = false): void {
        let element = document.getElementById(id) as HTMLElement;

        if (element.classList.contains("hidden")) {
            element.classList.remove("hidden");
            if (autoHide) {
                console.log("roget");
                setupAutoHide(element);
            }
        } else {
            element.classList.add("hidden");
        }
    }
}
