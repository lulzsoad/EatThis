import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { UserPanelComponent } from './user-panel/user-panel.component';
import { AdminNavigationComponent } from './admin-panel/admin-navigation/admin-navigation.component';
import { DropdownDirective } from './shared/directives/dropdown.directive';
import { InputsModule } from '@progress/kendo-angular-inputs';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GridModule } from '@progress/kendo-angular-grid';
import { IngredientsComponent } from './admin-panel/ingredients/ingredients.component';




@NgModule({
  declarations: [
    AppComponent,
    AdminPanelComponent,
    UserPanelComponent,
    AdminNavigationComponent,
    DropdownDirective,
    IngredientsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    InputsModule,
    BrowserAnimationsModule,
    GridModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
