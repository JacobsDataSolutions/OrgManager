// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

const packageJson = require("../../package.json");

export const environment = {
    appName: "Org Manager",
    envName: "DEV",
    production: false,
    test: false,
    i18nPrefix: "",
    versions: {
        app: packageJson.version,
        angular: packageJson.dependencies["@angular/core"],
        ngrx: packageJson.dependencies["@ngrx/store"],
        material: packageJson.dependencies["@angular/material"],
        bootstrap: packageJson.dependencies.bootstrap,
        rxjs: packageJson.dependencies.rxjs,
        ngxtranslate: packageJson.dependencies["@ngx-translate/core"],
        fontAwesome: packageJson.dependencies["@fortawesome/fontawesome-free"],
        angularCli: packageJson.devDependencies["@angular/cli"],
        typescript: packageJson.devDependencies["typescript"],
        cypress: packageJson.devDependencies["cypress"],
        eslint: packageJson.devDependencies["eslint"]
    }
};
