import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { DatePipe } from '@angular/common';
import { Router, Params, ActivatedRoute } from "@angular/router";
import 'rxjs/add/operator/switchMap';

import { TLd } from '../tld';

@Component({
    selector: 'tld-add',
    templateUrl: './tld.editar.component.html'
})
export class TldEditarComponent {
    public extension : string;
    private tldId : number;
    constructor(
        private http: Http,
        @Inject(Router) private _router: Router,
        @Inject(ActivatedRoute) private route: ActivatedRoute,
        @Inject('BASE_URL') private baseUrl: string)
    {
        console.log(this.route.params);
        this.route.params.subscribe(params => {
            this.tldId = params['id'];
            console.log(params['id']);
        });
        this.http.get(this.baseUrl + 'api/tlds/' + this.tldId).subscribe(
            result => {
                var rs = result.json();
                var tld = rs as TLd;
                this.extension =tld.extension;
            }
        );
    }

    public error : boolean;

    public editar(){
        this.http.put(this.baseUrl + 'api/tlds/' + this.tldId,{extension: this.extension}).subscribe(
            result => {
                this._router.navigate(['/tlds']);
            }, 
            error =>  { this.error = true;}
        );
    }
}