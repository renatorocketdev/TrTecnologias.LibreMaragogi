import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Livro } from 'src/models/Livro';

@Injectable()
export class LivroService {

    constructor(private client: HttpClient) { }

    url = `${environment.baseUrl}/api/v1/livros`;
    
    getAll(): Observable<Livro[]>{
        return this.client.get<Livro[]>(`${this.url}`);
    }  
}
