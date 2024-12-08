import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { WebsocketService } from './servicos/websocket.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { v4 as uuidv4 } from 'uuid'

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  private _snackBar = inject(MatSnackBar);
  title = 'client-websocket';
  exibeMensagem: boolean = false;
  mensagem: string = "";
  usuario: string = "";
  variant: string = "information";
  /**
   *
   */
  constructor(private websocket: WebsocketService) {
    let usuarioId = localStorage.getItem("usuarioId");
    if (usuarioId == null || usuarioId == "" || usuarioId == undefined) {
      localStorage.setItem("usuarioId", this.generateGUID());
    }
    websocket.escutarHub("ReceiveMessage", (user, message) => this.exibirNotificacao(user, message));
    websocket.iniciarConexao();
  }

  public exibirNotificacao(usuario: string, mensagem: string): void {
    console.log("exibirNotificacao");
    console.log(usuario);
    console.log(mensagem);
    this.mensagem = mensagem;
    this.exibeMensagem = true;
    this._snackBar.open(mensagem, this.variant);
  }

  generateGUID(): string {
    let myuuid = uuidv4();
    return myuuid;
  }

}
