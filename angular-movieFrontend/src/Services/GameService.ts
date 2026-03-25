import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { MovieQuery } from "../Data/Request/MovieQuery";
import { GameResponse } from "../Data/Response/GameResponse";
import { Injectable } from "@angular/core";
@Injectable({
    providedIn: 'root'
})
export class GameService {
    private apiUrl = 'http://localhost:5009/Game';
    constructor(private http: HttpClient) {}
    getGames(): Observable<GameResponse[]> {
    return this.http.get<GameResponse[]>(`${this.apiUrl}`);
    }
    getGameById(id: number): Observable<GameResponse> {
    return this.http.get<GameResponse>(`${this.apiUrl}/id/${id}`);
    }
    getGamesByFilter(query: MovieQuery): Observable<GameResponse[]> {
    let params = new HttpParams();
    Object.keys(query).forEach(key => {
        const value = query[key as keyof MovieQuery];
        if (value !== null && value !== undefined && value !== '') {
            params = params.set(key, value.toString());
        }
        });
    return this.http.get<GameResponse[]>(`${this.apiUrl}/FilterBy`, { params: params });
    }
}