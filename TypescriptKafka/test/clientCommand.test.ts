import request from "supertest";
import app from "../src/app";

describe("GET /command", () => {
  it("should return 200 OK", () => {
    return request(app).get("/command")
      .expect(200);
  });
});
