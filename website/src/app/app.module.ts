import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER} from '@angular/core';
import { MaterialModule } from './mat.module';
import { RouterModule, Routes  } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { ClientListComponent } from './clients/client-list/client-list.component';
import { CreateClientComponent } from './clients/create-client/create-client.component';
import { PageNotFoundComponent } from './common/page-not-found/page-not-found.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  AuthModule,
  OidcSecurityService,
  OpenIDImplicitFlowConfiguration,
  OidcConfigService,
  AuthWellKnownEndpoints
} from 'angular-auth-oidc-client';
import { AppConfigService } from './services/app-config.service';

export function loadConfig(configService: AppConfigService) {
  console.log('APP_INITIALIZER STARTING');
  return () => configService.getConfig();
}


const appRoutes: Routes = [
  { path: 'main', component: ClientListComponent, data: { title: 'Clients List' } },
  { path: 'create-client', component: CreateClientComponent },
  {
    path: '',
    redirectTo: '/main',
    pathMatch: 'full'
  },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    ClientListComponent,
    CreateClientComponent,
    PageNotFoundComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
	  HttpClientModule,
    RouterModule.forRoot(
      appRoutes,
    )
  ],
  providers: [
    OidcConfigService,
    {
        provide: APP_INITIALIZER,
        useFactory: loadConfig,
        deps: [OidcConfigService],
        multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
