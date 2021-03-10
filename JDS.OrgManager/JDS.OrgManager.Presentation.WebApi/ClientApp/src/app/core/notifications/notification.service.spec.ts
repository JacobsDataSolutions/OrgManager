// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { TestBed } from "@angular/core/testing";

import { NotificationService } from "./notification.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Overlay } from "@angular/cdk/overlay";

describe("NotificationsService", () => {
    let service: NotificationService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [NotificationService, MatSnackBar, Overlay]
        });
        service = TestBed.inject<NotificationService>(NotificationService);
    });

    it("should be created", () => {
        expect(service).toBeTruthy();
    });

    it("default method should be executable", () => {
        spyOn(service, "default");
        service.default("default message");
        expect(service.default).toHaveBeenCalled();
    });

    it("info method should be executable", () => {
        spyOn(service, "info");
        service.info("info message");
        expect(service.info).toHaveBeenCalled();
    });

    it("success method should be executable", () => {
        spyOn(service, "success");
        service.success("success message");
        expect(service.success).toHaveBeenCalled();
    });

    it("warning method should be executable", () => {
        spyOn(service, "warn");
        service.warn("warning message");
        expect(service.warn).toHaveBeenCalled();
    });

    it("error method should be executable", () => {
        spyOn(service, "error");
        service.error("error message");
        expect(service.error).toHaveBeenCalled();
    });
});
