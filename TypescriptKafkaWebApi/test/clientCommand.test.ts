import request from "supertest";
import app from "../src/app";

describe("Post /command", () => {
  it("should return 200 OK", () => {
    request(app)
      .post("/command")
      .field("command", JSON.stringify({ Id: "0001", Name: "TypeKafka" }))
      .expect(200);
  });
});