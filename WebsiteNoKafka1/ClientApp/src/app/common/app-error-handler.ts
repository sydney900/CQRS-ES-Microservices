import { ErrorHandler, Injectable } from '@angular/core';
import { LogService } from '../services/log.service';
import { NotificationService } from '../services/notification.service';

@Injectable({
    providedIn: 'root'
})
export class AppErrorHandler implements ErrorHandler {
    constructor(private log: LogService, private notification: NotificationService) {
    }

    handleError(error: any): void {
        this.log.error(error);

        const message = error.message ? '\n' + error.message : '';
        this.notification.showError('An unexpected error occurred' + message);
    }
}
