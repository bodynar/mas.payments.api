import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app.routing';

import { CoreModule } from './core/core.module';

import { AppContainerComponent } from './component/appContainer.component';
import { AppComponent } from './components/app/app.component';
import { BellComponent } from './components/bell/bell.component';
import { HomeComponent } from './components/home/home.component';
import { MenuComponent } from './components/menu/menu.component';
import { NotificatorComponent } from './components/notificator/notificator.component';
import { AppUserComponent } from './components/user/user.component';

import { MeasurementModule } from './areas/measurement/measurement.module';
import { PaymentsModule } from './areas/payments/payments.module';
import { StatisticsModule } from './areas/statistics/statistics.module';
import { UserModule } from './areas/user/user.module';
import { ModalComponentsModule } from './components/modal/modalComponents.module';
import { PagesModule } from './pages/pages.module';

@NgModule({
  declarations: [
    AppContainerComponent,
    AppComponent,
    HomeComponent,
    MenuComponent,
    BellComponent,
    AppUserComponent,
    NotificatorComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    CommonModule,
    NgbModule,

    CoreModule,
    ModalComponentsModule,

    PaymentsModule,
    MeasurementModule,
    StatisticsModule,
    UserModule,
    PagesModule
  ],
  providers: [],
  bootstrap: [AppContainerComponent]
})
class AppModule { }

export { AppModule }