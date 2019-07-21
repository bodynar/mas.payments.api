import { Injectable } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';

import { Observable, ReplaySubject, Subject } from 'rxjs';
import { filter, map, pairwise } from 'rxjs/operators';

import { IRouterService } from 'services/IRouterService.ts';

@Injectable()
class RouterService implements IRouterService {

    private previousRoute: string;

    private whenNavigationEnds$: Subject<NavigationEnd> =
        new ReplaySubject(1);

    constructor(
        private router: Router,
    ) {
        this.router
            .events
            .pipe(
                filter(event => event instanceof NavigationEnd),
                map(event => event as NavigationEnd)
            )
            .subscribe(event => this.whenNavigationEnds$.next(event));

        this.whenNavigationEnds$
            .pipe(
                pairwise(),
                map(([{ urlAfterRedirects }]) => urlAfterRedirects)
            )
            .subscribe(route => this.previousRoute = route);
    }

    public whenRouteChange(): Observable<Array<string>> {
        return this.whenNavigationEnds$
            .pipe(
                map(({ urlAfterRedirects }) =>
                    urlAfterRedirects
                        .split('/')
                        .filter(routerPath => routerPath !== '')
                )
            );
    }

    public getCurrentRoute(withoutArgs: boolean = false): string {
        const currentRoute: string =
            this.router.url.substring(1);

        if (!withoutArgs) {
            return currentRoute;
        } else {
            return currentRoute.split('?').reverse().pop();
        }
    }

    public getPreviousRoute(): string {
        return this.previousRoute;
    }

    public navigate(path: Array<string>, extras: any): void {
        this.router
            .navigate(path, extras)
            .then()
            .catch();
    }
}

export { RouterService };