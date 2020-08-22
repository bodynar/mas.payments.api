import { Directive, SimpleChanges, OnChanges } from '@angular/core';
import { Validator, NG_VALIDATORS, AbstractControl } from '@angular/forms';

import { isNullOrUndefined } from 'common/utils/common';
import { isRgbColor, isHexColor } from 'common/utils/colors';

@Directive({
    selector: '[appIsValidColor]',
    providers: [
        { provide: NG_VALIDATORS, useExisting: ColorValidatorDirective, multi: true }
    ]
})
export class ColorValidatorDirective implements Validator, OnChanges {
    private validatorName: string =
        'appIsValidColor';

    private readonly INVALID_VALIDATE_RESULT: { [key: string]: any } =
        { appIsValidColor: { valid: false } };

    private onChange: () => void;

    public validate(formControl: AbstractControl): { [key: string]: any } {
        const value: string =
            formControl.value;

        if (!isNullOrUndefined(value) && value !== '') {
            const isValidColor: boolean =
                isRgbColor(value) || isHexColor(value);

            return isValidColor
                ? null
                : this.INVALID_VALIDATE_RESULT;
        }

        return this.INVALID_VALIDATE_RESULT;
    }

    public registerOnValidatorChange(fn: () => void): void {
        this.onChange = fn;
    }

    public ngOnChanges(changes: SimpleChanges): void {
        if (this.validatorName in changes && this.onChange) {
            this.onChange();
        }
    }
}