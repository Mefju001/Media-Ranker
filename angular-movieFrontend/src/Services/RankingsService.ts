import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { RankingDto } from "../ClientPages/rankings/RankingDto";
@Injectable({
    providedIn: 'root'
})
export class RankingsService {
  private apiUrl = 'http://localhost:5009/api/Ranking';
  constructor(private http: HttpClient) {}
  getRankings(rankingDto: RankingDto): Observable<any[]> {
    var params = new HttpParams();
    if (rankingDto.type && rankingDto.type !== 'all') {
      params = params.set('mediaType', rankingDto.type);
    }
    return this.http.get<any[]>(`${this.apiUrl}`, { params });
  }
}