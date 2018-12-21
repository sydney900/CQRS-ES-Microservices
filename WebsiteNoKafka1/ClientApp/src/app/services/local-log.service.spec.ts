import { TestBed, inject } from '@angular/core/testing';

import { LocalLogService } from './local-log.service';

describe('LocalLogService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LocalLogService]
    });
  });

  it('should be created', inject([LocalLogService], (service: LocalLogService) => {
    expect(service).toBeTruthy();
  }));

  it('log methed should work', inject([LocalLogService], (service: LocalLogService) => {
    expect(service.log('test')).toBeUndefined();
  }));

  it('info methed should work', inject([LocalLogService], (service: LocalLogService) => {
    expect(service.info('test')).toBeUndefined();
  }));

  it('debug methed should work', inject([LocalLogService], (service: LocalLogService) => {
    expect(service.debug('test')).toBeUndefined();
  }));

  it('warn methed should work', inject([LocalLogService], (service: LocalLogService) => {
    expect(service.warn('test')).toBeUndefined();
  }));

  it('error methed should work', inject([LocalLogService], (service: LocalLogService) => {
    expect(service.error('test')).toBeUndefined();
  }));
});
