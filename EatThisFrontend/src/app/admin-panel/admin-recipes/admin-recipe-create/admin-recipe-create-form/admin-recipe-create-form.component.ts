import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Category } from 'src/app/models/category.model';
import { Ingredient } from 'src/app/models/ingredient.model';
import { CategoryService } from 'src/app/services/category.service';
import { IngredientService } from 'src/app/services/ingredient.service';

@Component({
  selector: 'app-admin-recipe-create-form',
  templateUrl: './admin-recipe-create-form.component.html',
  styleUrls: ['./admin-recipe-create-form.component.scss']
})
export class AdminRecipeCreateFormComponent implements OnInit {
  public form: FormGroup
  public ingredients: Ingredient[];
  public categories: Category[];
  public loadingPanelVisible = false;
  constructor(
    private categorySerice: CategoryService,
    private ingredientService: IngredientService
  ) { }

  async ngOnInit(): Promise<void> {
    this.loadingPanelVisible = true;

    this.form = new FormGroup({
      id: new FormControl(),
      name: new FormControl(),
      subName: new FormControl(),
      description: new FormControl(),
      difficulty: new FormControl(),
      isVisivble: new FormControl(),
      ingredientQuantities: new FormControl(),
      steps: new FormControl(),
      category: new FormControl(),
      image: new FormControl(),
      time: new FormControl(),
      personQuantity: new FormControl(),
    });

    await this.getIngredients();
    await this.getCategories();

    this.loadingPanelVisible = false;
  }

  async getIngredients(){
    this.ingredients = await this.ingredientService.getAll().toPromise();
  }

  async getCategories(){
    this.categories = await this.categorySerice.getAll().toPromise();
  }
}
