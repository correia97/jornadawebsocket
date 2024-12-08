import { Routes } from '@angular/router';
import { ConfirmacaoComponent } from './paginas/confirmacao/confirmacao.component';
import { JornadaComponent } from './paginas/jornada/jornada.component';

export const routes: Routes = [
    { path:'',component:JornadaComponent},
    { path:'jornada',component:JornadaComponent},
    { path:'confirmacao',component:ConfirmacaoComponent},
];
