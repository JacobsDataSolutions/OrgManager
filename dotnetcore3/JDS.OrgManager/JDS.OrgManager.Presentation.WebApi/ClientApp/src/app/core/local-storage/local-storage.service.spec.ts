// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { TestBed } from "@angular/core/testing";

import { LocalStorageService } from "./local-storage.service";

describe("LocalStorageService", () => {
    let service: LocalStorageService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [LocalStorageService]
        });
        service = TestBed.inject<LocalStorageService>(LocalStorageService);
    });

    afterEach(() => localStorage.clear());

    it("should be created", () => {
        expect(service).toBeTruthy();
    });

    it("testLocalStorage should be executable", () => {
        spyOn(service, "testLocalStorage");
        service.testLocalStorage();
        expect(service.testLocalStorage).toHaveBeenCalled();
    });

    it("should get, set, and remove the item", () => {
        service.setItem("TEST", "item");
        expect(service.getItem("TEST")).toBe("item");
        service.removeItem("TEST");
        expect(service.getItem("TEST")).toBe(null);
    });

    it("should load initial state", () => {
        service.setItem("TEST.PROP", "value");
        expect(LocalStorageService.loadInitialState()).toEqual({
            test: { prop: "value" }
        });
    });

    it("should load nested initial state", () => {
        service.setItem("TEST.PROP1.PROP2", "value");
        expect(LocalStorageService.loadInitialState()).toEqual({
            test: { prop1: { prop2: "value" } }
        });
    });

    it("should load initial state with camel case property", () => {
        service.setItem("TEST.SUB-PROP", "value");
        expect(LocalStorageService.loadInitialState()).toEqual({
            test: { subProp: "value" }
        });
    });

    it("should load nested initial state with camel case properties", () => {
        service.setItem("TEST.SUB-PROP.SUB-SUB-PROP", "value");
        expect(LocalStorageService.loadInitialState()).toEqual({
            test: { subProp: { subSubProp: "value" } }
        });
    });
});
