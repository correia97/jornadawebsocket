import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class WebsocketService {

  public _hubConnection: HubConnection;
  public loginToken: string | null;
  constructor() {
    this.loginToken = localStorage.getItem('token');
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(environment.hubUrl)
      .withAutomaticReconnect([5, 30, 10000])
      .configureLogging(LogLevel.Debug)
      .build();
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
}
