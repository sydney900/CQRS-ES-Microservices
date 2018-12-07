import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IResource } from '../models/iResource';
import { LogService } from './log.service';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Injectable()
export class DataService<T extends IResource> {
  private nameOfT: string;
  private headers: HttpHeaders = new HttpHeaders();

  constructor(private url: string, private httpClient: HttpClient, private log: LogService, private oidcSecurityService: OidcSecurityService, t: (new () => T)) {
    this.nameOfT = t.name;
  }

  private setHeaders() {
    this.headers = new HttpHeaders();
    this.headers = this.headers.set('Content-Type', 'application/json');
    this.headers = this.headers.set('Accept', 'application/json');

    const token = this.oidcSecurityService.getToken();
    if (token !== '') {
      const tokenValue = 'Bearer ' + token;
      this.headers = this.headers.append('Authorization', tokenValue);
    }
  }

  public create(item: T): Observable<T> {
    this.log.info(JSON.stringify(item), 'Post');

    this.setHeaders();
    return this.httpClient.post<T>(this.url, item, { headers: this.headers });
  }

  public update(item: T): Observable<T> {
    this.log.info(JSON.stringify(item), 'Put');

    this.setHeaders();
    return this.httpClient.put<T>(`${this.url}/${item.id}`, item, { headers: this.headers });
  }

  public get(id: number): Observable<T> {
    this.log.info(`Get object for id ${id} of ${this.nameOfT}`);

    this.setHeaders();
    return this.httpClient.get<T>(`${this.url}/${id}`, { headers: this.headers });
  }

  public query(queryString: string): Observable<T> {
    this.log.info(`Get objects of ${this.nameOfT} where query string is ${queryString}`);

    this.setHeaders();
    return this.httpClient.get<T>(`${this.url}/?${queryString}`, { headers: this.headers });
  }

  public getAll(): Observable<T> {
    this.log.info(`Get all of ${this.nameOfT}`);

    this.setHeaders();
    return this.httpClient.get<T>(`${this.url}`, { headers: this.headers });
  }

  delete(id: number) {
    this.log.info(`Delete object of ${this.nameOfT} where id is id ${id}`);

    this.setHeaders();
    return this.httpClient.delete(`${this.url}/${id}`, { headers: this.headers });
  }
}
