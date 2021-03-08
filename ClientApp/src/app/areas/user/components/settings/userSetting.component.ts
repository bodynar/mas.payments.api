import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';

import { Subject, Subscription } from 'rxjs';
import { delay, filter, switchMap, takeUntil, tap } from 'rxjs/operators';

import BaseComponent from 'common/components/BaseComponent';

import { INotificationService } from 'services/INotificationService';
import { IUserService } from 'services/IUserService';
import { IMeasurementService } from 'services/IMeasurementService';

import { UpdateUserSettingRequest } from 'models/request/user';
import { GetUserSettingsResponse } from 'models/response/user';

type UserSettingPageMode = 'loading_settings' | 'loading_businessProccesses' | 'loaded';

@Component({
    templateUrl: 'userSetting.template.pug'
})
export class UserSettingComponent extends BaseComponent {
    public userSettings: Array<GetUserSettingsResponse> = [];

    public isSettingsLoading: boolean = false;
    public isDiffLoading: boolean = false;
    public measurementsWithoutDiff: number = 0;

    public settingsForm: FormGroup;

    private whenSaveSettings$: Subject<Array<UpdateUserSettingRequest>> =
        new Subject();

    private whenRequestUserSettings$: Subject<null> =
        new Subject();

    constructor(
        private formBuilder: FormBuilder,
        private userService: IUserService,
        private notificationService: INotificationService,
        private measurementService: IMeasurementService,
    ) {
        super();

        this.whenComponentInit$
            .subscribe(() => {
                this.settingsForm = this.formBuilder.group({
                    items: this.formBuilder.array([])
                });

                this.whenRequestUserSettings$.next(null);
                this.loadMeasurementsWithoutDiff();
            });

        this.whenRequestUserSettings$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                switchMap(_ => {
                    this.isSettingsLoading = true;
                    return this.userService.getUserSettings();
                }),
                delay(1.5 * 1000),
                filter(response => {
                    this.isSettingsLoading = false;

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
            .subscribe(({ result }) => this.userSettings = result);

        this.whenSaveSettings$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                switchMap(changedSettings => {
                    this.isSettingsLoading = true;
                    return this.userService.updateUserSettings(changedSettings);
                }),
                delay(1.5 * 1000),
                tap(response => {
                    this.isSettingsLoading = false;

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

    public onBusinessProcessExecute(): void {
        const subscription: Subscription =
            this.measurementService.updateDiff()
                .pipe(
                    takeUntil(this.whenComponentDestroy$),
                    tap(_ => this.isDiffLoading = true),
                    filter(result => {
                        if (!result.success) {
                            this.notificationService.error(result.error);
                            this.isDiffLoading = false;
                        }

                        return result.success;
                    })
                )
                .subscribe(_ => {
                    this.loadMeasurementsWithoutDiff();
                    subscription.unsubscribe();
                });
    }

    private loadMeasurementsWithoutDiff(): void {
        const subscription: Subscription =
            this.measurementService.getMeasurementsWithoutDiffCount()
                .pipe(
                    takeUntil(this.whenComponentDestroy$),
                    tap(_ => this.isDiffLoading = true)
                )
                .subscribe(({ result }) => {
                    this.isDiffLoading = false;
                    this.measurementsWithoutDiff = result;
                    subscription.unsubscribe();
                });
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