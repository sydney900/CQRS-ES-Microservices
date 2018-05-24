///<reference path="../../../node_modules/@types/node/index.d.ts"/>
import KafkaRest from "kafka-rest";
import { Observable, Subject, bindCallback } from 'rxjs';
import { map, take } from 'rxjs/operators';

export class KafkaSender {
	constructor(
		public apiUrl: string,
		public topicName: string,
		public partitionId: number = null
	) { }

	private kafka: any;
	private target: any;

	private tryToInitialize(): void {
		if (!this.kafka) {
			this.kafka = new KafkaRest({ 'url': this.apiUrl });
			this.target = this.kafka.topic(this.topicName);
			if (this.partitionId)
				this.target = this.target.partition(this.partitionId);
		}
	}

	sendCommand(command): Observable<any> {
		this.tryToInitialize();

		return new Observable(observer => {
			if (this.target) {
				return this.target.produce(JSON.stringify(command), function(err,res){
					if (err) {
						observer.error(err);
					}
					else {
						observer.next(res);
						observer.complete();
					}
				});
			}
			else
			{
				observer.error("Not such a topic in  ")
			}
		});
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
