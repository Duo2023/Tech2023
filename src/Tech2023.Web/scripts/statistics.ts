export namespace statistics {
    // Keep the same layout as Web.Shared/Statistics/PingResponse.cs
    export interface PingResponse {
        runtime: number; // The json is serialized as a long so keep it as a long here
        ip: string;
    }

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