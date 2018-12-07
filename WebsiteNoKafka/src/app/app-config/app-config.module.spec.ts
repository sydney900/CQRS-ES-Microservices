import { AppConfigModule } from './app-config.module';

describe('AppConfigModule', () => {
  let appConfigModule: AppConfigModule;

  beforeEach(() => {
    appConfigModule = new AppConfigModule(null, null);
  });

  it('should create an instance', () => {
    expect(appConfigModule).toBeTruthy();
  });
});
