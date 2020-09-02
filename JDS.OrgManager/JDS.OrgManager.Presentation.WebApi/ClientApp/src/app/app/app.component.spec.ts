// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { NoopAnimationsModule } from "@angular/platform-browser/animations";
import { TestBed, async } from "@angular/core/testing";
import { RouterTestingModule } from "@angular/router/testing";
import { TranslateModule } from "@ngx-translate/core";
import { MockStore, provideMockStore } from "@ngrx/store/testing";

import {
  selectEffectiveTheme,
  selectIsAuthenticated,
  selectSettingsLanguage,
  selectSettingsStickyHeader
} from "../core/core.module";
import { SharedModule } from "../shared/shared.module";

import { AppComponent } from "./app.component";

describe("AppComponent", () => {
  let store: MockStore;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        SharedModule,
        RouterTestingModule,
        NoopAnimationsModule,
        TranslateModule.forRoot()
      ],
      providers: [provideMockStore()],
      declarations: [AppComponent]
    }).compileComponents();

    store = TestBed.inject(MockStore);
    store.overrideSelector(selectIsAuthenticated, false);
    store.overrideSelector(selectSettingsStickyHeader, true);
    store.overrideSelector(selectSettingsLanguage, "en");
    store.overrideSelector(selectEffectiveTheme, "default");
  }));

  it("should create the app", async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  }));
});
