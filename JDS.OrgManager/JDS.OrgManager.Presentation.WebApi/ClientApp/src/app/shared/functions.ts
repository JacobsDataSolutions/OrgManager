function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if (new Date().getTime() - start > milliseconds) {
            break;
        }
    }
}

export function isEmptyInputValue(value: any): boolean {
    // we don't check for string here so it also works with arrays
    return value === null || value.length === 0;
}
