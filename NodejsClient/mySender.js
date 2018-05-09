'use strict';

var KafkaHelper = require("./kafkaHelper");

var apiUrl = 'http://192.168.99.100:8082';
var topicName = 'Clients-Topic';

var producer = new KafkaHelper.CommandSender(apiUrl, topicName);

var command = {
     Id: "001",
     Name: "Safari, Cortana"
};

producer.sendCommand(command, function(err,res){});
