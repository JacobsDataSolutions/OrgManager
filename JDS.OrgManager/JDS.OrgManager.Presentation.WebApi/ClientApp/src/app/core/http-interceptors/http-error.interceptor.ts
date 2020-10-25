// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { Injectable, Injector } from "@angular/core";
import {
    HttpEvent,
    HttpInterceptor,
    HttpHandler,
    HttpRequest,
    HttpErrorResponse
} from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { tap, catchError } from "rxjs/operators";
import { NotificationService } from "../notifications/notification.service";
import { Router } from "@angular/router";

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
    constructor(private injector: Injector) {}

    intercept(
        request: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError((response: Response) => {
                const reader = new FileReader();

                const notificationService = this.injector.get(
                    NotificationService
                );
                const router = this.injector.get(Router);

                let errorMessage = "";
                switch (response.status) {
                    case 401:
                        router.navigate(["unauthorized"]);
                        break;
                    case 404:
                        errorMessage = "Not Found";
                        break;
                    case 403:
                        errorMessage = "Forbidden";
                        break;
                    case 500:
                        errorMessage = "Server Error";
                        break;
                    case 400:
                        errorMessage = "Bad Request";
                        break;
                }

                reader.onload = function () {
                    const result = JSON.parse(this.result as string);
                    try {
                        errorMessage = result.errorMessage;
                    } catch {}
                };

                reader.readAsText(response["error"]);
                if (errorMessage) {
                    notificationService.error(errorMessage);
                }

                return throwError(errorMessage);
            })
        );
    }
}
