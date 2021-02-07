import { isNullOrUndefined } from './common';

export interface Color {
    red: number;
    green: number;
    blue: number;
}

export function isRgbColor(colorString: string): boolean {
    return colorString.trim().startsWith('rgb');
}

export function isHexColor(colorString: string): boolean {
    return !isNullOrUndefined(colorString)
        && /^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$/i.test(colorString);
}

export function getRgbColor(colorString: string): Color {
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

export function hexToRgb(hexColor: string): Color {
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

export const blackHex: string = '#000';
export const whiteHex: string = '#fff';

export function getFontColorFromString(colorString: string): string {
    const rgbColor: Color =
        isRgbColor(colorString)
            ? getRgbColor(colorString)
            : hexToRgb(colorString);

    return getFontColor(rgbColor);
}

export function getFontColor(color: Color): string {
    if (!isNullOrUndefined(color)) {
        const intensity: number
            = color.red * 0.299
            + color.green * 0.587
            + color.blue * 0.114;

        return intensity > 125 ? blackHex : whiteHex;
    }

    return blackHex;
}