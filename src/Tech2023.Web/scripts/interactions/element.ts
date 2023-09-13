export module element {
    export function toggleHiddenElement(id: string): void {
        let element = document.getElementById(id) as HTMLElement;

        if (element.classList.contains("hidden")) {
            element.classList.remove("hidden");
        } else {
            element.classList.add("hidden");
        }
    }
}