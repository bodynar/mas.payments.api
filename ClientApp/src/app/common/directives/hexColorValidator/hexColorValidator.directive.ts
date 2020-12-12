import { Directive } from '@angular/core';
import { NG_VALIDATORS, AbstractControl } from '@angular/forms';

import { isNullOrUndefined } from 'common/utils/common';
import { isHexColor } from 'common/utils/colors';

import { BaseValidatorDirective } from '../baseValidatorDirective';

@Directive({
    selector: '[appIsValidHexColor]',
    providers: [
        { provide: NG_VALIDATORS, useExisting: HexColorValidatorDirective, multi: true }
    ]
})
export class HexColorValidatorDirective extends BaseValidatorDirective {
    public validatorName: string =
        'appIsValidHexColor';

    public validate(formControl: AbstractControl): { [key: string]: any } {
        const value: string =
            formControl.value;

        if (!isNullOrUndefined(value) && value !== '') {
            const isValidColor: boolean =
                isHexColor(value);

            return this.getValidationResult(isValidColor);
        }

        return this.getInvalidValidateResult();
    }
}