// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { ValidatorFn, AbstractControl } from "@angular/forms";
import { GuidValidator } from "./utils";

export function isValidGuidValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const invalid = !GuidValidator.isValidGuid(control.value);
        return invalid ? { invalidGuid: { value: control.value } } : null;
    };
}

function isString(value: any) {
    return typeof value === "string" || value instanceof String;
}

export function matchesOtherFormControlValidator(other: AbstractControl): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        let valid = false;
        if (isString(control.value) && isString(other.value)) {
            valid = (control.value ?? "").toUpperCase() === (other.value ?? "").toUpperCase();
        } else {
            valid = control.value === other.value;
        }
        return !valid ? { invalid: { value: control.value } } : null;
    };
}

export function matchesPropertyValidator(other: () => string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        let valid = false;
        const otherVal = other();
        if (isString(control.value)) {
            valid = (control.value ?? "").toUpperCase() === (otherVal ?? "").toUpperCase();
        } else {
            valid = control.value === otherVal;
        }
        return !valid ? { invalid: { value: control.value } } : null;
    };
}
