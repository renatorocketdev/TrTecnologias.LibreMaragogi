import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LivroService } from 'src/services/livro.service';

@Component({
  selector: 'app-livro',
  templateUrl: './livro.component.html',
  styleUrls: ['./livro.component.css']
})
export class LivroComponent implements OnInit {
  livrosForm!: FormGroup;

  constructor(private modalService: NgbModal, private fb: FormBuilder, private service: LivroService) { 
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

  ngOnInit() {
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
}
