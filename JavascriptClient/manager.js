"use strict";

var apiUrl = 'http://192.168.99.100:8082';

var KafkaRest = require('kafka-rest');
var kafka = new KafkaRest({ 'url':  apiUrl});

function listBrokers() {
    console.log("Listing brokers:");
    kafka.brokers.list(function (err, brokers) {
        if (err) {
            console.log("Failed trying to list brokers: " + err);
        } else {
            for (var i = 0; i < brokers.length; i++)
                console.log(brokers[i].toString() + " (raw: " + JSON.stringify(brokers[i].raw) + ")");
        }
    });
}

var firstTopicName = null;
function listTopics() {
    console.log("Listing topics:");
    kafka.topics.list(function (err, topics) {
        if (err) {
            console("Failed to list topics: " + err);
        } else {
           if (topics.length > 0)
                firstTopicName = topics[0].name;
             for (var i = 0; i < topics.length; i++)
                console.log(topics[i].toString() + " (raw: " + JSON.stringify(topics[i].raw) + ")");
        }
    });
}

function getSingleTopic() {
    if (firstTopicName == null) {
        console.log("Didn't find any topics, skipping getting a single topic.");
		return;
    }
	
    console.log("Getting single topic " + firstTopicName + ":");
    kafka.topic(firstTopicName).get(function(err, topic) {
        if (err)
            console.log("Failed to get topic " + firstTopicName + ": " + err);
        else
            console.log(topic.toString() + " (raw: " + JSON.stringify(topic.raw) + ")");
    });
}

var firstTopicPartitionId = null;
function listTopicPartitions() {
    if (firstTopicName == null) {
        console.log("Didn't find any topics, skipping listing partitions.");
        console.log();
        return;
    }
    console.log("Listing partitions for topic " + firstTopicName + ":");
    kafka.topic(firstTopicName).partitions.list(function (err, data) {
        if (err) {
            console("Failed to list partitions: " + err);
        } else {
            for (var i = 0; i < data.length; i++)
                console.log(data[i].toString() + " (raw: " + JSON.stringify(data[i].raw) + ")");
            if (data.length > 0)
                firstTopicPartitionId = data[0].id;
        }
    });
}

function getSingleTopicPartition() {
    if (firstTopicName == null || firstTopicPartitionId == null) {
        console.log("Didn't find any topics or partitions, skipping getting a single partition.");
		return;
    }
    console.log("Getting single partition " + firstTopicName + ":" + firstTopicPartitionId + ":");
    kafka.topic(firstTopicName).partition(firstTopicPartitionId).get(function(err, partition) {
        if (err)
            console.log("Failed to get partition \"" + firstTopicName + "\":" + firstTopicPartitionId + ": " + err);
        else
            console.log(partition.toString() + " (raw: " + JSON.stringify(partition.raw) + ")");
    });
}

listBrokers();
listTopics();
getSingleTopic();
listTopicPartitions();
getSingleTopicPartition();