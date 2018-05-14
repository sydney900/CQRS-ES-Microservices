import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule, MatInputModule, MatFormFieldModule, MatIconModule, MatListModule, MatCardModule } from '@angular/material';

@NgModule({
  imports: [NoopAnimationsModule, MatButtonModule, MatInputModule, MatFormFieldModule, MatIconModule, MatListModule, MatCardModule],
  exports: [NoopAnimationsModule, MatButtonModule, MatInputModule, MatFormFieldModule, MatIconModule, MatListModule, MatCardModule],
})
export class MaterialModule { }
