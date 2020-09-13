import { isNullOrUndefined } from './common';

export function isRgbColor(colorString: string): boolean {
    return colorString.trim().startsWith('rgb');
}

export function isHexColor(colorString: string): boolean {
    return !isNullOrUndefined(colorString)
        && /^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$/i.test(colorString);
}

export function getRgbColor(colorString: string): {
    red: number,
    green: number,
    blue: number,
} {
    if (!isRgbColor(colorString)) {
        return undefined;
    }
    const colorGroups: Array<string> =
        colorString.trim().match(/\d+/g);

    return {
        red: parseInt(colorGroups[0], 10),
        green: parseInt(colorGroups[1], 10),
        blue: parseInt(colorGroups[2], 10),
    };
}

export function hexToRgb(hexColor: string): {
    red: number,
    green: number,
    blue: number,
} {
    if (hexColor.startsWith('#')) {
        hexColor = hexColor.substring(1);
    }
    const aRgbHex: RegExpMatchArray =
        hexColor.match(/.{1,2}/g);

    const rgb: Array<number> = [
        parseInt(aRgbHex[0], 16),
        parseInt(aRgbHex[1], 16),
        parseInt(aRgbHex[2], 16)
    ];

    return {
        red: rgb[0],
        green: rgb[1],
        blue: rgb[2],
    };
}

const blackHex: string = '#000';
const whiteHex: string = '#fff';

export function getFontColor(colorString: string): string {
    const rgbColor: {
        red: number,
        green: number,
        blue: number,
    } = isRgbColor(colorString)
            ? getRgbColor(colorString)
            : hexToRgb(colorString);

    if (!isNullOrUndefined(rgbColor)) {
        const intensity: number
            = rgbColor.red * 0.299
            + rgbColor.green * 0.587
            + rgbColor.blue * 0.114;

        return intensity > 125 ? blackHex : whiteHex;
    }

    return blackHex;
}