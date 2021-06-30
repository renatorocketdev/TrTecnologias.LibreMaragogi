import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
 
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LivroComponent } from 'src/components/livro/livro.component';
import { EmprestimoComponent } from 'src/components/emprestimo/emprestimo.component';
import { DevolucaoComponent } from 'src/components/devolucao/devolucao.component';
import { LeitorComponent } from 'src/components/leitor/leitor.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [				
    AppComponent,
      LivroComponent,
      EmprestimoComponent,
      DevolucaoComponent,
      LeitorComponent
   ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
