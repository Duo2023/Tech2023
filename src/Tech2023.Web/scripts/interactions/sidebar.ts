import interact from "interactjs";

export module sidebar {
    export function mobileSidebarSwipeUpInit(sidebarId: string, displacementCSSVariableName: string) {
        const root = getComputedStyle(document.querySelector(":root"));
        const sidebar = document.getElementById(sidebarId) as HTMLDivElement;

        const sidebarStyle = getComputedStyle(sidebar);
        const originalYTranslate = root.getPropertyValue(displacementCSSVariableName);

        const swipeDistance: number = 20;

        registerSwipeUp();
        window.onresize = (event) => registerSwipeUp();

        function registerSwipeUp() {
            // If it is not in "mobile" mode, then reset events and styling
            if (sidebarStyle.position !== "fixed") {
                interact(`#${sidebarId}`).unset();
                sidebar.style.transform = `translate(0px, 0px)`;
                return;
            }

            // The styling is reset when going to "mobile" mode
            sidebar.style.transform = `translate(0px, ${originalYTranslate})`;

            interact(`#${sidebarId}`).draggable({
                startAxis: "y",
                lockAxis: "y",

                listeners: {
                    start(event) {
                        console.log(event.type, event.target);
                    },
                    move(event) {
                        console.log(event.dy);

                        if (event.dy <= -swipeDistance) {
                            sidebar.style.transform = `translate(0px, 0px)`;
                        } else if (event.dy >= swipeDistance) {
                            sidebar.style.transform = `translate(0px, ${originalYTranslate})`;
                        }
                    },
                },
            });
        }
    }
}
