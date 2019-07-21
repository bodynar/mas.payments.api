import { Observable } from 'rxjs';

abstract class IRouterService {
    abstract whenRouteChange(): Observable<Array<string>>;

    abstract getCurrentRoute(withoutArgs?: boolean): string;

    abstract getPreviousRoute(): string;

    abstract navigate(path: Array<string>, extras?: any): void;
}

export { IRouterService };