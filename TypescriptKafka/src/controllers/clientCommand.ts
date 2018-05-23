import { Request, Response, NextFunction } from "express";
const request = require("express-validator");
import { KafkaConsumer, KafkaSender } from './kafka-helper';

let sender = new KafkaSender(
  process.env.ZOOKEEPERCONNECTIONSTRING,
  process.env.TOPICNAME,
  parseInt(process.env.PARTITIONID, 0),
  parseInt(process.env.COMPRESSIONATTRIBUTES, 0)
);

let comsumer = new KafkaConsumer(
  process.env.ZOOKEEPERCONNECTIONSTRING,
  process.env.TOPICNAME,
  process.env.CONSUMERGROUP,
  process.env.FROMBEGINNING==='true'
);

/**
 * POST /command
 * Send command to Kafka.
 */
export let sendCommand = (req: Request, res: Response, next: NextFunction) => {
  req.assert("command", "Command must ne not empty.").isEmpty();

  const errors = req.validationErrors();

  if (errors) {
    req.flash("errors", JSON.stringify(errors));
  }

  sender.sendCommand(req.body.command);
  
  req.flash("success", "Command has been sent successfully!");
};

export let getClientCreated = (req: Request, res: Response, next: NextFunction) => {

  comsumer.consume();
};
