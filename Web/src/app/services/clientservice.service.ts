import { Injectable } from '@angular/core';

import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { Client } from '../models/client';


@Injectable({
  providedIn: 'root'
})
export class ClientserviceService {

  constructor() { }

  getClients(): Observable<Client[]> {

    // ...using get request
    return this.http.get(this.commentsUrl)
      // ...and calling .json() on the response to return data
      .map((res: Response) => res.json())
      //...errors if any
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));

  }
}
