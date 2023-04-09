export class PingResponse {
  runtime: number;
  ip: string;

  constructor(duration: number, address: string) {
    this.runtime = duration;
    this.ip = address;
  }

  static fromJson(json: string): PingResponse {
    const object = JSON.parse(json);

    return new PingResponse(object.runtime, object.ip);
  }
}
