import { Injectable, Inject } from "@angular/core";
import { Observable, of } from "rxjs";
import { LocalStorageService } from "../core/core.module";
import { tap } from "rxjs/operators";
import { TenantClient } from "../shared/nswag";
import { ActivatedRoute } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class TenantService {
    constructor(private tenantClient: TenantClient, private localStorageService: LocalStorageService, private route: ActivatedRoute) {}

    getSlug(): string {
        const routeSnapshot = this.route.snapshot;
        if (routeSnapshot.paramMap.has("slug")) {
            return routeSnapshot.paramMap.get("slug");
        }
        return "";
    }

    getCachedTenantIdFromSlug(slug: string): Observable<number> {
        if (!slug) {
            return of(0);
        }
        const storageKey = "tenant-" + slug;
        const tenantId: number = this.localStorageService.getItem(storageKey);
        if (tenantId) {
            return of(tenantId);
        } else {
            return this.tenantClient.getTenantIdFromSlug(slug).pipe(tap((tenantId) => this.localStorageService.setItem(storageKey, tenantId)));
        }
    }

    getTenantLink(urlSubPath: string) {
        const slug = this.getSlug();
        const path = (urlSubPath ?? "").replace("/", "");
        return `/t/${slug}/${path}`;
    }
}
