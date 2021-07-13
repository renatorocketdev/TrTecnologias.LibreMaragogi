import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { Usuario } from 'src/models/Usuario';
import { tokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  userData = new BehaviorSubject<Usuario>(new Usuario());

  url = `${environment.baseUrl}/api/v1/login`;

  constructor(private http: HttpClient, private token: tokenService) { }

  authenticate(usuario: Usuario): Observable<any>{
    return this.http.post<any>(this.url, usuario).pipe(
      map(response => {
        this.token.saveToken(response.token);
        this.setUserDetails();
      return response;
    }));
  }

  setUserDetails() {
    if (localStorage.getItem('token')) {
      const token = this.token.getToken().split('.')[1];
      const userDetails = new Usuario();
      const decodeUserDetails = JSON.parse(window.atob(token));

      userDetails.nome = decodeUserDetails.nome;
      userDetails.role = decodeUserDetails.role;

      this.userData.next(userDetails);
    }
  }

  // logout() {
  //   localStorage.removeItem('authToken');
  //   this.router.navigate(['/login']);
  //   this.userData.next(new User());
  // }
}
