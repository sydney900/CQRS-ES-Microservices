import { Injectable } from '@angular/core';

import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { Client } from '../models/client';


@Injectable({
  providedIn: 'root'
})
export class ClientService {

  constructor(private configService: AppConfigService) { 
     configService.getConfig().then(config => {
     this.sender = new KafkaHelper.CommandSender(config.apiUrl, config.topicName);
	 this.consumer = new KafkaHelper.KafkaConsumer(config.apiUrl, config.topicName, config.consumerGroup, config.fromBeginning);
	 });
  }

  private sender: KafkaSender;
  private consumer: KafkaConsumer;
  
  getClients(): Observable<Client[]> {
    return this.consumer.subscribe();
  }
  
  sendCommand(command): Observable {
	  this.sender.sendCommand(command);
  }
}
