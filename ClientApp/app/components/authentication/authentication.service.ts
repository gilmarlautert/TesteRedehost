import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Http, URLSearchParams } from '@angular/http';
import { Observable } from 'rxjs/Rx';

@Injectable()
export class AuthenticationService {

    constructor(private router: Router,private http: Http) { }

    isLoggedIn () {
        console.log("/account/logado");
            return this.http.get( "/account/logado")
            .toPromise()
            .then(data => {
                window.localStorage.setItem('usuario',data.toString());
                return Observable.of(true);
            }).catch(data => {
                console.log('erro',data);
                window.location.href = "/account/login";
                return Observable.of(false);
            });
        
    }
}