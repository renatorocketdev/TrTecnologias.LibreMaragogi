import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-livro',
  templateUrl: './livro.component.html',
  styleUrls: ['./livro.component.css']
})
export class LivroComponent implements OnInit {
  livrosForm!: FormGroup;

  constructor(private modalService: NgbModal, private fb: FormBuilder) { 
    this.livrosForm = this.fb.group({
      livrosId: [''],
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

  open(content: any) {
    this.modalService.open(content, {windowClass: 'diabo-class'}).result.then((result) => {
      if(result == 'Save'){
        console.log(this.livrosForm);
      }
    });
  }
}
