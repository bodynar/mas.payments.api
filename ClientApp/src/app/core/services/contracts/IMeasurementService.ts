import { Observable } from 'rxjs';

import { MeasurementsFilter } from 'models/measurementsFilter';
import { AddMeasurementRequest } from 'models/request/addMeasurementRequest';
import { AddMeasurementTypeRequest } from 'models/request/addMeasurementTypeRequest';
import { MeasurementResponse } from 'models/response/measurementResponse';
import { MeasurementTypeResponse } from 'models/response/measurementTypeResponse';

abstract class IMeasurementService {
    abstract addMeasurementType(MeasurementTypeData: AddMeasurementTypeRequest): Observable<boolean>;

    abstract addMeasurement(MeasurementData: AddMeasurementRequest): Observable<boolean>;

    abstract getMeasurementTypes(): Observable<Array<MeasurementTypeResponse>>;

    abstract getMeasurements(filter?: MeasurementsFilter): Observable<Array<MeasurementResponse>>;
}

export { IMeasurementService };