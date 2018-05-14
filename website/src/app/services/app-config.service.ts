import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {

  private configUrl;
  private appConfig;
  

  constructor(private http: HttpClient) { 
    this.configUrl='/assets/data/appConfig.json';
  }

  async getConfig(): Config {
	  if (!this.appConfig) {
		  var res = await this.http.get<Config>(this.configUrl).toPromise();
		  return res.tojson();
	  }
    return this.appConfig;
  }
}
