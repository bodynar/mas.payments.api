import { Observable } from 'rxjs';

import { MeasurementsFilter } from 'models/measurementsFilter';
import { AddMeasurementRequest } from 'models/request/addMeasurementRequest';
import { AddMeasurementTypeRequest } from 'models/request/addMeasurementTypeRequest';
import MeasurementResponse from 'models/response/measurements/measurementResponse';
import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';

abstract class IMeasurementApiBackendService {
    // #region measurements
    abstract addMeasurement(measurementData: AddMeasurementRequest): Observable<any>;

    abstract getMeasurements(filter?: MeasurementsFilter): Observable<Array<MeasurementResponse>>;

    abstract getMeasurement(id: number): Observable<MeasurementResponse>;

    abstract updateMeasurement(id: number, measurementData: AddMeasurementRequest): Observable<any>;

    abstract deleteMeasurement(measurementId: number): Observable<boolean>;

    abstract sendMeasurements(measurementIds: Array<number>): Observable<boolean>;

    // #endregion measurements

    // #region measurement types

    abstract addMeasurementType(measurementTypeData: AddMeasurementTypeRequest): Observable<any>;

    abstract getMeasurementTypes(): Observable<Array<MeasurementTypeResponse>>;

    abstract getMeasurementType(id: number): Observable<MeasurementTypeResponse>;

    abstract updateMeasurementType(id: number, measurementTypeData: AddMeasurementTypeRequest): Observable<any>;

    abstract deleteMeasurementType(measurementTypeId: number): Observable<boolean>;

    // #endregion measurement types
}

export { IMeasurementApiBackendService };