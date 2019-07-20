import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IMeasurementApiBackendService } from 'services/IMeasurementApi.backend';
import { IMeasurementService } from 'services/IMeasurementService';

import { AddMeasurementRequest } from 'models/request/addMeasurementRequest';
import { AddMeasurementTypeRequest } from 'models/request/addMeasurementTypeRequest';
import { MeasurementResponse } from 'models/response/MeasurementResponse';
import { MeasurementTypeResponse } from 'models/response/MeasurementTypeResponse';

@Injectable()
class MeasurementService implements IMeasurementService {

    constructor(
        private MeasurementApiBackend: IMeasurementApiBackendService,
        // private notificationService: INotificationService,
        // private loggingService: ILoggingService
    ) { }

    public addMeasurementType(MeasurementTypeData: AddMeasurementTypeRequest): void {
        // data validation

        this.MeasurementApiBackend
            .addMeasurementType(MeasurementTypeData)
            .pipe(
                tap(response => {
                    const hasError: boolean =
                        !isNullOrUndefined(response);

                    if (hasError) {
                        // this.notificationService.error();
                        // this.loggingService.error()
                    }
                }),
                map(response => !isNullOrUndefined(response))
            );
    }

    public addMeasurement(MeasurementData: AddMeasurementRequest): void {
        // data validation

        this.MeasurementApiBackend
            .addMeasurement(MeasurementData)
            .pipe(
                tap(response => {
                    const hasError: boolean =
                        !isNullOrUndefined(response);

                    if (hasError) {
                        // this.notificationService.error();
                        // this.loggingService.error()
                    }
                }),
                map(response => !isNullOrUndefined(response))
            );
    }

    public getMeasurementTypes(): Observable<Array<MeasurementTypeResponse>> {
        return this.MeasurementApiBackend
            .getMeasurementTypes()
            .pipe(
                map(response => {
                    const hasError: boolean =
                        isNullOrUndefined(response) || !(response instanceof Array);

                    if (hasError) {
                        // this.notificationService.error();
                        // this.loggingService.error(response);
                    }

                    return hasError ? [] : response;
                }),
            );
    }

    public getMeasurements(): Observable<Array<MeasurementResponse>> {
        return this.MeasurementApiBackend
            .getMeasurements()
            .pipe(
                map(response => {
                    const hasError: boolean =
                        isNullOrUndefined(response) || !(response instanceof Array);

                    if (hasError) {
                        // this.notificationService.error();
                        // this.loggingService.error(response);
                    }

                    return hasError ? [] : response;
                }),
            );
    }
}

export { MeasurementService };