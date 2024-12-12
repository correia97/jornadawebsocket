import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { WebsocketService } from './servicos/websocket.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { v4 as uuidv4 } from 'uuid'
import { environment } from '../environments/environment';

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

    console.log('environment.bffUrl')
    console.log(environment.bffUrl);
    
    console.log('environment.hubUrl')
    console.log(environment.hubUrl);

    let usuarioId = localStorage.getItem("usuarioId");
    if (usuarioId == null || usuarioId == "" || usuarioId == undefined) {
      localStorage.setItem("usuarioId", this.generateGUID());
    }

    let token = localStorage.getItem("token");
    if (token == null || token == "" || token == undefined) {
      this.setToken();
    }
    websocket.escutarHub("ReceiveMessage", (user, message) => this.exibirNotificacao(user, message));
    websocket.iniciarConexao();
    websocket.registrarUsuario();
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

  setToken(){
    localStorage.setItem("token","eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c");
  }

}
