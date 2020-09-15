import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';

import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';
import { delay, filter, switchMap, takeUntil, tap } from 'rxjs/operators';

import BaseComponent from 'common/components/BaseComponent';

import { INotificationService } from 'services/INotificationService';
import { IUserService } from 'services/IUserService';

import UpdateUserSettingRequest from 'models/request/user/updateUserSettingRequest';
import GetUserSettingsResponse from 'models/response/user/getUserSettingsResponse';

@Component({
    templateUrl: 'userSetting.template.pug'
})
export class UserSettingComponent extends BaseComponent {
    public isLoading$: Subject<boolean> =
        new BehaviorSubject(true);

    public userSettings$: Subject<Array<GetUserSettingsResponse>> =
        new ReplaySubject();

    public settingsForm: FormGroup;

    private whenSaveSettings$: Subject<Array<UpdateUserSettingRequest>> =
        new Subject();

    private whenRequestUserSettings$: Subject<null> =
        new Subject();

    private userSettings: Array<GetUserSettingsResponse> =
        [];

    constructor(
        private formBuilder: FormBuilder,
        private userService: IUserService,
        private notificationService: INotificationService,
    ) {
        super();

        this.whenComponentInit$
            .subscribe(() => {
                this.settingsForm = this.formBuilder.group({
                    items: this.formBuilder.array([])
                });

                this.whenRequestUserSettings$.next(null);
            });

        this.userSettings$
            .subscribe(settings => this.userSettings = settings);

        this.whenRequestUserSettings$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                switchMap(_ => {
                    this.isLoading$.next(true);
                    return this.userService.getUserSettings();
                }),
                delay(1.5 * 1000),
                filter(response => {
                    this.isLoading$.next(false);

                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
                tap(({ result }) => {
                    const settingsFormArray: FormArray =
                        this.settingsForm.controls['items'] as FormArray;

                    settingsFormArray.controls = [];

                    result.forEach(x => {
                        settingsFormArray.push(this.formBuilder.group({
                            [x.name]: [x.value]
                        }));
                    });
                }),
            )
            .subscribe(({ result }) => this.userSettings$.next(result));

        this.whenSaveSettings$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                switchMap(changedSettings => {
                    this.isLoading$.next(true);
                    return this.userService.updateUserSettings(changedSettings);
                }),
                delay(1.5 * 1000),
                tap(response => {
                    this.isLoading$.next(false);

                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    else {
                        this.notificationService.success('Settings successfully saved.');
                    }

                    return response.success;
                }),
            )
            .subscribe(_ => this.whenRequestUserSettings$.next(null));
    }

    public onFormSubmit(): void {
        const settingsFormArray: Array<{
            name: string;
            value: string;
        }> =
            [...this.settingsForm.value['items']]
                .map(item => ({
                    ...item,
                    formControlName: Object.keys(item)[0],
                }))
                .map(item => ({
                    name: item.formControlName,
                    value: item[item.formControlName],
                }));

        const changedSettings: Array<UpdateUserSettingRequest> =
            this.getChangedSettings(this.userSettings, settingsFormArray);

        if (changedSettings.length === 0) {
            this.notificationService.warning('You haven\'t changed any settings');
        } else {
            this.whenSaveSettings$.next(changedSettings);
        }
    }

    private getChangedSettings(
        userSettings: Array<GetUserSettingsResponse>,
        settingsFormArray: Array<{
            name: string;
            value: string;
        }>
    ): Array<UpdateUserSettingRequest> {
        return userSettings
            .map(setting => ({
                setting: { ...setting },
                formValue: { ...settingsFormArray.filter(formItem => formItem.name === setting.name).pop() },
            }))
            .map(({ setting, formValue }) => {
                if (setting.typeName === 'Boolean') {
                    return {
                        setting: { ...setting },
                        formValue: {
                            name: formValue.name,
                            value: formValue.value.toString()
                        }
                    };
                } else if (setting.typeName === 'Number') {
                    return {
                        setting: { ...setting },
                        formValue: {
                            name: formValue.name,
                            value: formValue.value.toString(),
                        }
                    };
                }
                return {
                    setting: { ...setting },
                    formValue: { ...formValue }
                };
            })
            .filter(({ setting, formValue }) => formValue.value !== setting.rawValue)
            .map(({ setting, formValue }) => ({
                id: setting.id,
                rawValue: formValue.value
            }));
    }
}