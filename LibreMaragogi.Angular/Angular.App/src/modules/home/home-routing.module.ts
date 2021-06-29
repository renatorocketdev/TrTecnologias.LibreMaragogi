import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from '../login/login.component';
import { RegistrarComponent } from '../registrar/registrar.component';
import { HomeComponent } from './home.component';

const routes: Routes = [
  { path: '', component: HomeComponent  }  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
