import { TestBed, inject } from '@angular/core/testing';

import { ClientService } from './client.service';

describe('ClientserviceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ClientserviceService]
    });
  });

  it('should be created', inject([ClientserviceService], (service: ClientserviceService) => {
    expect(service).toBeTruthy();
  }));
});
