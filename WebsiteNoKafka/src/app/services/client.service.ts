import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { LogService } from './log.service';
import { Client } from '../models/client';
import { AppConfigService } from './app-config.service';

@Injectable({
  providedIn: 'root'
})
export class ClientService extends DataService<Client> {
  constructor(http: HttpClient, log: LogService, configService: AppConfigService) {
    super(configService.appConfig.apiGatewayhUrl, http, log, Client);
  }

}
