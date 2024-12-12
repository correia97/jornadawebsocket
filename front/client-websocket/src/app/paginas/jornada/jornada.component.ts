import { Component } from '@angular/core';
import { BffService } from '../../servicos/bff.service';
import { Simulacao } from '../../Model/simulacao';
import { v4 as uuidv4 } from 'uuid'
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-jornada',
  imports: [],
  templateUrl: './jornada.component.html',
  styleUrl: './jornada.component.scss'
})
export class JornadaComponent {

  /**
   *
   */
  public simulacao: Simulacao | undefined;
  constructor(private bff: BffService, private router: Router) {

  }
  async simular() {

    let correlationID = uuidv4();
    let usuarioId = localStorage.getItem("usuarioId") ?? uuidv4();
    let simulacao = this.bff.simular(usuarioId, correlationID);
    // debugger;
    simulacao.subscribe(resp => {
      // debugger;
      this.simulacao = resp;
      localStorage.setItem("correlationID", correlationID);
    });
  }

  async contratar() {
    // debugger;
    let correlationID = localStorage.getItem("correlationID") ?? uuidv4();
    let simulacao = this.bff.contratar(this.simulacao as Simulacao, correlationID);
    simulacao.subscribe(resp => {
      // debugger;
      this.router.navigate(['/confirmacao'])
    });
  }
}

