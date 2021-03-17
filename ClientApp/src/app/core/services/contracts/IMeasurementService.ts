import { Observable } from 'rxjs';

import CommandExecutionResult from 'models/response/commandExecutionResult';
import QueryExecutionResult from 'models/response/queryExecutionResult';

import { AddMeasurementRequest, UpdateMeasurementRequest, MeasurementFilter, AddMeasurementTypeRequest } from 'models/request/measurement';

import { MeasurementResponse, MeasurementsResponse, MeasurementTypeResponse } from 'models/response/measurements';

abstract class IMeasurementService {
    // #region measurements
    abstract addMeasurement(measurementData: AddMeasurementRequest): Observable<CommandExecutionResult>;

    abstract getMeasurements(filter?: MeasurementFilter): Observable<QueryExecutionResult<Array<MeasurementsResponse>>>;

    abstract getMeasurement(id: number): Observable<QueryExecutionResult<MeasurementResponse>>;

    abstract updateMeasurement(id: number, measurementData: UpdateMeasurementRequest): Observable<CommandExecutionResult>;

    abstract deleteMeasurement(measurementId: number): Observable<CommandExecutionResult>;

    abstract sendMeasurements(measurementIds: Array<number>): Observable<CommandExecutionResult>;

    abstract getMeasurementsWithoutDiffCount(): Observable<QueryExecutionResult<number>>;

    abstract updateDiff(): Observable<QueryExecutionResult<string>>;

    // #endregion measurements

    // #region measurement types

    abstract addMeasurementType(measurementTypeData: AddMeasurementTypeRequest): Observable<CommandExecutionResult>;

    abstract getMeasurementTypes(): Observable<QueryExecutionResult<Array<MeasurementTypeResponse>>>;

    abstract getMeasurementType(id: number): Observable<QueryExecutionResult<MeasurementTypeResponse>>;

    abstract updateMeasurementType(id: number, measurementTypeData: AddMeasurementTypeRequest): Observable<CommandExecutionResult>;

    abstract deleteMeasurementType(measurementTypeId: number): Observable<CommandExecutionResult>;

    // #endregion measurement types
}

export { IMeasurementService };