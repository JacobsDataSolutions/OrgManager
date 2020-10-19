export class GuidValidator {
  constructor() {}

  public static isValidGuid(guid: string): boolean {
    const regex = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;
    if (guid) {
      return regex.test(guid);
    }
    return false;
  }
}
