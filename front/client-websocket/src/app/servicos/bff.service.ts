import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of, tap } from 'rxjs';
import { Simulacao } from '../Model/simulacao';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class BffService {

  constructor(private httpClient: HttpClient) { }

  simular(idUsuario: string, correlationId: string) : Observable<Simulacao> 
  {
    debugger;
   return this.httpClient.get<Simulacao>(`${environment.bffUrl}/simulacao/${idUsuario}`, {
      params: {idUsuario: idUsuario},
      headers: {
        'X-Debug-Level': 'verbose',
        'correlationId': correlationId
      }})
      .pipe(
        tap(_ => console.log('fetched')),
        catchError(this.handleError<Simulacao>('getSimulacao'))
      );
  }

  contratar(simulacao: Simulacao, correlationId: string)
  {
    debugger;
    return this.httpClient.post(`${environment.bffUrl}/simulacao`,simulacao, {
       headers: {
         'X-Debug-Level': 'verbose',
         'correlationId': correlationId
       }})
       .pipe(
         tap(_ => console.log('fetched')),
         catchError(this.handleError<Simulacao>('getSimulacao'))
       );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
  debugger;
      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead
  
      // TODO: better job of transforming error for user consumption
      console.log(`${operation} failed: ${error.message}`);
  
      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
