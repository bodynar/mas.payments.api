const hashSigns: Array<string> = [];

function generateHashSigns(): void {
    for (let i = 0; i < 256; i++) {
        hashSigns[i] = (i < 16 ? '0' : '') + (i).toString(16);
    }
}

export function generateGuid(): string {
    const d0 = Math.random() * 0xffffffff | 0;
    const d1 = Math.random() * 0xffffffff | 0;
    const d2 = Math.random() * 0xffffffff | 0;
    const d3 = Math.random() * 0xffffffff | 0;

    if (hashSigns.length === 0) {
        generateHashSigns();
    }

    return hashSigns[d0 & 0xff] + hashSigns[d0 >> 8 & 0xff] + hashSigns[d0 >> 16 & 0xff] + hashSigns[d0 >> 24 & 0xff] + '-' +
        hashSigns[d1 & 0xff] + hashSigns[d1 >> 8 & 0xff] + '-' + hashSigns[d1 >> 16 & 0x0f | 0x40] + hashSigns[d1 >> 24 & 0xff] + '-' +
        hashSigns[d2 & 0x3f | 0x80] + hashSigns[d2 >> 8 & 0xff] + '-' + hashSigns[d2 >> 16 & 0xff] + hashSigns[d2 >> 24 & 0xff] +
        hashSigns[d3 & 0xff] + hashSigns[d3 >> 8 & 0xff] + hashSigns[d3 >> 16 & 0xff] + hashSigns[d3 >> 24 & 0xff];
}

export function isNullOrUndefined(arg: any): boolean {
    return arg === undefined || arg === null;
}

export function isNullOrEmpty(arg: string): boolean {
    return isNullOrUndefined(arg) || arg === '';
}