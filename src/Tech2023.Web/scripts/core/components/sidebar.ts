export module core.sidebar {
    export function toggleSidebar() {
        let root = document.querySelector(":root") as HTMLElement;
        let sidebar = document.getElementById("sidebar") as HTMLDivElement;
        let sidebarButton = document.getElementById("sidebar-toggle-btn") as HTMLButtonElement;

        if (sidebar.dataset.expanded == "true") {
            sidebar.dataset.expanded = "false";
            sidebar.ariaExpanded = sidebarButton.ariaExpanded = "false";
            root.style.setProperty("--sidebar-width", "var(--sidebar-collapsed-width)");
            sidebarButton.innerHTML = "left_panel_open";
        } else {
            sidebar.dataset.expanded = "true";
            sidebar.ariaExpanded = sidebarButton.ariaExpanded = "true";
            root.style.setProperty("--sidebar-width", "var(--sidebar-expanded-width)");
            sidebarButton.innerHTML = "left_panel_close";
        }
    }
}
