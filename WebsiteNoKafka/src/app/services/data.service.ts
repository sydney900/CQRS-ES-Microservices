import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IResource } from '../models/iResource';
import { LogService } from './log.service';

@Injectable()
export class DataService<T extends IResource> {
  private nameOfT: string;
  constructor(private url: string, private httpClient: HttpClient, private log: LogService, t: (new () => T)) {
    this.nameOfT = t.name;
  }

  public create(item: T): Observable<T> {
    this.log.info(JSON.stringify(item), 'Post');
    return this.httpClient.post<T>(this.url, item);
  }

  public update(item: T): Observable<T> {
    this.log.info(JSON.stringify(item), 'Put');
    return this.httpClient.put<T>(`${this.url}/${item.id}`, item);
  }

  public get(id: number): Observable<T> {
    this.log.info(`Get object for id ${id} of ${this.nameOfT}`);
    return this.httpClient.get<T>(`${this.url}/${id}`);
  }

  public query(queryString: string): Observable<T> {
    this.log.info(`Get objects of ${this.nameOfT} where query string is ${queryString}`);
    return this.httpClient.get<T>(`${this.url}/?${queryString}`);
  }

  public getAll(): Observable<T> {
    this.log.info(`Get all of ${this.nameOfT}`);
    return this.httpClient.get<T>(`${this.url}`);
  }

  delete(id: number) {
    this.log.info(`Delete object of ${this.nameOfT} where id is id ${id}`);
    return this.httpClient.delete(`${this.url}/${id}`);
  }
}
