import { NoopAnimationsModule } from "@angular/platform-browser/animations";
import { TestBed, async } from "@angular/core/testing";
import { RouterTestingModule } from "@angular/router/testing";
import { TranslateModule } from "@ngx-translate/core";
import { MockStore, provideMockStore } from "@ngrx/store/testing";
import { MatSidenavModule } from "@angular/material/sidenav";
import { MatToolbarModule } from "@angular/material/toolbar";

import { SharedModule } from "../shared/shared.module";

import { AppComponent } from "./app.component";

describe("AppComponent", () => {
    let store: MockStore;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [SharedModule, MatSidenavModule, MatToolbarModule, RouterTestingModule, NoopAnimationsModule, TranslateModule.forRoot()],
            providers: [provideMockStore()],
            declarations: [AppComponent]
        }).compileComponents();

        store = TestBed.inject(MockStore);
    }));

    it("should create the app", async(() => {
        const fixture = TestBed.createComponent(AppComponent);
        const app = fixture.debugElement.componentInstance;
        expect(app).toBeTruthy();
    }));
});
