import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { AdminRecipeCreateComponent } from './admin-panel/admin-recipes/admin-recipe-create/admin-recipe-create.component';
import { CategoriesComponent } from './admin-panel/categories/categories.component';
import { IngredientsComponent } from './admin-panel/ingredients/ingredients.component';
import { UnitsComponent } from './admin-panel/units/units.component';
import { UserPanelComponent } from './user-panel/user-panel.component';

const routes: Routes = [
  {path: '', component: UserPanelComponent},
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
