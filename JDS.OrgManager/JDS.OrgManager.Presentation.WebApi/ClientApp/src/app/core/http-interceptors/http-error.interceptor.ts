// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { Injectable, Injector } from "@angular/core";
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { NotificationService } from "../notifications/notification.service";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class HttpErrorInterceptor implements HttpInterceptor {
    constructor(private injector: Injector) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError((response) => {
                if (response instanceof HttpErrorResponse && response.error instanceof Blob && response.error.type === "application/problem+json") {
                    return new Promise<any>((resolve, reject) => {
                        const notificationService = this.injector.get(NotificationService);
                        const router = this.injector.get(Router);

                        if (response.status === 401) {
                            router.navigate(["unauthorized"]);
                        } else {
                            const reader = new FileReader();
                            reader.onload = (e: Event) => {
                                try {
                                    const error = JSON.parse((<any>e.target).result);
                                    notificationService.error(error.title);
                                    reject(
                                        new HttpErrorResponse({
                                            error: error.title + " / " + error.detail,
                                            headers: response.headers,
                                            status: response.status,
                                            statusText: response.statusText,
                                            url: response.url
                                        })
                                    );
                                } catch (e) {
                                    reject(response);
                                }
                            };
                            reader.onerror = (e) => {
                                reject(response);
                            };
                            reader.readAsText(response.error);
                        }
                    });
                }

                return throwError(response);
            })
        );
    }
}
