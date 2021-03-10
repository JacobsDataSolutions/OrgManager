// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { AnimationsService } from "./animations.service";

describe("AnimationsService", () => {
    let service: AnimationsService;

    beforeEach(() => {
        service = new AnimationsService();
    });

    it('should set route animation type to "NONE" by default', () => {
        expect(AnimationsService.isRouteAnimationsType("NONE")).toBe(true);
    });

    it('should set route animation type to "ALL"', () => {
        service.updateRouteAnimationType(true, true);
        expect(AnimationsService.isRouteAnimationsType("ALL")).toBe(true);
    });

    it('should set route animation type to "PAGE"', () => {
        service.updateRouteAnimationType(true, false);
        expect(AnimationsService.isRouteAnimationsType("PAGE")).toBe(true);
    });

    it('should set route animation type to "ELEMENTS"', () => {
        service.updateRouteAnimationType(false, true);
        expect(AnimationsService.isRouteAnimationsType("ELEMENTS")).toBe(true);
    });
});
