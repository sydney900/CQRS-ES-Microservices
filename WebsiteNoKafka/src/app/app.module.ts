import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler, APP_INITIALIZER } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { MaterialModule } from './mat.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppErrorHandler } from './common/app-error-handler';
import { HttpErrorInterceptor } from './common/http-error-interceptor';
import { AboutComponent } from './about/about.component';
import { LoginComponent } from './login/login.component';
import { JwtInterceptor } from './common/jwt-interceptor';
import { AuthGuard } from './services/auth-guard.service';
import { ClientListComponent } from './clients/client-list/client-list.component';
import { CreateClientComponent } from './clients/create-client/create-client.component';
import { AuthModule, OidcSecurityService, OpenIDImplicitFlowConfiguration, AuthWellKnownEndpoints } from 'angular-auth-oidc-client';
import { AppConfigService } from './services/app-config.service';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';

@NgModule({
  declarations: [
    AppComponent,
    AboutComponent,
    LoginComponent,
    ClientListComponent,
    CreateClientComponent,
    UnauthorizedComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: '', component: ClientListComponent, canActivate: [AuthGuard] },
      { path: 'create-client', component: CreateClientComponent, canActivate: [AuthGuard]},
      { path: 'login', component: LoginComponent },
      { path: 'about', component: AboutComponent }
    ]),
    AuthModule.forRoot()
  ],
  providers: [
    { provide: ErrorHandler, useClass: AppErrorHandler },
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true, },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(configService: AppConfigService, private oidcSecurityService: OidcSecurityService) {
    configService.getConfig().then(config => {
      const c = new OpenIDImplicitFlowConfiguration();
      c.stsServer = config.authUrl;
      c.redirect_url = window.location.origin;
      c.client_id = config.clientId;
      c.response_type = 'id_token token';
      c.scope = 'openid profile email role core.api';
      c.post_logout_redirect_uri = window.location.origin + '/unauthorized';
      c.forbidden_route = '/forbidden';
      c.unauthorized_route = '/unauthorized';
      c.auto_userinfo = true;
      c.log_console_warning_active = true;
      c.log_console_debug_active = true;
      c.max_id_token_iat_offset_allowed_in_seconds = 10;
      c.start_checksession = false;
      c.silent_renew = false;
      const wn = new AuthWellKnownEndpoints();
      wn.setWellKnownEndpoints(configService.wellKnownEndpoints);
      oidcSecurityService.setupModule(c, wn);
    });
  }
}
