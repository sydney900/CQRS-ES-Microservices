import express from "express";
import compression from "compression";  // compresses requests
import session from "express-session";
import bodyParser from "body-parser";
import lusca from "lusca";
import dotenv from "dotenv";
import flash from "express-flash";
import path from "path";
import expressValidator from "express-validator";

// Controllers (route handlers)
import * as clientCommandController from "./controllers/ClientCommand";

// Load environment variables from .env file
dotenv.config({ path: ".env" });


// Create Express server
const app = express();

// Express configuration
app.set("port", process.env.PORT || 3000);
app.use(compression());
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));
app.use(expressValidator());
app.use(flash());
app.use(lusca.xframe("SAMEORIGIN"));
app.use(lusca.xssProtection(true));

/**
 * Primary app routes.
 */
app.get("/clientCreated", clientCommandController.getClientCreated);
app.post("/command", clientCommandController.sendCommand);


export default app;