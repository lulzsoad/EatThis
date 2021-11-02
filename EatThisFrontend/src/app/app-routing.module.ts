import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { AdminRecipeCreateComponent } from './admin-panel/admin-recipes/admin-recipe-create/admin-recipe-create.component';
import { CategoriesComponent } from './admin-panel/categories/categories.component';
import { IngredientCategoriesComponent } from './admin-panel/ingredient-categories/ingredient-categories.component';
import { IngredientsComponent } from './admin-panel/ingredients/ingredients.component';
import { UnitsComponent } from './admin-panel/units/units.component';
import { ActivateComponent } from './user-panel/account/activate/activate.component';
import { ForgotPasswordComponent } from './user-panel/account/forgot-password/forgot-password.component';
import { LoginComponent } from './user-panel/account/login/login.component';
import { RegisterSuccessComponent } from './user-panel/account/register/register-success/register-success.component';
import { RegisterComponent } from './user-panel/account/register/register.component';
import { UserPanelComponent } from './user-panel/user-panel.component';

const routes: Routes = [
  {path: '', component: UserPanelComponent, pathMatch: 'full'},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'register/success', component: RegisterSuccessComponent},
  {path: 'account/activate', component: ActivateComponent},
  {path: 'account/forgot-password', component: ForgotPasswordComponent},
  {path: 'admin-panel', component: AdminPanelComponent, children: [
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
