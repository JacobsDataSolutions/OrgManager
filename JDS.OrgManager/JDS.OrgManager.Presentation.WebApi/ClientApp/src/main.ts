import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

// Spliced in from ASP.NET Core scaffolding.
export function getBaseUrl() {
    return document.getElementsByTagName("base")[0].href;
}

// Spliced in from ASP.NET Core scaffolding.
const providers = [{ provide: "BASE_URL", useFactory: getBaseUrl, deps: [] }];

if (environment.production) {
    enableProdMode();
}

//platformBrowserDynamic()
//  .bootstrapModule(AppModule)
//  .catch((err) => console.error(err));

// Spliced in from ASP.NET Core scaffolding.
platformBrowserDynamic(providers).bootstrapModule(AppModule)
    .catch(err => console.log(err));
