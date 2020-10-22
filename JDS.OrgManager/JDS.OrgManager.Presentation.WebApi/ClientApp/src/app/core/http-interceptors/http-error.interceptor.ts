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
import { tap } from "rxjs/operators";
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
            //retry(1),
            tap({
                error: (error: any) => {
                    let errorMessage = "";
                    if (error.error instanceof ErrorEvent) {
                        // client-side error
                        errorMessage = `Error: ${error.error.message}`;
                    } else {
                        // server-side error
                        errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
                    }
                    const notificationService = this.injector.get(
                        NotificationService
                    );
                    const router = this.injector.get(Router);
                    if (error instanceof HttpErrorResponse) {
                        // Get server-side error
                        switch (error.status) {
                            case 401:
                                router.navigate(["unauthorized"]);
                                return;
                            case 404:
                                errorMessage = error.error.error;
                                break;
                            case 400:
                                errorMessage = error.error.error;
                                break;
                        }
                    }

                    notificationService.error(errorMessage);
                    return throwError(errorMessage);
                }
            })
        );
    }
}
