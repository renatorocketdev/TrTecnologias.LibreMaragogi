import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  cpf = '';
  pwd = '';

  constructor(private auth: AuthService, private router: Router) {}

  ngOnInit() {

  }

  login() {
    //this.router.navigate(['/', 'dashboard']);
    
    this.auth.authenticate(this.cpf, this.pwd).subscribe(
      () => {
        this.router.navigate(['/', 'dashboard']);
      }, 
      (error) => {
        alert(error);
      }
    );
  }
}
