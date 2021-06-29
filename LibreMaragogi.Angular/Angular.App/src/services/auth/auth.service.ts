import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  url = `${environment.baseUrl}/api/v1/login`;

  constructor(private client: HttpClient) { }

  authenticate(user: string, pwd: string): Observable<any>{
    return this.client.post(this.url, {
      Nome: user,
      Senha: pwd
    });
  }

}
