'use strict';

var KafkaHelper = require("./kafkaHelper");

var apiUrl = 'http://192.168.99.100:8082';
var topicName = 'Clients-Topic';

var manager = new KafkaHelper.KafkaManager(apiUrl);

manager.listBrokers(function (err, brokers) {
        console.log("Listing brokers:");

        if (err) {
            console.log("Failed trying to list brokers: " + err);
        } else {
            for (var i = 0; i < brokers.length; i++)
                console.log(brokers[i].toString() + " (raw: " + JSON.stringify(brokers[i].raw) + ")");
        }

});

var firstTopicName = null;
manager.listTopics(function (err, topics) {
        console.log("Listing topics:");

        if (err) {
            console("Failed to list topics: " + err);
        } else {
            if (topics.length > 0)
                firstTopicName = topics[0].name;
            for (var i = 0; i < topics.length; i++)
                console.log(topics[i].toString() + " (raw: " + JSON.stringify(topics[i].raw) + ")");
        }
    });


manager.getSingleTopic(firstTopicName , function(err, topic) {
        console.log("Getting single topic " + firstTopicName + ":");

        if (err)
            console.log("Failed to get topic " + firstTopicName + ": " + err);
        else
            console.log(topic.toString() + " (raw: " + JSON.stringify(topic.raw) + ")");
    });

var firstTopicPartitionId = null;
manager.listTopicPartitions(firstTopicName, function (err, data) {
        console.log("Listing partitions for topic " + firstTopicName + ":");

        if (err) {
            console("Failed to list partitions: " + err);
        } else {
            for (var i = 0; i < data.length; i++)
                console.log(data[i].toString() + " (raw: " + JSON.stringify(data[i].raw) + ")");
            if (data.length > 0)
                firstTopicPartitionId = data[0].id;
        }
    });


manager.getSingleTopicPartition(firstTopicName, firstTopicPartitionId, function(err, partition) {
        console.log("Getting single partition " + firstTopicName + ":" + firstTopicPartitionId + ":");

        if (err)
            console.log("Failed to get partition \"" + firstTopicName + "\":" + firstTopicPartitionId + ": " + err);
        else
            console.log(partition.toString() + " (raw: " + JSON.stringify(partition.raw) + ")");
    });





