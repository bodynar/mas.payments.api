import { Directive, HostListener } from '@angular/core';
import { NG_VALIDATORS, AbstractControl } from '@angular/forms';

import { isNullOrUndefined } from 'common/utils/common';

import { BaseValidatorDirective } from '../baseValidatorDirective';

@Directive({
    selector: '[appIsPositiveNumber]',
    providers: [
        { provide: NG_VALIDATORS, useExisting: PositiveNumberValidatorDirective, multi: false,  }
    ]
})
export class PositiveNumberValidatorDirective extends BaseValidatorDirective {
    public validatorName: string =
        'appIsPositiveNumber';

    public validate(formControl: AbstractControl): { [key: string]: any } {
        const value: any =
            formControl.value;

        const isValidValue: boolean =
            this.isValidValue(value);

        return this.getValidationResult(isValidValue);
    }

    @HostListener('paste', ['$event'])
    public onKeyDown(event: ClipboardEvent): void {
        const stringData: string =
            event.clipboardData.getData('Text');

        const isValidValue: boolean =
            this.isValidValue(stringData);

        if (!isValidValue) {
            event.preventDefault();
        }
    }

    private isValidValue(value: any): boolean {
        if (!isNullOrUndefined(value) && value !== '') {
            const parsedValue: number =
                parseFloat(value);

            return !isNaN(parsedValue) && parsedValue > 0;
        }

        return false;
    }
}