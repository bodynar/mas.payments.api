import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IMeasurementApiBackendService } from 'services/backend/IMeasurementApi.backend';
import { IMeasurementService } from 'services/IMeasurementService';

import { AddMeasurementRequest } from 'models/request/addMeasurementRequest';
import { AddMeasurementTypeRequest } from 'models/request/addMeasurementTypeRequest';
import { MeasurementResponse } from 'models/response/measurementResponse';
import { MeasurementTypeResponse } from 'models/response/measurementTypeResponse';

@Injectable()
class MeasurementService implements IMeasurementService {

    constructor(
        private measurementApiBackend: IMeasurementApiBackendService,
        // private notificationService: INotificationService,
        // private loggingService: ILoggingService
    ) { }

    public addMeasurementType(measurementTypeData: AddMeasurementTypeRequest): Observable<boolean> {
        // data validation

        return this.measurementApiBackend
            .addMeasurementType(measurementTypeData)
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

    public addMeasurement(measurementData: AddMeasurementRequest): Observable<boolean> {
        // data validation

        return this.measurementApiBackend
            .addMeasurement(measurementData)
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
        return this.measurementApiBackend
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
        return this.measurementApiBackend
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