import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'home'},
  { path: 'home', loadChildren:() => import('../modules/home/home.module').then((m) => m.HomeModule )},
  { path: 'login', loadChildren:() => import('../modules/login/login.module').then((m) => m.LoginModule )},
  { path: 'dashboard', loadChildren:() => import('../modules/dashboard/dashboard.module').then((m) => m.DashboardModule )},
  { path: 'marketplace', loadChildren:() => import('../modules/marketplace/marketplace.component').then((m) => m.MarketplaceComponent )},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
