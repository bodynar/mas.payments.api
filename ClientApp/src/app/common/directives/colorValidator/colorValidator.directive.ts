import { Directive } from '@angular/core';
import { NG_VALIDATORS, AbstractControl } from '@angular/forms';

import { isNullOrUndefined } from 'common/utils/common';
import { isRgbColor, isHexColor } from 'common/utils/colors';
import { BaseValidatorDirective } from '../baseValidatorDirective';

@Directive({
    selector: '[appIsValidColor]',
    providers: [
        { provide: NG_VALIDATORS, useExisting: ColorValidatorDirective, multi: true }
    ]
})
export class ColorValidatorDirective extends BaseValidatorDirective {
    public validatorName: string =
        'appIsValidColor';

    public validate(formControl: AbstractControl): { [key: string]: any } {
        const value: string =
            formControl.value;

        if (!isNullOrUndefined(value) && value !== '') {
            const isValidColor: boolean =
                isRgbColor(value) || isHexColor(value);

            return this.getValidationResult(isValidColor);
        }

        return this.getInvalidValidateResult();
    }
}