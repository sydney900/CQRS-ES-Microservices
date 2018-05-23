import kafka from "kafka-node";
import { Observable, Subject, bindCallback } from 'rxjs';
import { map, take } from 'rxjs/operators';

export class KafkaSender {
	constructor(
		public zookeeperConnectionString: string,
		public topicName: string,
		public partitionId: number = 0,
		public compressAttribute: number = 0  // 0: No compression, 1: Compress using GZip, 2: Compress using snappy	
	) { 
		this.ready = false;
	}

	private client: kafka.Client;
	private producer: kafka.Producer;
	private ready: boolean;

	private tryToInitialize(): void {
		if (!this.client) {
			this.client = new kafka.Client(this.zookeeperConnectionString);
			this.producer = new kafka.Producer(this.client, { requireAcks: 1 });			
			this.producer.on('ready', function () {
				this.ready = true;
			});

			this.producer.on('error', function (err) {
				console.log('error', err);
			});	  
		}
	}

	sendCommand(command): void {
		this.tryToInitialize();

		if (this.ready) {
			var commandMessage = new kafka.KeyedMessage('command', JSON.stringify(command));
		
			this.producer.send([
			  { 
				  topic: this.topicName, 
				  partition: this.partitionId, 
				  messages: [commandMessage], 
				  attributes: this.compressAttribute 
			  }
			], function (err, result) {
			  console.log(err || result);
			});  
		}
		else {
			console.log('please wait for connection...');			
		}
	}
}

export class KafkaConsumer {
	constructor(
		public apiUrl: string,
		public topicName: string,
		public consumerGroup: string,
		public fromBeginning: boolean
	) { }

	private kafka: any;
	private target: any;
	private consumerInstance: any;
	private consumerConfig: any;

	private logShutdown(err, observer): void {
		if (err && observer)
			observer.error("Error while shutting down: " + err);
		else
			console.log("Shutdown cleanly.");
	}

	consume(): Observable<string> {

		var Offset = kafka.Offset;
		var Client = kafka.Client;
		var argv = require('optimist').argv;
		var topic = argv.topic || 'topic1';
		
		var client = new kafka.Client('localhost:2181');
		var topics = [{ topic: topic, partition: 1 }, { topic: topic, partition: 0 }];
		var options = { autoCommit: false, fetchMaxWaitMs: 1000, fetchMaxBytes: 1024 * 1024 };
		
		var consumer = new kafka.Consumer(client, topics, options);
		var offset = new Offset(client);
		
		consumer.on('message', function (message) {
		  console.log(message);
		});
		
		consumer.on('error', function (err) {
		  console.log('error', err);
		});
		
		/*
		* If consumer get `offsetOutOfRange` event, fetch data from the smallest(oldest) offset
		*/
		consumer.on('offsetOutOfRange', function (topic) {
		  topic.maxNum = 2;
		  offset.fetch([topic], function (err, offsets) {
			if (err) {
			  return console.error(err);
			}
			var min = Math.min.apply(null, offsets[topic.topic][topic.partition]);
			consumer.setOffset(topic.topic, topic.partition, min);
		  });
		});
		
		
		return new Observable(observer => {
			if (!this.kafka) {
				this.consumerConfig = {
					"format": "binary"
				};
				if (this.fromBeginning) {
					this.consumerConfig['auto.offset.reset'] = 'smallest';
				}

				this.kafka = new KafkaRest({ 'url': this.apiUrl });
				this.kafka.consumer(this.consumerGroup).join(this.consumerConfig, function (err, consumerInstance) {
					if (err) {
						observer.error(err);
						return;
					}

					this.consumerInstance = consumerInstance;

					var stream = consumerInstance.subscribe(this.topicName);
					stream.on('data', function (msgs) {
						for (var i = 0; i < msgs.length; i++) {
							observer.next(msgs[i].value.toString('utf8'));
						}
					}.bind(this));

					stream.on('error', function (err) {
						observer.error(err);
						this.consumerInstance.shutdown(this.logShutdown(err, observer));
					}.bind(this));
					stream.on('end', function () {
					});

					this.consumerInstance.on('end', function () {
						console.log("Consumer instance closed.");
					});
				}.bind(this));
			}
		});
	}

	stopConsumer(observer: any): void {
		if (observer)
			observer.complete();

		if (this.consumerInstance)
			this.consumerInstance.shutdown(this.logShutdown);
	}
}


export class KafkaManager {
	private kafka: any;

	constructor(
		public apiUrl: string
	) {
		this.kafka = new KafkaRest({ 'url': apiUrl });
	};


	listBrokers(): Observable<any> {
		return this.kafka.brokers.list()
			.map(res => res.json())
			.catch((error: any) => Observable.throw(error.json().error || 'Server error'));
	}
	listTopics(): Observable<any> {
		return this.kafka.topics.list()
			.map(res => res.json())
			.catch((error: any) => Observable.throw(error.json().error || 'Server error'));
	}

	getSingleTopic(firstTopicName): Observable<any> {
		if (firstTopicName === null) {
			throw Observable.throw("Parameter firstTopicName is empty.");
		}

		return this.kafka.topic(firstTopicName).get()
			.map(res => res.json())
			.catch((error: any) => Observable.throw(error.json().error || 'Server error'));
	}
	listTopicPartitions(firstTopicName): Observable<any> {
		if (firstTopicName === null) {
			throw Observable.throw("Parameter firstTopicName is empty.");
		}

		return this.kafka.topic(firstTopicName).partitions.list()
			.map(res => res.json())
			.catch((error: any) => Observable.throw(error.json().error || 'Server error'));
	}

	getSingleTopicPartition(firstTopicName, firstTopicPartitionId) {
		if (firstTopicName === null || firstTopicPartitionId === null) {
			throw Observable.throw("Parameter firstTopicName or firstTopicPartitionId is empty.");
		}

		return this.kafka.topic(firstTopicName).partition(firstTopicPartitionId).get()
			.map(res => res.json())
			.catch((error: any) => Observable.throw(error.json().error || 'Server error'));
	}

}
