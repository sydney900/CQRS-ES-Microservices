import { NgModule } from '@angular/core';
import { MatButtonModule, MatInputModule, MatFormFieldModule, MatIconModule, MatListModule,
  MatToolbarModule, MatSidenavModule } from '@angular/material';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import {MatCardModule} from '@angular/material/card';


@NgModule({
  imports: [MatButtonModule, MatInputModule, MatFormFieldModule, MatIconModule, MatListModule,
    MatSnackBarModule, MatToolbarModule, MatSidenavModule, MatCardModule],
  exports: [MatButtonModule, MatInputModule, MatFormFieldModule, MatIconModule, MatListModule,
    MatSnackBarModule, MatToolbarModule, MatSidenavModule, MatCardModule],
})
export class MaterialModule { }
