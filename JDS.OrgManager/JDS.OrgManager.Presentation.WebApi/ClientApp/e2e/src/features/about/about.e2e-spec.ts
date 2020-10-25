// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { AboutPage } from "./about.po";
import { getCurrentRouteUrl } from "../../utils/utils";

describe("About Page", () => {
    let page: AboutPage;

    beforeEach(() => (page = new AboutPage()));

    it("should display main heading", () => {
        page.navigateTo();
        expect(page.getParagraphText()).toEqual(
            "ANGULAR NGRX MATERIAL STARTER"
        );
    });

    it('should display "Geting Started" section', () => {
        page.navigateTo();
        page.getGettingStarted()
            .isPresent()
            .then((isPresent) => expect(isPresent).toBe(true));
    });

    it('first action button should lead to "Features" route', () => {
        page.navigateTo();
        page.getActionButton(0)
            .click()
            .then(() => {
                expect(getCurrentRouteUrl()).toBe("feature-list");
            });
    });
});
