import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { IngredientsComponent } from './admin-panel/ingredients/ingredients.component';
import { UserPanelComponent } from './user-panel/user-panel.component';

const routes: Routes = [
  {path: '', component: UserPanelComponent},
  {path: 'admin-panel', component: AdminPanelComponent, children: [
    {path: 'ingredients', component: IngredientsComponent}
  ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
