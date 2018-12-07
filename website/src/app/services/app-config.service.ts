import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Config } from '../models/config';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {

  private configUrl: string;
  private appConfig: Config;
  private wellKnownEndpoints: any;


  constructor(private http: HttpClient) {
    this.configUrl = '/assets/data/appConfig.json';
  }

  async getConfig() {
    if (!this.appConfig && process.env.KAFKACLIENTCREATEDURL && process.env.KAFKASENDCOMMANDURL) {
      this.appConfig.kafkaClientCreatedUrl = process.env.KAFKACLIENTCREATEDURL;
      this.appConfig.kafkaSendCommandUrl = process.env.KAFKASENDCOMMANDURL;
    }

    if (!this.appConfig) {
      this.appConfig = await this.http.get<Config>(this.configUrl).toPromise();
        
      // load SSo config 
      if (!this.wellKnownEndpoints) {
        this.wellKnownEndpoints = await this.http.get<any>(`${this.appConfig.ssoUrl}/.well-known/openid-configuration`).toPromise();
      }

      //then setup SSO
    }

    return this.appConfig;
  }
}
