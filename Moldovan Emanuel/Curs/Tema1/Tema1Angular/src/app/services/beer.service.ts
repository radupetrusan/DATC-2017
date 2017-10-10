import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';
import { IBrewery } from "../interfaces/IBrewery";

@Injectable()
export class BeerService {

  constructor(private _http: Http) { }

  getBreweries(): Observable<IBrewery> {
    const headers = new Headers();
    headers.append('Content-type', 'application/hal+json');
    return this._http.get('http://datc-rest.azurewebsites.net/breweries', { headers: headers })
        .map((res: Response) => <IBrewery>res.json())
        .catch(this.handleError);
  }

  private handleError(error: Response) {
      console.log(error);
      return Observable.throw(error.json().error || 'Server error');
  }
}
