import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login.component';
import { RegistrarComponent } from '../registrar/registrar.component';


const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'registrar', loadChildren:() => import('../registrar/registrar.module').then((m) => m.RegistrarModule )}
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LoginRoutingModule { }
