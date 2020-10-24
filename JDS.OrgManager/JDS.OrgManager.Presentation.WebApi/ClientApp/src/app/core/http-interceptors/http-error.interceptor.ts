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

                //TODO: clean this up and handle response w/ no messages.
                reader.onload = function () {
                    const result = JSON.parse(this.result as string);
                    switch (response.status) {
                        case 401:
                            router.navigate(["unauthorized"]);
                            break;
                        //return;
                        case 404:
                        case 403:
                        case 500:
                        case 400:
                            errorMessage = result.errorMessage;
                            notificationService.error(result.errorMessage);
                            break;
                    }
                };

                reader.readAsText(response["error"]);

                return throwError(errorMessage);

                //if (errorMessage) {
                //    return throwError(errorMessage);
                //}
                //return request;
            })
        );
    }

    //public handleError = (error: Response) => {
    //    let reader = new FileReader();

    //    let ngNotify = this._ngNotify;

    //    reader.onload = function () {
    //        var result = JSON.parse(this.result);

    //        ngNotify.nofity("Error", result.error);
    //    };

    //    reader.readAsText(error["error"]);

    //    return Observable.throw(error);
    //};

    //return next.handle(request).pipe(
    //    //retry(1),
    //    tap({
    //        error: (error: any) => {
    //            let errorMessage = "";
    //            if (error.error instanceof ErrorEvent) {
    //                // client-side error
    //                errorMessage = `Error: ${error.error.message}`;
    //            } else {
    //                // server-side error
    //                errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    //            }
    //            const notificationService = this.injector.get(
    //                NotificationService
    //            );
    //            const router = this.injector.get(Router);
    //            const httpErrorResponse = error as HttpErrorResponse;
    //            if (httpErrorResponse) {
    //                // Get server-side error
    //                switch (error.status) {
    //                    case 401:
    //                        router.navigate(["unauthorized"]);
    //                        return;
    //                    case 404:
    //                    case 400:
    //                        errorMessage =
    //                            httpErrorResponse.error.errorText;
    //                        break;
    //                }
    //            }

    //            notificationService.error(errorMessage);
    //            return throwError(errorMessage);
    //        }
    //    })
    //);
    //}
}
