import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'tld',
    templateUrl: './tld.component.html'
})
export class TldComponent {
    public tlds: TLd[];

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/tlds').subscribe(result => {
            this.tlds = result.json() as TLd[];
        }, error => console.error(error));
    }
}

interface TLd {
    extension: string;
    id: number;
    usuarioCriacao: string;
    usuarioAlteracao: string;
    dataCriacao: string;
    dataAlteracao: string;
}
