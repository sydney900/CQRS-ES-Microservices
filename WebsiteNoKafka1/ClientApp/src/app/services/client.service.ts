import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { DataService } from './data.service';
import { HttpClient } from '@angular/common/http';
import { LogService } from './log.service';
import { Client } from '../models/client';
import { AppConfigService } from '../app-config/app-config.service';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Injectable({
  providedIn: 'root'
})
export class ClientService extends DataService<Client> {
  constructor(http: HttpClient, log: LogService, configService: AppConfigService, oidcSecurityService: OidcSecurityService) {
    super(configService.appConfig.apiGatewayhUrl + '/clients', http, log, oidcSecurityService, Client);
  }

}
