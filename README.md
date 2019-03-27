# CQRS-ES-Kafka-Microservices-Docker

[![Build Status](https://dev.azure.com/wangbing1688/CQRS-ES-Microservices/_apis/build/status/sydney900.CQRS-ES-Microservices?branchName=master)](https://dev.azure.com/wangbing1688/CQRS-ES-Microservices/_build/latest?definitionId=2?branchName=master)

Open solution with Visual Studio 2017 and run in docker mode

## How to run
Open Windows Powershell then run command
```
.\RunDockerCompose.ps1
```
and then run command to get IP
```
.\RunDockerCompose.ps1
```
and then open URL: http://10.0.75.1:64003 in your browser

## Version 1
![alt text](https://github.com/sydney900/CQRS-RS-Microservices/blob/master/Version1.png "Version 1 Chart")

## Version 2
![alt text](https://github.com/sydney900/CQRS-RS-Microservices/blob/master/Version2.png "Version 2 Chart")

## How to run the microservices on Kubernetes cluster
[See document how to run the microservices on Kubernetes cluster](https://github.com/sydney900/CQRS-RS-Microservices/blob/master/Charts/Readme.md)


## How to use minikube
### install Minikube and Kubectl
1. Minikube download: https://github.com/kubernetes/minikube/releases

2. Kubectl versions are available at a generic location as per the following format: https://storage.googleapis.com/kubernetes-release/release/${K8S_VERSION}/bin/${GOOS}/${GOARCH}/${K8S_BINARY}

3. how to get latest stabel: 
https://storage.googleapis.com/kubernetes-release/release/stable.txt

4. windows version download: https://storage.googleapis.com/kubernetes-release/release/v1.10.3/bin/windows/amd64/kubectl.exe

5. windows version Minikube must be installed in system disk. 

### use Minikube
```
./minikube.exe start
eval $(./minikube docker-env)
docker-compose build
/kubectl create -f YAML
.\minikube.exe dashboard
```

## How to use spotify/kafka
run : 
```
   docker run -p 2181:2181 -p 9092:9092 --env ADVERTISED_HOST=`docker-machine ip \`docker-machine active\`` --env ADVERTISED_PORT=9092 --env TOPICS=Clients-Topic,Products-Topic --env GROUP_ID=clients-consumer-group spotify/kafka
```

list topics:  (8f5588924e89 is container id)
```
   docker exec 8f5588924e89 /opt/kafka_2.11-0.10.1.0/bin/kafka-topics.sh --list --zookeeper localhost:2181
```

Create a topic "Clients-Topic":
```
   docker exec 8f5588924e89 /opt/kafka_2.11-0.10.1.0/bin/kafka-topics.sh --create --zookeeper localhost:2181 --replication-factor 1 --partitions 1 --topic Clients-Topic
```

Start a producer:
```
   docker run -it --rm --link 8f5588924e89 spotify/kafka /opt/kafka_2.11-0.10.1.0/bin/kafka-console-producer.sh --broker-list 8f5588924e89:9092 --topic Clients-Topic
```

Start a consumer:
```
docker run -it --rm --link 8f5588924e89 spotify/kafka /opt/kafka_2.11-0.10.1.0/bin/kafka-console-consumer.sh --bootstrap-server 8f5588924e89:9092 --topic Clients-Topic --from-beginning
```
Run Kafka Rest Proxy:
```
docker run -d --net=host --name=kafka-rest -e KAFKA_REST_ZOOKEEPER_CONNECT=192.168.99.100:2181 -e KAFKA_REST_LISTENERS=http://192.168.99.100:8082 -e KAFKA_REST_HOST_NAME=192.168.99.100 confluentinc/cp-kafka-rest:4.1.0
```
Run bash
```
docker run --rm -v /var/run/docker.sock:/var/run/docker.sock -e HOST_IP=$1 -e ZK=$2 -i -t spotify/kafka /bin/bash
```

## wurstmeister/kafka command
```
docker exec 8f5588924e89 /bin/kafka-topics.sh --list --zookeeper localhost:2181
docker exec 8f5588924e89 /bin/kafka-topics.sh --create --zookeeper localhost:2181 --replication-factor 1 --partitions 1 --topic test
```

## Credits
* https://github.com/gregoryyoung/m-r
* https://github.com/ThreeMammals/Ocelot
