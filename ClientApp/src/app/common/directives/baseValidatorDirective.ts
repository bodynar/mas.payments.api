import { SimpleChanges, OnChanges, Directive } from '@angular/core';
import { Validator, AbstractControl } from '@angular/forms';

@Directive({})
export abstract class BaseValidatorDirective implements Validator, OnChanges {
    abstract validatorName: string;

    private onChange: () => void;

    abstract validate(formControl: AbstractControl): { [key: string]: any };

    public registerOnValidatorChange(fn: () => void): void {
        this.onChange = fn;
    }

    public ngOnChanges(changes: SimpleChanges): void {
        if (this.validatorName in changes && this.onChange) {
            this.onChange();
        }
    }

    public getInvalidValidateResult(): { [key: string]: any } {
        return { [this.validatorName.toString()]: { valid: false } };
    }

    public getValidationResult(condition: boolean): { [key: string]: any } {
        return condition ? this.getInvalidValidateResult() : null;
    }
}