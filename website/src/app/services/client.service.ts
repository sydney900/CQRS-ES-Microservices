import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map, take, catchError } from 'rxjs/operators';

import { Client } from '../models/client';
import { KafkaSender, KafkaConsumer } from './kafka-helper';
import { AppConfigService } from './app-config.service';


@Injectable({
  providedIn: 'root'
})
export class ClientService {

  constructor(private configService: AppConfigService) {
    configService.getConfig().then(config => {
      this.sender = new KafkaSender(config.apiUrl, config.topicName);
      this.consumer = new KafkaConsumer(config.apiUrl, config.topicName, config.consumerGroup, config.fromBeginning);
    });
  }

  private sender: KafkaSender;
  private consumer: KafkaConsumer;

  getClients(): Observable<Client> {
    return this.consumer.consume().pipe(map(c => JSON.parse(c)));
  }

  sendCommand(command): Observable<any> {
    return this.sender.sendCommand(command);
  }
}
