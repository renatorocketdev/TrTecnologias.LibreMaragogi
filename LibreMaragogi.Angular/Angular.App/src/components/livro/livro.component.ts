import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Livro } from 'src/models/Livro';
import { LivroService } from 'src/services/livro.service';

@Component({
  selector: 'app-livro',
  templateUrl: './livro.component.html',
  styleUrls: ['./livro.component.css']
})
export class LivroComponent implements OnInit {
  public livrosForm!: FormGroup;
  public livros: Livro[] = [];

  constructor(private modalService: NgbModal, private fb: FormBuilder, private service: LivroService) {}

  ngOnInit() {
    this.getAllLivros();
    this.group();
  }

  group(){
    this.livrosForm = this.fb.group({
      livrosId: 0,
      titulo: [''],
      autor: [''],
      categoria: [''],
      categoria2: [''],
      ano: [''],
      exemplares: [''],
      volume: [''],
      editora: [''],
      serie: [''],
      tombo: [''],
      classificacao: ['']
    });
  }

  getAllLivros() {
    this.service.getAll().subscribe(
      (livros: Livro[]) => {
        this.livros = livros;
      },
      (error: any) => {
        console.error(error);
      }
    );
  }

  onSubmit(){
    console.log(this.livrosForm.value);
    
    this.service.post(this.livrosForm.value).subscribe(
      (response) => {
        console.log(response);
    }, (erro: any) => {
      console.log(erro);
    });
  }

  open(content: any) {
    this.modalService.open(content, {windowClass: 'diabo-class'}).result.then((result) => {
      if(result == 'Save'){
        console.log(this.livrosForm);
      }
    });
  }

  close() {
    this.modalService.dismissAll()
  };
}
