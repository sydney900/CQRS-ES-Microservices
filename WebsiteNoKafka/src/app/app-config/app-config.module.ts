import { NgModule, APP_INITIALIZER } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppConfigService } from './app-config.service';
import { AuthModule, OidcSecurityService, OpenIDImplicitFlowConfiguration, AuthWellKnownEndpoints } from 'angular-auth-oidc-client';

export function loadConfig(configService: AppConfigService) {
  console.log('APP_INITIALIZER STARTING');
  return () => configService.getConfig();
}

@NgModule({
  imports: [
    CommonModule,
    AuthModule.forRoot()
  ],
  providers: [
    AppConfigService,
    {
      provide: APP_INITIALIZER,
      useFactory: loadConfig,
      deps: [AppConfigService],
      multi: true
    },
    OidcSecurityService,
  ],
  declarations: []
})
export class AppConfigModule { 
  constructor(configService: AppConfigService, private oidcSecurityService: OidcSecurityService) {
    configService.getConfig().then(config => {
      const c = new OpenIDImplicitFlowConfiguration();
      c.stsServer = config.authUrl;
      c.redirect_url = window.location.origin;
      c.client_id = config.clientId;
      c.response_type = 'id_token token';
      c.scope = config.scope;
      c.post_logout_redirect_uri = window.location.origin;
      c.start_checksession = true;
      c.silent_renew = false;
      c.post_login_route = '/';
      c.forbidden_route = '/forbidden';
      c.unauthorized_route = '/unauthorized';
      c.auto_userinfo = true;
      c.log_console_warning_active = true;
      c.log_console_debug_active = true;
      c.max_id_token_iat_offset_allowed_in_seconds = 10;
      const wn = new AuthWellKnownEndpoints();
      wn.setWellKnownEndpoints(configService.wellKnownEndpoints);
      oidcSecurityService.setupModule(c, wn);
    });
  }
}
