/**
 * Contains functions and data types for working with the statistics of the web API
 */
export namespace statistics {
    /**
     * The same as Web.Shared/Statistics/PingResponse.cs but in TypeScript
     */
    export interface PingResponse {
        runtime: number; // The json is serialized as a long so keep it as a long here
        ip: string;
    }

    export async function updateButton(): Promise<void> {
        const status = document.querySelector("#status-text") as HTMLParagraphElement;
        if (await isApiUp()) {
            status.textContent = "200";
            status.style.backgroundColor = "#45fe57";
        } else {
            status.textContent = "400";
            status.style.backgroundColor = "#ff3d3d";
        }
    }

     /**
     * Checks if the API is up and running by sending a ping request to the server.
     * 
     * @returns {Promise<boolean>} A Promise that resolves to true if the API is up and running, or false otherwise.
     */
    export async function isApiUp(): Promise<boolean> {
        try {
            const response = await fetch('https://localhost:7098/api/statistics/ping');

            if (!response.ok) {
                console.error('error getting data from api');
                return false;
            }

            const json = await response.text();

            const pingResponse: PingResponse = JSON.parse(json);

            console.log(pingResponse.runtime); // Output the runtime value
            console.log(pingResponse.ip); // Output the IP value

            return true;
        } catch (error) {
            console.error('Error fetching the PingResponse:', error);
            return false;
        }
    }
}