import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { IMeasurementApiBackendService } from 'services/backend/IMeasurementApi.backend';

import { AddMeasurementRequest } from 'models/request/addMeasurementRequest';
import { AddMeasurementTypeRequest } from 'models/request/addMeasurementTypeRequest';
import { MeasurementResponse } from 'models/response/measurementResponse';
import { MeasurementTypeResponse } from 'models/response/measurementTypeResponse';

@Injectable()
class MeasurementApiBackendService implements IMeasurementApiBackendService {

    private readonly apiPrefix: string =
        '/api/measures';

    constructor(
        private http: HttpClient
    ) {
    }

    public addMeasurementType(MeasurementTypeData: AddMeasurementTypeRequest): Observable<any> {
        return this.http
            .post(`${this.apiPrefix}/addMeasurementType`, MeasurementTypeData)
            .pipe(catchError(error => of(error)));
    }

    public addMeasurement(MeasurementData: AddMeasurementRequest): Observable<any> {
        return this.http
            .post(`${this.apiPrefix}/addMeasurement`, MeasurementData)
            .pipe(catchError(error => of(error)));
    }

    public getMeasurementTypes(): Observable<Array<MeasurementTypeResponse>> {
        return this.http
            .get(`${this.apiPrefix}/getMeasurementTypes`)
            .pipe(
                map((response: Array<any>) =>
                    response.map(measurementType => ({
                        id: measurementType['id'],
                        name: measurementType['name'],
                        description: measurementType['description'],
                        paymentTypeId: measurementType['paymentTypeId'],
                        paymentTypeName: measurementType['paymentTypeName']
                    }) as MeasurementTypeResponse)),
                catchError(error => of(error))
            );
    }

    public getMeasurements(): Observable<Array<MeasurementResponse>> {
        return this.http
            .get(`${this.apiPrefix}/getMeasurements`)
            .pipe(
                map((response: Array<any>) =>
                    response.map(measurement => ({
                        id: measurement['id'],
                        measurement: measurement['measurement'],
                        comment: measurement['comment'],
                        date: measurement['date'],
                        meterMeasurementTypeId: measurement['meterMeasurementTypeId'],
                        measurementTypeName: measurement['measurementTypeName']
                    }) as MeasurementResponse)),
                catchError(error => of(error))
            );
    }
}

export { MeasurementApiBackendService };