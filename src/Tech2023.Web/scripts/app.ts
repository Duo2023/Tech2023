import { PingResponse } from "./api_check";

export namespace funcs {
  export async function check_api(): Promise<void> {
    const res = new PingResponse(5, "192.168.1.1");
    console.log(res);
  }
}
