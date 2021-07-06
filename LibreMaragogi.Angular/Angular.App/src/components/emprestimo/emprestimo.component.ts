import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-emprestimo',
  templateUrl: './emprestimo.component.html',
  styleUrls: ['./emprestimo.component.css']
})
export class EmprestimoComponent implements OnInit {
  emprestimosForm!: FormGroup;

  constructor(private modalService: NgbModal, private fb: FormBuilder) { 
    this.emprestimosForm = this.fb.group({
      emprestimosId: [''],
      classificacao: ['']
    });
  }

  ngOnInit() {
  }

  open(content: any) {
    this.modalService.open(content, {windowClass: 'diabo-class'}).result.then((result) => {
      if(result == 'Save'){
        console.log(this.emprestimosForm);
      }
    });
  }

  close(content: any) {
    this.modalService.dismissAll()
      };
}
