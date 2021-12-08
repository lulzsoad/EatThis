import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { AdminRecipeCreateComponent } from './admin-panel/admin-recipes/admin-recipe-create/admin-recipe-create.component';
import { CategoriesComponent } from './admin-panel/categories/categories.component';
import { IngredientCategoriesComponent } from './admin-panel/ingredient-categories/ingredient-categories.component';
import { IngredientsComponent } from './admin-panel/ingredients/ingredients.component';
import { UnitsComponent } from './admin-panel/units/units.component';
import { AuthGuard } from './guards/auth.guard';
import { AccountComponent } from './user-panel/account/account.component';
import { ActivateComponent } from './user-panel/account/activate/activate.component';
import { ForgotPasswordComponent } from './user-panel/account/forgot-password/forgot-password.component';
import { PasswordResetCodeComponent } from './user-panel/account/forgot-password/password-reset-code/password-reset-code.component';
import { LoginComponent } from './user-panel/account/login/login.component';
import { RegisterFormComponent } from './user-panel/account/register/register-form/register-form.component';
import { RegisterSuccessComponent } from './user-panel/account/register/register-success/register-success.component';
import { RegisterComponent } from './user-panel/account/register/register.component';
import { UserPanelComponent } from './user-panel/user-panel.component';

const routes: Routes = [
  {path: '', component: UserPanelComponent, children: [
    {path: 'login', component: LoginComponent},
    {path: 'register', component: RegisterComponent, children: [
      {path: '', component: RegisterFormComponent},
      {path: 'success', component: RegisterSuccessComponent},
    ]},
    {path: 'account', component: AccountComponent, children: [
      {path: 'activate', component: ActivateComponent},
      {path: 'forgot-password', component: ForgotPasswordComponent},
      {path: 'forgot-password/code', component: PasswordResetCodeComponent},
    ]}
  ]},
  
  {path: 'admin-panel', component: AdminPanelComponent, canActivate: [AuthGuard], children: [
    {path: 'ingredients', component: IngredientsComponent},
    {path: 'ingredient-categories', component: IngredientCategoriesComponent},
    {path: 'categories', component: CategoriesComponent},
    {path: 'units', component: UnitsComponent},
    {path: 'create-recipe', component: AdminRecipeCreateComponent}
  ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
