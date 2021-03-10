"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.isEmptyInputValue = void 0;
function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if (new Date().getTime() - start > milliseconds) {
            break;
        }
    }
}
function isEmptyInputValue(value) {
    // we don't check for string here so it also works with arrays
    return value === null || value.length === 0;
}
exports.isEmptyInputValue = isEmptyInputValue;
//# sourceMappingURL=functions.js.map