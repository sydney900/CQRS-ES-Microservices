'use strict';

var KafkaHelper = require("./kafkaHelper");

var apiUrl = 'http://192.168.99.100:8082';
var topicName = 'Clients-Topic';

var comsumer = new KafkaHelper.KafkaConsumer(
  apiUrl, 
  topicName, 
  function(err) {
     console.log(err);
  },
  function(msg) {
     console.log(msg);
  },"clients-consumer-group",true
);

comsumer.consume();
