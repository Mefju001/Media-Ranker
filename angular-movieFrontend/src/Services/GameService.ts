import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { MovieQuery } from "../Data/Request/MovieQuery";
import { GameResponse } from "../Data/Response/GameResponse";
import { Injectable } from "@angular/core";
import { GameRequest } from "../Data/Request/GameRequest";
@Injectable({
    providedIn: 'root'
})
export class GameService {
    private apiUrl = 'http://localhost:5009/api/Game';
    constructor(private http: HttpClient) {}
    getGames(): Observable<GameResponse[]> {
    return this.http.get<GameResponse[]>(`${this.apiUrl}`);
    }
    GetPlatforms(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Platforms`);
    }
    GetGenres(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Genres`);
    }
    getGameById(id: string): Observable<GameResponse> {
    return this.http.get<GameResponse>(`${this.apiUrl}/${id}`);
    }
    getGamesByFilter(query: MovieQuery): Observable<GameResponse[]> {
    let params = new HttpParams();
    Object.keys(query).forEach(key => {
        const value = query[key as keyof MovieQuery];
        if (value !== null && value !== undefined && value !== '') {
            params = params.set(key, value.toString());
        }
        });
    return this.http.get<GameResponse[]>(`${this.apiUrl}`, { params: params });
    }
    addGame(game: GameRequest): Observable<any>
    {
        return this.http.post<any>(`${this.apiUrl}`, game);
    }
    updateGame(gameId: string, updateCommand: any) {
      return this.http.put<any>(`${this.apiUrl}/${gameId}`, updateCommand);
    }
    deleteGame(gameId: string): Observable<any> {
      return this.http.delete<any>(`${this.apiUrl}/${gameId}`);
    }
}