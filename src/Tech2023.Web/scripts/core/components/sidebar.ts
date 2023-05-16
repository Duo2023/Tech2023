export module core.sidebar {
    export function toggleSidebar() {
        let sidebar = document.getElementById("sidebar") as HTMLDivElement;
        let sidebarButton = document.getElementById("sidebar-toggle-btn") as HTMLButtonElement;

        if (sidebar.dataset.expanded == "true") {
            sidebar.dataset.expanded = "false";
            sidebar.ariaExpanded = sidebarButton.ariaExpanded = "false";
            sidebarButton.innerHTML = "left_panel_open";
        } else {
            sidebar.dataset.expanded = "true";
            sidebar.ariaExpanded = sidebarButton.ariaExpanded = "true";
            sidebarButton.innerHTML = "left_panel_close";
        }
    }
}
