import { NgModule } from '@angular/core';

import { LoginComponent } from './login/login.component';
import { NotFoundComponent } from './notFound/notFound.component';

@NgModule({
    imports: [],
    exports: [],
    declarations: [
        NotFoundComponent,
        LoginComponent
    ],
    providers: [],
})
class PagesModule { }

export { PagesModule };