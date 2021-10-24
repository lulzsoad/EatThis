import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { AdminRecipeCreateComponent } from './admin-panel/admin-recipes/admin-recipe-create/admin-recipe-create.component';
import { CategoriesComponent } from './admin-panel/categories/categories.component';
import { IngredientsComponent } from './admin-panel/ingredients/ingredients.component';
import { UnitsComponent } from './admin-panel/units/units.component';
import { LoginComponent } from './user-panel/login/login.component';
import { RegisterSuccessComponent } from './user-panel/register/register-success/register-success.component';
import { RegisterComponent } from './user-panel/register/register.component';
import { UserPanelComponent } from './user-panel/user-panel.component';

const routes: Routes = [
  {path: '', component: UserPanelComponent, pathMatch: 'full'},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent, children: []},
  {path: 'register/success', component: RegisterSuccessComponent},
  {path: 'admin-panel', component: AdminPanelComponent, children: [
    {path: 'ingredients', component: IngredientsComponent},
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
