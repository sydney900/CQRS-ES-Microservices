# CQRS-ES-Kafka-Microservices-Docker

Open solution with Visual Studio 2017 and run in docker mode

## How to use spotify/kafka
run : 
```
   docker run -p 2181:2181 -p 9092:9092 --env ADVERTISED_HOST=`docker-machine ip \`docker-machine active\`` --env ADVERTISED_PORT=9092 --env TOPICS=Clients-Topic,Products-Topic --env GROUP_ID=clients-consumer-group spotify/kafka
```

list topics:  
```
   docker exec 8f5588924e89 /opt/kafka_2.11-0.10.1.0/bin/kafka-topics.sh --list --zookeeper localhost:2181
```

Create a topic "Clients-Topic":
```
   docker exec 8f5588924e89 /opt/kafka_2.11-0.10.1.0/bin/kafka-topics.sh --create --zookeeper localhost:2181 --replication-factor 1 --partitions 1 --topic Clients-Topic
```

Start a producer
```
   docker run -it --rm --link 8f5588924e89 spotify/kafka /opt/kafka_2.11-0.10.1.0/bin/kafka-console-producer.sh --broker-list 8f5588924e89:9092 --topic Clients-Topic
```

Start a consumer
```
docker run -it --rm --link 8f5588924e89 spotify/kafka /opt/kafka_2.11-0.10.1.0/bin/kafka-console-consumer.sh --bootstrap-server 8f5588924e89:9092 --topic Clients-Topic --from-beginning
```


auto.create.topics.enable
docker run --rm -v /var/run/docker.sock:/var/run/docker.sock -e HOST_IP=$1 -e ZK=$2 -i -t spotify/kafka /bin/bash
docker exec 8f5588924e89 /bin/kafka-topics.sh --list --zookeeper localhost:2181
docker exec 8f5588924e89 /bin/kafka-topics.sh --create --zookeeper localhost:2181 --replication-factor 1 --partitions 1 --topic test

## Credits
* https://github.com/ThreeMammals/Ocelot