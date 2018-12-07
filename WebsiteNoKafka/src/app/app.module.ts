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
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { AppConfigModule } from './app-config/app-config.module';

@NgModule({
  declarations: [
    AppComponent,
    AboutComponent,
    LoginComponent,
    ClientListComponent,
    CreateClientComponent,
    UnauthorizedComponent,
    ForbiddenComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    HttpClientModule,
    AppConfigModule,
    RouterModule.forRoot([
      //{ path: '', component: ClientListComponent, canActivate: [AuthGuard] },
      //{ path: 'create-client', component: CreateClientComponent, canActivate: [AuthGuard]},
      { path: '', component: ClientListComponent },
      { path: 'create-client', component: CreateClientComponent, canActivate: [AuthGuard] },
      { path: 'login', component: LoginComponent },
      { path: 'unauthorized', component: UnauthorizedComponent},
      { path: 'forbidden', component: ForbiddenComponent},
      { path: 'about', component: AboutComponent }
    ])
  ],
  providers: [
    { provide: ErrorHandler, useClass: AppErrorHandler },
    //{ provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true, },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
