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

export function matchesOtherFormControlValidator(
  other: AbstractControl
): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    let valid = false;
    if (isString(control.value) && isString(other.value)) {
      valid =
        (control.value ?? "").toUpperCase() ===
        (other.value ?? "").toUpperCase();
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
      valid =
        (control.value ?? "").toUpperCase() === (otherVal ?? "").toUpperCase();
    } else {
      valid = control.value === otherVal;
    }
    return !valid ? { invalid: { value: control.value } } : null;
  };
}
