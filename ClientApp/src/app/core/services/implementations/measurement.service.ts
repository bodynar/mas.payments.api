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
class MeasurementService implements IMeasurementService {

    constructor(
        private measurementApiBackend: IMeasurementApiBackendService,
        // private notificationService: INotificationService,
        // private loggingService: ILoggingService
    ) { }

    public getMeasurement(id: number): Observable<MeasurementResponse> {
        return this.measurementApiBackend.getMeasurement(id);
    }

    public getMeasurementType(id: number): Observable<MeasurementTypeResponse> {
        return this.measurementApiBackend.getMeasurementType(id);
    }

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

    public addMeasurement(measurementData: AddMeasurementRequest): Observable<boolean> {
        // data validation

        return this.measurementApiBackend
            .addMeasurement(measurementData)
            .pipe(
                map(response => isNullOrUndefined(response)),
                tap(withoutError => {
                    if (!withoutError) {
                        // this.loggingService.error()
                    }
                }),
            );
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

    public deleteMeasurementType(measurementTypeId: number): Observable<boolean> {
        return this.measurementApiBackend
            .deleteMeasurementType(measurementTypeId)
            .pipe(map(response => !isNullOrUndefined(response)));
    }

    public deleteMeasurement(measurementId: number): Observable<boolean> {
        return this.measurementApiBackend
            .deleteMeasurement(measurementId)
            .pipe(map(response => !isNullOrUndefined(response)));
    }
}

export { MeasurementService };