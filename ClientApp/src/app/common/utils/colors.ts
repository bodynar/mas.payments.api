import { isNullOrUndefined } from './common';

export function hexToRgb(hexColor: string): {
    red: number,
    green: number,
    blue: number,
} {
    if (hexColor.startsWith('#')) {
        hexColor = hexColor.substring(1);
    }
    const aRgbHex: RegExpMatchArray
        = hexColor.substring(1).match(/.{1,2}/g);

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

export function getFontColor(hexColor: string): string {
    const rgbColor: {
        red: number,
        green: number,
        blue: number,
    } = hexToRgb(hexColor);

    if (!isNullOrUndefined(rgbColor)) {
        const intensity: number
            = rgbColor.red * 0.299
            + rgbColor.green * 0.587
            + rgbColor.blue * 0.114;

        return intensity > 186 ? blackHex : whiteHex;
    }

    return blackHex;
}