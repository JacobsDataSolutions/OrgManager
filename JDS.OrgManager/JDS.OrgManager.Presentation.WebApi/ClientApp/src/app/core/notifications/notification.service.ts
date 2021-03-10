// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { Injectable, NgZone } from "@angular/core";
import { MatSnackBar, MatSnackBarConfig } from "@angular/material/snack-bar";

@Injectable({
    providedIn: "root"
})
export class NotificationService {
    constructor(private readonly snackBar: MatSnackBar, private readonly zone: NgZone) {}

    default(message: string) {
        this.show(message, {
            duration: 2000,
            panelClass: "default-notification-overlay"
        });
    }

    info(message: string) {
        this.show(message, {
            duration: 2000,
            panelClass: "info-notification-overlay"
        });
    }

    success(message: string) {
        this.show(message, {
            duration: 2000,
            panelClass: "success-notification-overlay"
        });
    }

    warn(message: string) {
        this.show(message, {
            duration: 2500,
            panelClass: "warning-notification-overlay"
        });
    }

    error(message: string) {
        this.show(message, {
            duration: 3000,
            panelClass: "error-notification-overlay"
        });
    }

    private show(message: string, configuration: MatSnackBarConfig) {
        // Need to open snackBar from Angular zone to prevent issues with its position per
        // https://stackoverflow.com/questions/50101912/snackbar-position-wrong-when-use-errorhandler-in-angular-5-and-material
        this.zone.run(() => this.snackBar.open(message, null, configuration));
    }
}
