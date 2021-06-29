import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  user = '';
  pwd = '';

  constructor(private auth: AuthService, private router: Router) {}

  ngOnInit() {

  }

  login() {
    this.auth.authenticate(this.user, this.pwd).subscribe(
      () => {
        this.router.navigate(['/', 'dashboard']);
      }, 
      (error) => {
        alert(error);
      }
    );
  }
}
