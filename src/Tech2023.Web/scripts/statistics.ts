/**
 * Contains functions and data types for working with the statistics of the web API
 */
export module statistics {
    /**
     * The same as Web.Shared/Statistics/PingResponse.cs but in TypeScript
     */
    export interface PingResponse {
        runtime: number; // The json is serialized as a long so keep it as a long here
        ip: string;
    }

    /** boolean flag to indicate whether the method is allowed to enter the function */
    let updateButtonFlag: boolean = false;

    export async function updateButton(): Promise<void> {
        if (updateButtonFlag) {
            return; // early return while waiting for the next function
        }

        updateButtonFlag = true;

        const status = document.querySelector("#status-text") as HTMLParagraphElement;
        if (await isApiUp()) {
            status.textContent = "200";
            status.style.backgroundColor = "#45fe57";
        } else {
            status.textContent = "500";
            status.style.backgroundColor = "#ff3d3d";
        }

        updateButtonFlag = false;
    }

     /**
     * Checks if the API is up and running by sending a ping request to the server.
     * 
     * @returns {Promise<boolean>} A Promise that resolves to true if the API is up and running, or false otherwise.
     */
    export async function isApiUp(): Promise<boolean> {
        try {
            const response: Response = await fetch('https://localhost:7098/api/statistics/ping');

            if (!response.ok) {
                console.error('error getting data from api');
                return false;
            }

            const json: string = await response.text();

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