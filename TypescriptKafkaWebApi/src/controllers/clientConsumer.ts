import { Request, Response, NextFunction } from "express";
import { KafkaConsumer } from "./kafka-helper";

/**
 * GET /clientCreated
 * Get clientCreated event form Kafka.
 */
export const getClientCreated = () => {
  const comsumer = new KafkaConsumer(
    process.env.ZOOKEEPERCONNECTIONSTRING,
    process.env.TOPICNAME,
    process.env.CONSUMERGROUP,
    process.env.FROMBEGINNING === "true"
  );

  return (req: Request, res: Response, next: NextFunction) => {
    console.log("handle getClientCreated");
    comsumer.consume().subscribe((value: any) => {
      res.write(value);
    }, (err) => {
      return next(err);
    }, () => {
      res.write("completed");
    });
  };
};
