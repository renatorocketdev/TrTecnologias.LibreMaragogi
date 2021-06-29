import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DevolucaoComponent } from 'src/components/devolucao/devolucao.component';
import { EmprestimoComponent } from 'src/components/emprestimo/emprestimo.component';
import { LeitorComponent } from 'src/components/leitor/leitor.component';
import { LivroComponent } from 'src/components/livro/livro.component';
import { ManagementComponent } from 'src/components/management/management.component';
import { DashboardComponent } from './dashboard.component';

const routes: Routes = [
  { path:  '', component: DashboardComponent, 
    children: [
      { path:  '', component: ManagementComponent },
      { path:  'livro', component: LivroComponent },
      { path:  'devolucao', component: DevolucaoComponent },
      { path:  'emprestimo', component: EmprestimoComponent },
      { path:  'leitor', component: LeitorComponent },
    ] 
  },
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
