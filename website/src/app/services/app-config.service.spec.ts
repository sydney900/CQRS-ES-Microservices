import { TestBed, inject } from '@angular/core/testing';

import { AppConfigService } from './app-config.service';

describe('AppConfigServiceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AppConfigServiceService]
    });
  });

  it('should be created', inject([AppConfigServiceService], (service: AppConfigServiceService) => {
    expect(service).toBeTruthy();
  }));
});
