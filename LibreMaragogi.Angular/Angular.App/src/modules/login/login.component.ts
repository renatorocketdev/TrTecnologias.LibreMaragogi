import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from 'src/services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(private auth: AuthService, private router: Router, private modalService: NgbModal, private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      cpf: [''],
      senha: ['']
    });
  }

  ngOnInit() {

  }

  onSubmit() {
    this.auth.authenticate(this.loginForm.value).subscribe(
      (dados) => {
        if(dados.userDetails.role == "Administrador"){
            this.router.navigate(['/', 'dashboard']);
        } else{
          this.router.navigate(['/', 'marketplace']);
        }
      }, 
      (error) => {
        alert(error);
      }
    );
  }
}
