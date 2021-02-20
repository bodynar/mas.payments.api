import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'common/utils/common';

import { IMeasurementApiBackendService } from 'services/backend/IMeasurementApi.backend';
import { IMeasurementService } from 'services/IMeasurementService';

import CommandExecutionResult from 'models/response/commandExecutionResult';
import QueryExecutionResult from 'models/response/queryExecutionResult';

import { AddMeasurementRequest, UpdateMeasurementRequest, MeasurementsFilter, AddMeasurementTypeRequest } from 'models/request/measurement';

import { MeasurementResponse, MeasurementsResponse, MeasurementTypeResponse } from 'models/response/measurements';

@Injectable()
export class MeasurementService implements IMeasurementService {

    constructor(
        private measurementApiBackend: IMeasurementApiBackendService,
        // private loggingService: ILoggingService
    ) { }

    // #region measurements

    public addMeasurement(measurementData: AddMeasurementRequest): Observable<CommandExecutionResult> {
        return this.measurementApiBackend
            .addMeasurement(measurementData)
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

    public updateMeasurement(id: number, measurementData: UpdateMeasurementRequest): Observable<CommandExecutionResult> {
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
                map(response =>
                    response.success
                        ? ({
                            ...response,
                            result: response.result.map(item => ({ ...item, hasColor: !isNullOrUndefined(item.color)}))
                        })
                        : response
                )
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