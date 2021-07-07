import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-devolucao',
  templateUrl: './devolucao.component.html',
  styleUrls: ['./devolucao.component.css']
})
export class DevolucaoComponent implements OnInit {
  devolucoesForm!: FormGroup;

  constructor(private modalService: NgbModal, private fb: FormBuilder) { 
    this.devolucoesForm = this.fb.group({
      devolucaoId: [''],      
      usuario: [''],
      livro: [''],
      dataDevolucao: [''],
      observacao: ['']
    });
  }

  ngOnInit() {
  }

  open(content: any) {
    this.modalService.open(content, {windowClass: 'diabo-class'}).result.then((result) => {
      if(result == 'Save'){
        console.log(this.devolucoesForm);
      }
    });
  }

  close(content: any) {
    this.modalService.dismissAll()
      };
}
