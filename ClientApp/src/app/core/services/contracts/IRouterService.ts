import { Observable } from 'rxjs';

abstract class IRouterService {
    abstract whenRouteChange(): Observable<Array<string>>;

    abstract getAreaName(): string;

    abstract getCurrentRoute(withoutArgs?: boolean): string;

    abstract getPreviousRoute(): string;

    abstract navigate(path: Array<string>, extras?: any): void;

    abstract navigateUp(): void;

    abstract navigateDeep(routeDefinition: Array<string>, extras?: any): void;

    abstract navigateArea(path: Array<string>, extras?: any): void;
}

export { IRouterService };