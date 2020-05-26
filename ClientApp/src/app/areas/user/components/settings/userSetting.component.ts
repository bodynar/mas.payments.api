import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';

import { ReplaySubject, Subject } from 'rxjs';
import { takeUntil, tap, switchMap } from 'rxjs/operators';

import { INotificationService } from 'services/INotificationService';
import { IUserService } from 'services/IUserService';

import UpdateUserSettingRequest from 'models/request/user/updateUserSettingRequest';
import GetUserSettingsResponse from 'models/response/user/getUserSettingsResponse';

@Component({
    templateUrl: 'userSetting.template.pug'
})
export default class UserSettingComponent implements OnDestroy, OnInit {

    public userSettings$: Subject<Array<GetUserSettingsResponse>> =
        new ReplaySubject();

    public settingsForm: FormGroup;

    private whenComponentDestroy$: Subject<null> =
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
        this.userSettings$
            .subscribe(settings => this.userSettings = settings);

        this.whenRequestUserSettings$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                switchMap(_ => this.userService.getUserSettings()),
                tap(settings => {
                    const settingsFormArray: FormArray =
                        this.settingsForm.controls['items'] as FormArray;

                    settingsFormArray.controls = [];

                    settings.forEach(x => {
                        settingsFormArray.push(this.formBuilder.group({
                            [x.name]: [x.value]
                        }));
                    });
                }),
            )
            .subscribe(settings => this.userSettings$.next(settings));
    }

    public ngOnInit(): void {
        this.settingsForm = this.formBuilder.group({
            items: this.formBuilder.array([])
        });

        this.whenRequestUserSettings$.next(null);
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
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
            this.userService
                .updateUserSettings(changedSettings)
                .pipe(
                    tap(isOperationSucceeded => {
                        if (!isOperationSucceeded) {
                            this.notificationService.error('Error occured due saving settings.');
                        }
                        else {
                            this.notificationService.success('Settings successfully saved.');
                        }

                        return isOperationSucceeded;
                    }),
                )
                .subscribe(_ => this.whenRequestUserSettings$.next(null));
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