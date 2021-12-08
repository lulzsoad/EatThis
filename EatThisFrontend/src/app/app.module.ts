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
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { IndicatorsModule } from '@progress/kendo-angular-indicators';
import { DialogsModule, WindowModule} from '@progress/kendo-angular-dialog';
import { ButtonsModule } from '@progress/kendo-angular-buttons';
import { IngredientCreateModalComponent } from './admin-panel/ingredients/ingredient-create-modal/ingredient-create-modal.component';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';
import { LabelModule } from '@progress/kendo-angular-label';
import { WindowService } from './services/window.service';
import { DeleteObjectModalComponent } from './shared/components/delete-object-modal/delete-object-modal.component';
import { AdminRecipeCreateComponent } from './admin-panel/admin-recipes/admin-recipe-create/admin-recipe-create.component';
import { AdminRecipeCreateFormComponent } from './admin-panel/admin-recipes/admin-recipe-create/admin-recipe-create-form/admin-recipe-create-form.component';
import { AdminRecipeCreatePreviewComponent } from './admin-panel/admin-recipes/admin-recipe-create/admin-recipe-create-preview/admin-recipe-create-preview.component';
import { CategoriesFormModalComponent } from './admin-panel/categories/categories-form-modal/categories-form-modal.component';
import { CategoriesComponent } from './admin-panel/categories/categories.component';
import { CategoryService } from './services/category.service';
import { DropDownsModule } from '@progress/kendo-angular-dropdowns';
import { UnitsComponent } from './admin-panel/units/units.component';
import { UnitFormModalComponent } from './admin-panel/units/unit-form-modal/unit-form-modal.component';
import { UnitService } from './services/unit.service';
import { ToastrModule } from 'ngx-toastr';
import { AlertService } from './services/alert.service';
import { LoginComponent } from './user-panel/account/login/login.component';
import { RegisterComponent } from './user-panel/account/register/register.component';
import {RequiredFieldDirective } from './shared/directives/required-field.directive';
import { AccountService } from './services/account.service';
import { RegisterSuccessComponent } from './user-panel/account/register/register-success/register-success.component';
import { ActivateComponent } from './user-panel/account/activate/activate.component';
import { IngredientCategoriesComponent } from './admin-panel/ingredient-categories/ingredient-categories.component';
import { IngredientCategoryFormModalComponent } from './admin-panel/ingredient-categories/ingredient-category-form-modal/ingredient-category-form-modal.component';
import { IngredientCategoryService } from './services/ingredient-category.service';
import { ForgotPasswordComponent } from './user-panel/account/forgot-password/forgot-password.component';
import { PasswordResetCodeComponent } from './user-panel/account/forgot-password/password-reset-code/password-reset-code.component';
import { ForgotPasswordNewPasswordComponent } from './user-panel/account/forgot-password/forgot-password-new-password/forgot-password-new-password.component';
import { SpinnerComponent } from './shared/components/spinner/spinner.component';
import { RegisterFormComponent } from './user-panel/account/register/register-form/register-form.component';
import { AccountComponent } from './user-panel/account/account.component';
import { UserNavigationComponent } from './user-panel/user-navigation/user-navigation.component';
import { AuthService } from './services/auth.service';
import { AppInformationService } from './services/app-information.service';
import { AuthInterceptorService } from './services/auth-interceptor.service';
import { AuthGuard } from './guards/auth.guard';


@NgModule({
  declarations: [
    AppComponent,
    AdminPanelComponent,
    UserPanelComponent,
    AdminNavigationComponent,
    DropdownDirective,
    RequiredFieldDirective,
    IngredientsComponent,
    IngredientCreateModalComponent,
    DeleteObjectModalComponent,
    AdminRecipeCreateComponent,
    AdminRecipeCreateFormComponent,
    AdminRecipeCreatePreviewComponent,
    CategoriesComponent,
    CategoriesFormModalComponent,
    UnitsComponent,
    UnitFormModalComponent,
    LoginComponent,
    RegisterComponent,
    RegisterSuccessComponent,
    RegisterFormComponent,
    ActivateComponent,
    IngredientCategoriesComponent,
    IngredientCategoryFormModalComponent,
    ForgotPasswordComponent,
    PasswordResetCodeComponent,
    ForgotPasswordNewPasswordComponent,
    SpinnerComponent,
    AccountComponent,
    UserNavigationComponent
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
    DropDownsModule,
    ToastrModule.forRoot(),
  ],
  providers: [
    HttpClient,
    IngredientService,
    WindowService,
    CategoryService,
    UnitService,
    AlertService,
    AccountService,
    IngredientCategoryService,
    AuthService,
    AppInformationService,
    AuthGuard,
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptorService, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
