import { Request, Response, NextFunction } from "express";
const request = require("express-validator");
import { KafkaSender } from "./kafka-helper";

/**
 * POST /command
 * Send command to Kafka.
 */
export const sendCommand = () => {
  const sender = new KafkaSender(
    process.env.ZOOKEEPERCONNECTIONSTRING,
    process.env.TOPICNAME,
    parseInt(process.env.PARTITIONID, 0),
    parseInt(process.env.COMPRESSIONATTRIBUTES, 0)
  );

  return (req: Request, res: Response, next: NextFunction) => {
    req.assert("command", "Command must ne not empty.").isEmpty();

    const errors = req.validationErrors();

    if (errors) {
      req.flash("errors", JSON.stringify(errors));
    }

    sender.sendCommand(req.body.command);

    req.flash("success", "Command has been sent successfully!");
  };
};
