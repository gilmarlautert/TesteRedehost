import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Router } from "@angular/router";
import { TLd } from '../tld';

@Component({
    selector: 'tld-lista',
    templateUrl: './tld.lista.component.html'
})
export class TldListaComponent {
    public tlds: TLd[];

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string, @Inject(Router) private _router: Router) {
        http.get(baseUrl + 'api/tlds').subscribe(result => {
            try{
                var rs = result.json();
                this.tlds = rs as TLd[];
            }
            catch(e){
            }
        }, error => console.error(error));
    }

    gotoDelete(tld: TLd){
        this._router.navigate(['/tlds/' + tld.id + '/delete']);
    }
}
