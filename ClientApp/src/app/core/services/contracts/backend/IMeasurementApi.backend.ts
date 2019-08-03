import { Observable } from 'rxjs';

import { MeasurementsFilter } from 'models/measurementsFilter';
import { AddMeasurementRequest } from 'models/request/addMeasurementRequest';
import { AddMeasurementTypeRequest } from 'models/request/addMeasurementTypeRequest';
import { MeasurementResponse } from 'models/response/measurementResponse';
import { MeasurementTypeResponse } from 'models/response/measurementTypeResponse';

abstract class IMeasurementApiBackendService {
    abstract addMeasurementType(MeasurementTypeData: AddMeasurementTypeRequest): Observable<any>;

    abstract addMeasurement(MeasurementData: AddMeasurementRequest): Observable<any>;

    abstract getMeasurementTypes(): Observable<Array<MeasurementTypeResponse>>;

    abstract getMeasurements(filter?: MeasurementsFilter): Observable<Array<MeasurementResponse>>;

    abstract deleteMeasurementType(measurementTypeId: number): Observable<boolean>;

    abstract deleteMeasurement(measurementId: number): Observable<boolean>;
}

export { IMeasurementApiBackendService };