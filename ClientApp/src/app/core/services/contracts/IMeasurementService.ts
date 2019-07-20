import { Observable } from 'rxjs';

import { AddMeasurementRequest } from 'models/request/addMeasurementRequest';
import { AddMeasurementTypeRequest } from 'models/request/addMeasurementTypeRequest';
import { MeasurementResponse } from 'models/response/measurementResponse';
import { MeasurementTypeResponse } from 'models/response/measurementTypeResponse';

abstract class IMeasurementService {
    abstract addMeasurementType(MeasurementTypeData: AddMeasurementTypeRequest): void;

    abstract addMeasurement(MeasurementData: AddMeasurementRequest): void;

    abstract getMeasurementTypes(): Observable<Array<MeasurementTypeResponse>>;

    abstract getMeasurements(): Observable<Array<MeasurementResponse>>;
}

export { IMeasurementService };