import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Router, Params, ActivatedRoute } from "@angular/router";
import 'rxjs/add/operator/switchMap';

@Component({
    selector: 'tld-add',
    templateUrl: './tld.adicionar.component.html'
})
export class TldAdicionarComponent {
    public extension : string = '';
    public error : boolean;
    constructor(
        @Inject(Router) private _router: Router,
        private http: Http,
        @Inject('BASE_URL') private baseUrl: string)
    {
        
    }


    public adicionar(){
        this.http.post(this.baseUrl + 'api/tlds',{extension: this.extension}).subscribe(
            result => {
                this._router.navigate(['/tlds']);
            }, 
            error =>  { this.error = true;}
        );
    }
}