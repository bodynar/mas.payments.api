import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app.routing';
import { CoreModule } from './core/core.module';

import { AppContainerComponent } from './component/appContainer.component';
import { AppComponent } from './components/app/app.component';
import { HomeComponent } from './components/home/home.component';
import { MenuComponent } from './components/menu/menu.component';

import { MeasurementModule } from './areas/measurement/measurement.module';
import { PaymentsModule } from './areas/payments/payments.module';
import { PagesModule } from './pages/pages.module';

@NgModule({
  declarations: [
    AppContainerComponent,
    AppComponent,
    HomeComponent,
    MenuComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    CoreModule,

    PaymentsModule,
    MeasurementModule,
    PagesModule
  ],
  providers: [],
  bootstrap: [AppContainerComponent]
})
class AppModule { }

export { AppModule };