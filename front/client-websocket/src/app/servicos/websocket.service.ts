import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel, IHttpConnectionOptions, HubConnectionState } from '@microsoft/signalr';
import { environment } from '../../environments/environment.development';
@Injectable({
  providedIn: 'root'
})
export class WebsocketService {

  public _hubConnection: HubConnection;
  public loginToken: string | null | Promise<string>;
  constructor() {
    this.loginToken = localStorage.getItem('token');
    let option: IHttpConnectionOptions = {
      accessTokenFactory: () => {
        return this.loginToken as string;
      },
      headers: {
        "xpto": "12345"
      }
    }
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(environment.hubUrl, option)
      .withAutomaticReconnect([5, 30, 10000])
      .configureLogging(LogLevel.Debug)
      .build();

    this._hubConnection.onreconnecting(err => {
      console.log("SignalR => Reconnecting...");
    })

    this._hubConnection.onreconnected(connId => {

      this.registrarUsuario();
      console.log("SignalR => Reconnected");
    })
  }

  async iniciarConexao(): Promise<void> {
    return this._hubConnection.start();

  }

  async escutarHub(metodo: string, callback: (...args: any[]) => any) {
    this._hubConnection.on(metodo, callback);
  }

  async enviarMensagem(metodo: string, usuario: string, mensagem: string): Promise<any> {
    try {
      return await this._hubConnection.invoke(metodo, usuario, mensagem);
    } catch (error) {
      debugger;
      console.log(error);
    }
  }

  async registrarUsuario() {
    debugger;
    if( this._hubConnection.state == HubConnectionState.Connecting){
      setTimeout(() => this.registrarUsuario(), 1000);
      return;
    }
    let usuarioId = localStorage.getItem("usuarioId");
    await this._hubConnection.invoke("RegistrarUsuario", usuarioId);
  }
}
