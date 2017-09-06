import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { Router, Params, ActivatedRoute } from "@angular/router";
import 'rxjs/add/operator/switchMap';

@Component({
    selector: 'tld-delete',
    templateUrl: './tld.excluir.component.html'
})
export class TldExcluirComponent {

    private tldId : number;
    public error : boolean;
    constructor(
        private http: Http,
        @Inject(Router) private _router: Router,
        @Inject(ActivatedRoute) private route: ActivatedRoute,
        @Inject('BASE_URL') private baseUrl: string){
            console.log(this.route.params);
            this.route.params.subscribe(params => {
                this.tldId = params['id'];
                console.log(params['id']);
           });
        }


    public remove(){
        this.http.delete(this.baseUrl + 'api/tlds/' + this.tldId).subscribe(result => {
            try{
               this._router.navigate(['/tlds']);
            }
            catch(e){
                this.error = true;
            }
        }, error => console.error(error));
       

    }
}