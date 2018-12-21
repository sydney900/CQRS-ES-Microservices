import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Config } from '../models/config';
import { config } from 'rxjs';
import { AuthModule, OidcSecurityService, OpenIDImplicitFlowConfiguration, AuthWellKnownEndpoints } from 'angular-auth-oidc-client';
import * as process from 'process';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {

  private configUrl: string;
  appConfig: Config;
  wellKnownEndpoints: any;

  constructor(private http: HttpClient) {
    this.configUrl = '/assets/data/appConfig.json';
  }

  async getConfig() {
    if (!this.appConfig && process.env.LOG_URL && process.env.APIGATEWAY_URL && process.env.AUTH_URL && process.env.SCOPE) {
      this.appConfig = {
        apiGatewayhUrl: process.env.APIGATEWAY_URL,
        authUrl: process.env.AUTH_URL,
        logUrl: process.env.LOG_URL,
        clientId: process.env.CLIENTID,
        scope: process.env.SCOPE
      }
    }


    if (!this.appConfig) {
      this.appConfig = await this.http.get<Config>(this.configUrl).toPromise();
    }

    if (!this.wellKnownEndpoints) {
      this.wellKnownEndpoints = await this.http.get<any>(`${this.appConfig.authUrl}/.well-known/openid-configuration`).toPromise();
    }

    return this.appConfig;
  }
}

