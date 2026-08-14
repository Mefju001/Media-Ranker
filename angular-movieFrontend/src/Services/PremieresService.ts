import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { PremieresDto } from "../ClientPages/premieres/PremieresDto";
@Injectable({
    providedIn: 'root'
})
export class PremieresService {
  private apiUrl = 'http://localhost:5009/api/PremieresAndAnnouncements';
  constructor(private http: HttpClient) {}
  getPremieres(premieresDto: PremieresDto): Observable<any[]> {
    var params = new HttpParams();
    if (premieresDto.scope) {
      params = params.set('scope', premieresDto.scope);
    }
    if (premieresDto.type&& premieresDto.type !== 'all') {
        params = params.set('mediaType', premieresDto.type);   
    }
    return this.http.get<any[]>(`${this.apiUrl}`, { params });
  }
}