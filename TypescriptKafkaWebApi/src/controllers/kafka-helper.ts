import { Client, Producer, Consumer, Offset } from "kafka-node";
import { Observable, Subject, bindCallback, from } from "rxjs";
import { map, take } from "rxjs/operators";

export class KafkaSender {
  constructor(
    public zookeeperConnectionString: string,
    public topicName: string,
    public partitionId: number = 0,
    public compressionAttributes: number = 0 // 0: No compression, 1: Compress using GZip, 2: Compress using snappy
  ) {
    this.ready = false;
  }

  private client: Client;
  private producer: Producer;
  private ready: boolean;

  private tryToInitialize(): void {
    if (this.client) {
      this.client = new Client(this.zookeeperConnectionString);
      this.producer = new Producer(this.client, { requireAcks: 1 });

      this.producer.on("ready", function () {
        this.ready = true;
      });

      this.producer.on("error", function (err: any) {
        console.log("error", err);
      });
    }
  }

  sendCommand(...params: any[]): Observable<any> {
    if (!this.ready) {
      return Observable.throw("Please wait for connection...");
    }

    if (!params || params.length === 0) {
      return Observable.throw("No commands need to be sent.");
    }

    const subject = new Subject();
    this.tryToInitialize();

    const payload = [];
    for (let i = 0; i < params.length; i++) {
      payload.push({
        topic: this.topicName,
        partition: this.partitionId,
        messages: [JSON.stringify(params[i])],
        attributes: this.compressionAttributes
      });
    }

    this.producer.send(payload, function (err: any, result: any) {
      if (err) {
        subject.error(err);
      }
      else {
        subject.next(result);
        subject.complete();
      }
      console.log(err || result);
    });

    return from(subject);
  }
}

export class KafkaConsumer {
  constructor(
    public zookeeperConnectionString: string,
    public topicName: string,
    public consumerGroup: string,
    public fromBeginning: boolean
  ) { }

  private client: Client;
  private consumer: Consumer;
  private clientOffset: Offset;
  private consumerConfig: any;

  consume(): Observable<any> {
    const subject = new Subject();

    this.client = new Client(this.zookeeperConnectionString);
    // const topics = [{ topic: this.topicName, partition: 1 }, { topic: this.topicName, partition: 0 }];
    const topics = [{ topic: this.topicName, partition: 0 }];
    const options = { autoCommit: false, fetchMaxWaitMs: 1000, fetchMaxBytes: 1024 * 1024 };

    this.consumer = new Consumer(this.client, topics, options);
    this.clientOffset = new Offset(this.client);

    this.consumer.on("message", function (message: any) {
      // console.log(message);
      subject.next(message);
    });

    this.consumer.on("error", function (err: any) {
      console.log("error", err);
      subject.error(err);
    }.bind(this));

    /*
    * If consumer get `offsetOutOfRange` event, fetch data from the smallest(oldest) offset
    */
    this.consumer.on("offsetOutOfRange", function (topic: any) {
      topic.maxNum = 2;
      this.clientOffset.fetch([topic], function (err: any, offsets: any) {
        if (err) {
          subject.error(err);
          return console.error(err);
        }
        const min = Math.min.apply(undefined, offsets[topic.topic][topic.partition]);
        this.consumer.setOffset(topic.topic, topic.partition, min);
      }.bind(this));
    }.bind(this));

    return from(subject);

  }

  stopConsumer(observer: any): void {
    if (observer)
      observer.complete();
  }
}