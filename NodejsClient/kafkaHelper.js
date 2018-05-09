"use strict";

var KafkaRest = require('kafka-rest');


function CommandSender(apiUrl, topicName, partitionId) {
    this.apiUrl = apiUrl;
    this.topicName = topicName;
    this.partitionId = partitionId;
}
CommandSender.prototype.tryToInitialize = function () {
    if (!this.kafka) {
        this.kafka = new KafkaRest({ 'url':  this.apiUrl});
        this.target = this.kafka.topic(this.topicName);
        if (this.partitionId)
            this.target = this.target.partition(this.partitionId);
    }
}
CommandSender.prototype.sendCommand = function(command, responseHandler)
{
    this.tryToInitialize();

    if (this.target)
    {
        this.target.produce(JSON.stringify(command), responseHandler)
    }
}


function KafkaConsumer(apiUrl, topicName, errorHandler, msgHandler, consumerGroup, fromBeginning) {
    this.apiUrl = apiUrl;
    this.topicName = topicName;
    this.errorHandler = errorHandler;
    this.msgHandler = msgHandler;
    this.consumerGroup = consumerGroup;
    this.fromBeginning = fromBeginning;

    this.consumed = 0;
    this.consumerInstance = undefined;

    this.logShutdown = function (err) {
        if (err && this.errorHandler)
            this.errorHandler("Error while shutting down: " + err);
        else
            console.log("Shutdown cleanly.");
    }

}
KafkaConsumer.prototype.consume = function () {
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
                this.errorHandler(err);
                return;
            }

            this.consumerInstance = consumerInstance;

            var stream = consumerInstance.subscribe(this.topicName);
            stream.on('data', function (msgs) {
                for (var i = 0; i < msgs.length; i++) {
                    this.msgHandler(msgs[i].value.toString('utf8'));
                }
            }.bind(this));

            stream.on('error', function (err) {
                this.errorHandler(err);
                this.consumerInstance.shutdown(this.logShutdown);
            }.bind(this));
            stream.on('end', function () {
            });

            this.consumerInstance.on('end', function () {
                console.log("Consumer instance closed.");
            });
        }.bind(this));
    }
}
KafkaConsumer.prototype.stopConsumer = function () {
    if(this.consumerInstance)
        this.consumerInstance.shutdown(this.logShutdown);
}



function KafkaManager(apiUrl) {
    this.apiUrl = apiUrl;
    this.kafka = new KafkaRest({ 'url':  apiUrl});
}
KafkaManager.prototype.listBrokers = function (listBrokersHandler) {
    this.kafka.brokers.list(listBrokersHandler);
}
KafkaManager.prototype.listTopics = function (listTopicsHandler) {
    this.kafka.topics.list(listTopicsHandler);
}
KafkaManager.prototype.getSingleTopic = function (firstTopicName, getSingleTopicHandler) {
    if (firstTopicName === null) {
        console.log("Parameter firstTopicName is empty.");
        return;
    }
	    
    this.kafka.topic(firstTopicName).get(getSingleTopicHandler);
}
KafkaManager.prototype.listTopicPartitions = function (firstTopicName, listTopicPartitionsHandler) {
    if (firstTopicName === null) {
        console.log("Parameter firstTopicName is empty.");
        return;
    }
    
    this.kafka.topic(firstTopicName).partitions.list(listTopicPartitionsHandler);
}
KafkaManager.prototype.getSingleTopicPartition = function (firstTopicName, firstTopicPartitionId, getSingleTopicPartitionHandler) {
    if (firstTopicName === null || firstTopicPartitionId === null) {
        console.log("Parameter firstTopicName or firstTopicPartitionId is empty.");
        return;
    }
    
    this.kafka.topic(firstTopicName).partition(firstTopicPartitionId).get(getSingleTopicPartitionHandler);
}


module.exports = {
    KafkaManager: KafkaManager,
    CommandSender: CommandSender,
    KafkaConsumer: KafkaConsumer
};
