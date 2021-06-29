import { Injectable } from '@angular/core';

const KEY = 'token';

@Injectable({
  providedIn: 'root'
})
export class tokenService {

  constructor() { }

  getToken(){
    return localStorage.getItem(KEY) ?? '';
  }

  saveToken(token: string){
    localStorage.setItem(KEY, token);
  }
 
  hasToken(){
    return !! this.getToken();
  }

  deleteToken(){
    localStorage.removeItem(KEY);
  }

}
