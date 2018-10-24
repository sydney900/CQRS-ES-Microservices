import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private snackBar: MatSnackBar) { }

  public showError(error: string): void {
    this.snackBar.open(error, '',
      {
        duration: 3000,
        verticalPosition: 'top',
        horizontalPosition: 'right',
        panelClass: ['red-snackbar']
      });
  }

  public showSuccess(notification: string): void {
    this.snackBar.open(notification, '', { duration: 3000, verticalPosition: 'top', horizontalPosition: 'right' });
  }
}
