import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IMeasurementApiBackendService } from 'services/backend/IMeasurementApi.backend';

import { MeasurementsFilter } from 'models/measurementsFilter';
import { AddMeasurementRequest } from 'models/request/addMeasurementRequest';
import { AddMeasurementTypeRequest } from 'models/request/addMeasurementTypeRequest';
import MeasurementResponse from 'models/response/measurements/measurementResponse';
import MeasurementsResponse from 'models/response/measurements/measurementsResponse';
import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';

@Injectable()
class MeasurementApiBackendService implements IMeasurementApiBackendService {

    private readonly apiPrefix: string =
        '/api/measurement';

    constructor(
        private http: HttpClient
    ) {
    }

    // #region measurements

    public addMeasurement(measurementData: AddMeasurementRequest): Observable<any> {
        return this.http
            .post(`${this.apiPrefix}/addMeasurement`, measurementData)
            .pipe(catchError(error => of(error)));
    }

    public getMeasurements(filter?: MeasurementsFilter): Observable<Array<MeasurementsResponse>> {
        let params: HttpParams =
            new HttpParams();

        if (!isNullOrUndefined(filter)) {
            if (!isNullOrUndefined(filter.month)) {
                params = params.set('month', `${filter.month + 1}`);
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
                        year: measurement['dateYear'],
                        month: measurement['dateMonth'],
                        measurements: measurement['measurements'].map(x => ({
                            id: x['id'],
                            measurement: x['measurement'],
                            comment: x['comment'],
                            meterMeasurementTypeId: x['meterMeasurementTypeId'],
                            measurementTypeName: x['measurementTypeName'],
                            isSent: x['isSent'],
                        })),
                    }) as MeasurementsResponse)),
                catchError(error => of(error))
            );
    }

    public getMeasurement(id: number): Observable<MeasurementResponse> {
        return this.http
            .get(`${this.apiPrefix}/getMeasurement`, {
                params: new HttpParams({
                    fromObject: { id: `${id}` }
                })
            })
            .pipe(
                map((response: any) =>
                    ({
                        id: response['id'],
                        measurement: response['measurement'],
                        comment: response['comment'],
                        month: response['dateMonth'],
                        year: response['dateYear'],
                        meterMeasurementTypeId: response['meterMeasurementTypeId'],
                        measurementTypeName: response['measurementTypeName'],
                    }) as MeasurementResponse),
                catchError(error => of(error))
            );
    }

    public updateMeasurement(id: number, measurementData: AddMeasurementRequest): Observable<any> {
        return this.http
            .post(`${this.apiPrefix}/updateMeasurement`, { id, ...measurementData })
            .pipe(catchError(error => of(error)));
    }

    public deleteMeasurement(measurementId: number): Observable<boolean> {
        return this.http
            .delete(`${this.apiPrefix}/deleteMeasurement`,
                { params: new HttpParams().set('measurementId', `${measurementId}`) })
            .pipe(catchError(error => of(error)));
    }

    public sendMeasurements(measurementIds: Array<number>): Observable<boolean> {
        return this.http
            .post(`${this.apiPrefix}/sendMeasurements`, measurementIds)
            .pipe(catchError(error => of(error)));
    }

    // #endregion measurements

    // #region measurement types

    public addMeasurementType(measurementTypeData: AddMeasurementTypeRequest): Observable<any> {
        return this.http
            .post(`${this.apiPrefix}/addMeasurementType`, measurementTypeData)
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
                        systemName: measurementType['systemName'],
                        description: measurementType['description'],
                        paymentTypeId: measurementType['paymentTypeId'],
                        paymentTypeName: measurementType['paymentTypeName']
                    }) as MeasurementTypeResponse)),
                catchError(error => of(error))
            );
    }

    public getMeasurementType(id: number): Observable<MeasurementTypeResponse> {
        return this.http
            .get(`${this.apiPrefix}/getMeasurementType`, {
                params: new HttpParams({
                    fromObject: { id: `${id}` }
                })
            })
            .pipe(
                map((response: any) =>
                    ({
                        id: response['id'],
                        name: response['name'],
                        systemName: response['systemName'],
                        description: response['description'],
                        paymentTypeId: response['paymentTypeId'],
                        paymentTypeName: response['paymentTypeName'],
                    }) as MeasurementTypeResponse),
                catchError(error => of(error))
            );
    }

    public updateMeasurementType(id: number, measurementTypeData: AddMeasurementTypeRequest): Observable<any> {
        return this.http
            .post(`${this.apiPrefix}/updateMeasurementType`, { id, ...measurementTypeData })
            .pipe(catchError(error => of(error)));
    }

    public deleteMeasurementType(measurementTypeId: number): Observable<boolean> {
        return this.http
            .delete(`${this.apiPrefix}/deleteMeasurementType`,
                { params: new HttpParams().set('measurementTypeId', `${measurementTypeId}`) })
            .pipe(catchError(error => of(error)));
    }

    // #endregion measurement types
}

export { MeasurementApiBackendService };