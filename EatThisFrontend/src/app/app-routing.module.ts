import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { AdminRecipeCreateComponent } from './admin-panel/admin-recipes/admin-recipe-create/admin-recipe-create.component';
import { AdminRecipeToAcceptComponent } from './admin-panel/admin-recipes/admin-recipes-to-accept-main/admin-recipe-to-accept/admin-recipe-to-accept.component';
import { AdminRecipesToAcceptMainComponent } from './admin-panel/admin-recipes/admin-recipes-to-accept-main/admin-recipes-to-accept-main.component';
import { AdminRecipesToAcceptComponent } from './admin-panel/admin-recipes/admin-recipes-to-accept-main/admin-recipes-to-accept/admin-recipes-to-accept.component';
import { CategoriesComponent } from './admin-panel/categories/categories.component';
import { IngredientCategoriesComponent } from './admin-panel/ingredient-categories/ingredient-categories.component';
import { IngredientsComponent } from './admin-panel/ingredients/ingredients.component';
import { UnitsComponent } from './admin-panel/units/units.component';
import { AdminGuard } from './guards/admin.guard';
import { AuthGuard } from './guards/auth.guard';
import { EmployeeGuard } from './guards/employee.guard';
import { AccountComponent } from './user-panel/account/account.component';
import { ActivateComponent } from './user-panel/account/activate/activate.component';
import { ForgotPasswordComponent } from './user-panel/account/forgot-password/forgot-password.component';
import { PasswordResetCodeComponent } from './user-panel/account/forgot-password/password-reset-code/password-reset-code.component';
import { LoginComponent } from './user-panel/account/login/login.component';
import { MyAccountComponent } from './user-panel/account/my-account/my-account.component';
import { RegisterFormComponent } from './user-panel/account/register/register-form/register-form.component';
import { RegisterSuccessComponent } from './user-panel/account/register/register-success/register-success.component';
import { RegisterComponent } from './user-panel/account/register/register.component';
import { RecipeAddComponent } from './user-panel/recipes-main/recipe-add/recipe-add.component';
import { RecipeComponent } from './user-panel/recipes-main/recipe/recipe.component';
import { RecipesMainComponent } from './user-panel/recipes-main/recipes-main.component';
import { RecipesComponent } from './user-panel/recipes-main/recipes/recipes.component';
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
      {path: 'my-account', component: MyAccountComponent, canActivate: [AuthGuard]},
      {path: 'forgot-password', component: ForgotPasswordComponent},
      {path: 'forgot-password/code', component: PasswordResetCodeComponent},
    ]},
    {path: 'recipes', component: RecipesMainComponent, children:
    [
      {path: '', component: RecipesComponent},
      {path: 'add', component: RecipeAddComponent, canActivate: [AuthGuard]},
      {path: ':id', component: RecipeComponent},
      
    ]}
  ]},
  
  {path: 'admin-panel', component: AdminPanelComponent, canActivate: [AuthGuard, EmployeeGuard], children: [
    {path: 'ingredients', component: IngredientsComponent, canActivate: [AuthGuard, AdminGuard]},
    {path: 'ingredient-categories', component: IngredientCategoriesComponent, canActivate: [AuthGuard, AdminGuard]},
    {path: 'categories', component: CategoriesComponent, canActivate: [AuthGuard, AdminGuard]},
    {path: 'units', component: UnitsComponent, canActivate: [AuthGuard, AdminGuard]},
    {path: 'create-recipe', component: AdminRecipeCreateComponent, canActivate: [AuthGuard, AdminGuard]},
    {path: 'recipes-to-accept', component: AdminRecipesToAcceptMainComponent, canActivate: [AuthGuard, EmployeeGuard], children: [
      {path: '', component: AdminRecipesToAcceptComponent, canActivate: [AuthGuard, EmployeeGuard]},
      {path: ':id', component: AdminRecipeToAcceptComponent, canActivate: [AuthGuard, EmployeeGuard]}
    ]}
  ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
