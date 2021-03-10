// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { enableProdMode } from "@angular/core";
import { platformBrowserDynamic } from "@angular/platform-browser-dynamic";

import { AppModule } from "./app/app.module";
import { environment } from "./environments/environment";

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
platformBrowserDynamic(providers)
    .bootstrapModule(AppModule)
    .catch((err) => console.log(err));
