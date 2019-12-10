import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { ForgotPasswordComponent } from './forgotPassword/forgotPassword.component';
import { LoginComponent } from './login/login.component';
import { NotFoundComponent } from './notFound/notFound.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule
    ],
    exports: [],
    declarations: [
        NotFoundComponent,
        LoginComponent,
        ForgotPasswordComponent
    ],
    providers: [],
})
class PagesModule { }

export { PagesModule };