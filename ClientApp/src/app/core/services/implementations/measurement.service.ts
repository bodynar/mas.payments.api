import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IMeasurementApiBackendService } from 'services/backend/IMeasurementApi.backend';
import { IMeasurementService } from 'services/IMeasurementService';

import { MeasurementsFilter } from 'models/measurementsFilter';
import { AddMeasurementRequest } from 'models/request/addMeasurementRequest';
import { AddMeasurementTypeRequest } from 'models/request/addMeasurementTypeRequest';
import { MeasurementResponse } from 'models/response/measurementResponse';
import { MeasurementTypeResponse } from 'models/response/measurementTypeResponse';

@Injectable()
export default class MeasurementService implements IMeasurementService {

    constructor(
        private measurementApiBackend: IMeasurementApiBackendService,
        // private notificationService: INotificationService,
        // private loggingService: ILoggingService
    ) { }

    // #region measurements

    public addMeasurement(measurementData: AddMeasurementRequest): Observable<boolean> {
        // data validation
        const parsedMonth: number =
            parseInt(measurementData.month) + 1;
        const month: number =
            parsedMonth > 12
                ? parsedMonth % 12
                : parsedMonth;
        return this.measurementApiBackend
            .addMeasurement({
                ...measurementData,
                month: month.toString()
            })
            .pipe(
                map(response => isNullOrUndefined(response)),
                tap(withoutError => {
                    if (!withoutError) {
                        // this.loggingService.error()
                    }
                }),
            );
    }

    public getMeasurements(filter?: MeasurementsFilter): Observable<Array<MeasurementResponse>> {
        return this.measurementApiBackend
            .getMeasurements(filter)
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

    public getMeasurement(id: number): Observable<MeasurementResponse> {
        return this.measurementApiBackend.getMeasurement(id);
    }

    public updateMeasurement(id: number, measurementData: AddMeasurementRequest): Observable<boolean> {
        return this.measurementApiBackend
            .updateMeasurement(id, measurementData)
            .pipe(
                map(response => isNullOrUndefined(response)),
                tap(withoutError => {
                    if (!withoutError) {
                        // this.loggingService.error()
                    }
                }),
            );
    }

    public deleteMeasurement(measurementId: number): Observable<boolean> {
        return this.measurementApiBackend
            .deleteMeasurement(measurementId)
            .pipe(map(response => !isNullOrUndefined(response)));
    }

    public sendMeasurements(measurementIds: Array<number>): Observable<boolean> {
        return this.measurementApiBackend
            .sendMeasurements(measurementIds)
            .pipe(map(response => !isNullOrUndefined(response)));
    }

    // #endregion measurements

    // #region measurement types

    public addMeasurementType(measurementTypeData: AddMeasurementTypeRequest): Observable<boolean> {
        // data validation

        return this.measurementApiBackend
            .addMeasurementType(measurementTypeData)
            .pipe(
                map(response => isNullOrUndefined(response)),
                tap(withoutError => {
                    if (!withoutError) {
                        // this.loggingService.error()
                    }
                }),
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

    public getMeasurementType(id: number): Observable<MeasurementTypeResponse> {
        return this.measurementApiBackend.getMeasurementType(id);
    }

    public updateMeasurementType(id: number, measurementTypeData: AddMeasurementTypeRequest): Observable<boolean> {
        return this.measurementApiBackend
            .updateMeasurementType(id, measurementTypeData)
            .pipe(
                map(response => isNullOrUndefined(response)),
                tap(withoutError => {
                    if (!withoutError) {
                        // this.loggingService.error()
                    }
                }),
            );
    }

    public deleteMeasurementType(measurementTypeId: number): Observable<boolean> {
        return this.measurementApiBackend
            .deleteMeasurementType(measurementTypeId)
            .pipe(map(response => !isNullOrUndefined(response)));
    }

    // #endregion measurement types
}