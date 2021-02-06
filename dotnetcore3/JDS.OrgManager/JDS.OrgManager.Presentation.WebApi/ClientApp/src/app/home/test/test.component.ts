import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    OnDestroy
} from "@angular/core";
import { TestClient } from "../../shared/nswag";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";

@Component({
    selector: "org-manager-test",
    templateUrl: "./test.component.html",
    styleUrls: ["./test.component.scss"],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class TestComponent implements OnInit, OnDestroy {
    private ngUnsubscribe = new Subject();

    constructor(private testClient: TestClient) {}

    ngOnInit(): void {}

    onTestApplicationLayerException() {
        this.testClient
            .testApplicationLayerException()
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe();
    }

    onTestAuthorizationException() {
        this.testClient
            .testAuthorizationException()
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe();
    }

    onTestNotFoundException() {
        this.testClient
            .testNotFoundException()
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe();
    }

    onTestInvalidOperationException() {
        this.testClient
            .testInvalidOperationException()
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe();
    }

    ngOnDestroy() {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }
}
