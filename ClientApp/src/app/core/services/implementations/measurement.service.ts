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
        measurementData.date.month = +measurementData.date.month  + 1;

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
                map(x => {
                    if (x.success) {
                        x.result = x.result.map(item => ({
                            ...item,
                            month: item.month - 1,
                            measurements: item.measurements.map(m => ({ ...m, month: +m.month - 1 }))
                        }));
                    }

                    return x;
                }),
            );
    }

    public getMeasurement(id: number): Observable<QueryExecutionResult<MeasurementResponse>> {
        return this.measurementApiBackend.getMeasurement(id)
            .pipe(
                tap(response => {
                    if (!response.success) {
                        // this.loggingService.error(response);
                    }
                }),
                map(response => {
                    if (response.success) {
                        response.result.date.month = response.result.date.month - 1;
                    }

                    return response;
                })
            );
    }

    public updateMeasurement(id: number, measurementData: UpdateMeasurementRequest): Observable<CommandExecutionResult> {
        measurementData.date.month = measurementData.date.month + 1;

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

    public getMeasurementsWithoutDiffCount(): Observable<QueryExecutionResult<number>> {
        return this.measurementApiBackend.getMeasurementsWithoutDiffCount();
    }

    public updateDiff(): Observable<QueryExecutionResult<string>> {
        return this.measurementApiBackend.updateDiff();
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