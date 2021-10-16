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
import { IngredientService } from './services/ingredient.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { IndicatorsModule } from '@progress/kendo-angular-indicators';
import { DialogsModule, WindowModule} from '@progress/kendo-angular-dialog';
import { ButtonsModule } from '@progress/kendo-angular-buttons';
import { IngredientCreateModalComponent } from './admin-panel/ingredients/ingredient-create-modal/ingredient-create-modal.component';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';
import { LabelModule } from '@progress/kendo-angular-label';
import { WindowService } from './services/window.service';
import { DeleteObjectModalComponent } from './shared/components/delete-object-modal/delete-object-modal.component';

@NgModule({
  declarations: [
    AppComponent,
    AdminPanelComponent,
    UserPanelComponent,
    AdminNavigationComponent,
    DropdownDirective,
    IngredientsComponent,
    IngredientCreateModalComponent,
    DeleteObjectModalComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    InputsModule,
    BrowserAnimationsModule,
    GridModule,
    HttpClientModule,
    IndicatorsModule,
    BrowserModule, 
    IndicatorsModule, 
    DialogsModule,
    WindowModule,
    ButtonsModule,
    FormsModule,
    ReactiveFormsModule,
    DateInputsModule,
    InputsModule,
    LabelModule,
  ],
  providers: [
    HttpClient,
    IngredientService,
    WindowService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
