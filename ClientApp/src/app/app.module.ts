import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app.routing';

import { AppContainerComponent } from './component/appContainer.component';
import { HomeComponent } from './components/home/home.component';

@NgModule({
  declarations: [
    AppContainerComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
  ],
  providers: [],
  bootstrap: [AppContainerComponent]
})
class AppModule { }

export { AppModule };