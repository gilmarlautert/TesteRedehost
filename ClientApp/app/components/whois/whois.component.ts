import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'whois',
    templateUrl: './whois.component.html'
})
export class WhoisComponent {
    public text = "";
    public lista = [];
    private showAlert = false;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) {
    }

    public pesquisaWhois() {
        this.showAlert = false;
        this.http.get(this.baseUrl + 'api/whois?text='+this.text).subscribe(result => {
            this.lista = result.json();
            this.showAlert = true;
        }, error => console.error(error));    
    }
}
