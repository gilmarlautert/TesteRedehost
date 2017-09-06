import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { TldListaComponent } from './components/tld/lista/tld.lista.component';
import { TldExcluirComponent } from './components/tld/excluir/tld.excluir.component';
import { TldAdicionarComponent } from './components/tld/adicionar/tld.adicionar.component';
import { TldEditarComponent } from './components/tld/editar/tld.editar.component';

import { WhoisComponent } from './components/whois/whois.component';
import { AuthGuard } from './components/authentication/authguard';
import { AuthenticationService } from './components/authentication/authentication.service';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        WhoisComponent,
        TldListaComponent,
        TldAdicionarComponent,  
        TldEditarComponent,      
        TldExcluirComponent        
    ],
    providers: [AuthGuard, AuthenticationService],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: WhoisComponent },
            { path: 'tlds', component: TldListaComponent, canActivate: [AuthGuard]  },
            { path: 'tlds/add', component: TldAdicionarComponent, canActivate: [AuthGuard]  },
            { path: 'tlds/edit/:id', component: TldEditarComponent, canActivate: [AuthGuard]  },
            { path: 'tlds/delete/:id', component: TldExcluirComponent, canActivate: [AuthGuard]  },
            { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModuleShared {
}
