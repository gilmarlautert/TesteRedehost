import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { AppConfig } from '../../config/config';

@Component({
    selector: 'whois',
    templateUrl: './whois.component.html',
    styles: ['./whois.component.css']
})
export class WhoisComponent {
    public text = "";
    public tld = "";
    public lista = [];
    private showAlert = false;
    private available = false;
    

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) {
        this.http.get(this.baseUrl + AppConfig.whoisListTld).subscribe(result => {
            this.lista = result.json();
        }, error => console.error(error));    
    }

    public pesquisaWhois() {
        this.showAlert = false;
        let domain = this.text + this.tld;
        this.http.post(AppConfig.whoisEndpoint + '?' + AppConfig.whoisQsParam + '=' + domain, {} ).subscribe(result => {
            this.showAlert = true;
            let lista : any = result.json();
            this.available = lista['available?'];
        }, error => console.error(error));    
    }
}
