import { Injectable } from '@angular/core';
import { ILog } from '../models/ilog';

@Injectable({
  providedIn: 'root'
})
export class LocalLogService implements ILog<void> {
  log(message: string): void {
    console.log(message);
  }
  info(message: string): void {
    console.info(message);
  }
  warn(message: string): void {
    console.warn(message);
  }
  error(message: string): void {
    console.error(message);
  }
  debug(message: string): void {
    console.debug(message);
  }

  constructor() { }
}
