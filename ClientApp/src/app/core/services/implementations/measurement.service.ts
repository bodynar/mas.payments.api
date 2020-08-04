import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'common/utils/common';

import { IMeasurementApiBackendService } from 'services/backend/IMeasurementApi.backend';
import { IMeasurementService } from 'services/IMeasurementService';

import MeasurementsFilter from 'models/measurementsFilter';
import { AddMeasurementRequest } from 'models/request/measurement/addMeasurementRequest';
import { AddMeasurementTypeRequest } from 'models/request/measurement/addMeasurementTypeRequest';
import CommandExecutionResult from 'models/response/commandExecutionResult';
import MeasurementResponse from 'models/response/measurements/measurementResponse';
import MeasurementsResponse from 'models/response/measurements/measurementsResponse';
import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';
import QueryExecutionResult from 'models/response/queryExecutionResult';

@Injectable()
export class MeasurementService implements IMeasurementService {

    constructor(
        private measurementApiBackend: IMeasurementApiBackendService,
        // private loggingService: ILoggingService
    ) { }

    // #region measurements

    public addMeasurement(measurementData: AddMeasurementRequest): Observable<CommandExecutionResult> {
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
                tap(withoutError => {
                    if (!withoutError) {
                        // this.loggingService.error()
                    }
                }),
            );
    }

    public getMeasurements(filter?: MeasurementsFilter): Observable<QueryExecutionResult<Array<MeasurementsResponse>>> {
        return this.measurementApiBackend
            .getMeasurements(filter)
            .pipe(
                tap(response => {
                    if (!response.success) {
                        // this.loggingService.error(response);
                    }
                }),
            );
    }

    public getMeasurement(id: number): Observable<QueryExecutionResult<MeasurementResponse>> {
        return this.measurementApiBackend.getMeasurement(id);
    }

    public updateMeasurement(id: number, measurementData: AddMeasurementRequest): Observable<CommandExecutionResult> {
        return this.measurementApiBackend
            .updateMeasurement(id, measurementData)
            .pipe(
                tap(withoutError => {
                    if (!withoutError) {
                        // this.loggingService.error()
                    }
                }),
            );
    }

    public deleteMeasurement(measurementId: number): Observable<CommandExecutionResult> {
        return this.measurementApiBackend
            .deleteMeasurement(measurementId);
    }

    public sendMeasurements(measurementIds: Array<number>): Observable<CommandExecutionResult> {
        return this.measurementApiBackend
            .sendMeasurements(measurementIds);
    }

    // #endregion measurements

    // #region measurement types

    public addMeasurementType(measurementTypeData: AddMeasurementTypeRequest): Observable<CommandExecutionResult> {
        // data validation

        return this.measurementApiBackend
            .addMeasurementType(measurementTypeData)
            .pipe(
                tap(withoutError => {
                    if (!withoutError) {
                        // this.loggingService.error()
                    }
                }),
            );
    }

    public getMeasurementTypes(): Observable<QueryExecutionResult<Array<MeasurementTypeResponse>>> {
        return this.measurementApiBackend
            .getMeasurementTypes()
            .pipe(
                tap(response => {
                    if (!response.success) {
                        // this.loggingService.error(response);
                    }
                }),
            );
    }

    public getMeasurementType(id: number): Observable<QueryExecutionResult<MeasurementTypeResponse>> {
        return this.measurementApiBackend.getMeasurementType(id);
    }

    public updateMeasurementType(id: number, measurementTypeData: AddMeasurementTypeRequest): Observable<CommandExecutionResult> {
        return this.measurementApiBackend
            .updateMeasurementType(id, measurementTypeData)
            .pipe(
                tap(withoutError => {
                    if (!withoutError) {
                        // this.loggingService.error()
                    }
                }),
            );
    }

    public deleteMeasurementType(measurementTypeId: number): Observable<CommandExecutionResult> {
        return this.measurementApiBackend
            .deleteMeasurementType(measurementTypeId);
    }

    // #endregion measurement types
}