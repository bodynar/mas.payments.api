import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IMeasurementApiBackendService } from 'services/backend/IMeasurementApi.backend';

import { MeasurementsFilter } from 'models/measurementsFilter';
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

    public getMeasurements(filter?: MeasurementsFilter): Observable<Array<MeasurementResponse>> {
        let params: HttpParams =
            new HttpParams();

        if (!isNullOrUndefined(filter)) {
            if (!isNullOrUndefined(filter.month)) {
                params = params.set('month', `${filter.month}`);
            }
            if (!isNullOrUndefined(filter.measurementTypeId)) {
                params = params.set('measurementTypeId', `${filter.measurementTypeId}`);
            }
        }

        const headers: HttpHeaders =
            new HttpHeaders({
                'Content-Type': 'application/json'
            });

        return this.http
            .get(`${this.apiPrefix}/getMeasurements`, { headers, params })
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

    public deleteMeasurementType(measurementTypeId: number): Observable<boolean> {
        return this.http
            .delete(`${this.apiPrefix}/deleteMeasurementType`,
                { params: new HttpParams().set('measurementTypeId', `${measurementTypeId}`) })
            .pipe(catchError(error => of(error)));
    }

    public deleteMeasurement(measurementId: number): Observable<boolean> {
        return this.http
            .delete(`${this.apiPrefix}/deleteMeasurement`,
                { params: new HttpParams().set('measurementId', `${measurementId}`) })
            .pipe(catchError(error => of(error)));
    }
}

export { MeasurementApiBackendService };