import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, take, catchError } from 'rxjs/operators';

import { Client } from '../models/client';
import { AppConfigService } from './app-config.service';
import { Config } from 'protractor';


@Injectable({
  providedIn: 'root'
})
export class ClientService {
  private appConfig: Config;

  constructor(private configService: AppConfigService, private http: HttpClient) {
    configService.getConfig().then(config => {
      this.appConfig = config;
    });
  }

  getClientCreated(): Observable<Client> {
    if (!this.appConfig) {
      return this.http.get<Client>(this.appConfig.kafkaClientCreatedUrl);
    }

    return Observable.throw('please wait until get configuration.');
  }

  sendCommand(command): Observable<any> {
    if (!this.appConfig) {
      return this.http.post(this.appConfig.kafkaSendCommandUrl, command);
    }

    return Observable.throw('please wait until get configuration.');
  }
}
