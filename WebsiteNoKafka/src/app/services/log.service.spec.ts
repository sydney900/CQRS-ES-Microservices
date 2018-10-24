import { TestBed, inject, getTestBed } from '@angular/core/testing';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { LogService } from './log.service';
import { LocalLogService } from './local-log.service';
import { environment } from '../../environments/environment';

describe('LogService', () => {
  let injector: TestBed;
  let service: LogService;
  let httpMock: HttpTestingController;
  let localLogServiceSpy;

  beforeEach(() => {
    localLogServiceSpy = jasmine.createSpyObj('LocalLogService', ['log', 'info', 'debug', 'error', 'warn']);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        LogService,
        { provide: LocalLogService, useValue: localLogServiceSpy }
      ]
    });

    injector = getTestBed();
    service = injector.get(LogService);
    httpMock = injector.get(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('Create service should raise error if no envirement varibale LOG_URL', () => {
    const saveValue = environment.LOGURL;

    environment.LOGURL = undefined;
    expect(() => new LogService(new HttpClient(null), console)).toThrow(new Error('The envirement varibale LOG_URL should be provided'));

    environment.LOGURL = saveValue;
  });

  it('log function should call the log function of LocalLogService', () => {
    const msg = 'test';
    service.log(msg);

    expect(localLogServiceSpy.log).toHaveBeenCalledWith(msg);
  });

  it('log function should post the correct log item', () => {
    const msg = 'test';
    service.log(msg).subscribe(
      () => {},
      error => {
        fail(error);
      }
    );

    const req = httpMock.expectOne(service.LOG_URL);
    expect(req.request.url).toBe(`${service.LOG_URL}`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(jasmine.objectContaining({content: msg, type: 'log'}));
    req.flush({content: `$msg`, type: 'log'});
  });

  it('info function should call the info function of LocalLogService', () => {
    const msg = 'test info';
    service.info(msg);

    expect(localLogServiceSpy.info).toHaveBeenCalledWith(msg);
  });

  it('info function should post the correct log item', () => {
    const msg = 'test info';
    service.info(msg).subscribe(
      () => {},
      error => {
        fail(error);
      }
    );

    const req = httpMock.expectOne(service.LOG_URL);
    expect(req.request.url).toBe(`${service.LOG_URL}`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(jasmine.objectContaining({content: msg, type: 'info'}));
    req.flush({content: `$msg`, type: 'info'});
  });

  it('warn function should call the warn function of LocalLogService', () => {
    const msg = 'warn info';
    service.warn(msg);

    expect(localLogServiceSpy.warn).toHaveBeenCalledWith(msg);
  });

  it('warn function should post the correct log item', () => {
    const msg = 'warn info';
    service.warn(msg).subscribe(
      () => {},
      error => {
        fail(error);
      }
    );

    const req = httpMock.expectOne(service.LOG_URL);
    expect(req.request.url).toBe(`${service.LOG_URL}`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(jasmine.objectContaining({content: msg, type: 'warn'}));
    req.flush({content: `$msg`, type: 'warn'});
  });

  it('debug function should call the debug function of LocalLogService', () => {
    const msg = 'debug info';
    service.debug(msg);

    expect(localLogServiceSpy.debug).toHaveBeenCalledWith(msg);
  });

  it('debug function should post the correct log item', () => {
    const msg = 'debug info';
    service.debug(msg).subscribe(
      () => {},
      error => {
        fail(error);
      }
    );

    const req = httpMock.expectOne(service.LOG_URL);
    expect(req.request.url).toBe(`${service.LOG_URL}`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(jasmine.objectContaining({content: msg, type: 'debug'}));
    req.flush({content: `$msg`, type: 'debug'});
  });

  it('error function should call the error function of LocalLogService', () => {
    const msg = 'debug info';
    service.debug(msg);

    expect(localLogServiceSpy.debug).toHaveBeenCalledWith(msg);
  });

  it('error function should post the correct log item', () => {
    const msg = 'error info';
    service.error(msg).subscribe(
      () => {},
      error => {
        fail(error);
      }
    );

    const req = httpMock.expectOne(service.LOG_URL);
    expect(req.request.url).toBe(`${service.LOG_URL}`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(jasmine.objectContaining({content: msg, type: 'error'}));
    req.flush({content: `$msg`, type: 'error'});
  });
});
