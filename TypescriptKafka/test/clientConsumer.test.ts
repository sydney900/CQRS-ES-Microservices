import request from "superagent";
import app from "../src/app";

describe("GET /clientCreated", () => {
  it("should return 200 OK", () => {
    request
      .get("/clientCreated")
      .then(function (res) {
        expect(res.status).toBe(200);
      });
  });

  it("CORS should work", () => {
    request
      .get("/clientCreated")
      .withCredentials()
      .then(function (res) {
        expect(res.status).toBe(200);
      });
  });
});