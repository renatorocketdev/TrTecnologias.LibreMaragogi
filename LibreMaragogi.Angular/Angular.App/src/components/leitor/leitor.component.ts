import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-leitor',
  templateUrl: './leitor.component.html',
  styleUrls: ['./leitor.component.css']
})
export class LeitorComponent implements OnInit {
  leitoresForm!: FormGroup;

  constructor(private modalService: NgbModal, private fb: FormBuilder) { 
    this.leitoresForm = this.fb.group({
      leitoresId: [''],      
      nome: [''],
      email: [''],
      nascimento: [''],
      cpf: [''],
      telefone: [''],
      cep: [''],
      numero: [''],
      logradouro: [''],
      senha: [''],
      area: [''],
      profissao: [''],
      sexo: ['']
    });
  }

  ngOnInit() {
  }

  open(content: any) {
    this.modalService.open(content, {windowClass: 'diabo-class'}).result.then((result) => {
      if(result == 'Save'){
        console.log(this.leitoresForm);
      }
    });
  }

  close(content: any) {
    this.modalService.dismissAll()
      };
}
