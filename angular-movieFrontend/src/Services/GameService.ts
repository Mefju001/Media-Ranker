import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { MovieQuery } from "../ClientPages/MainPages/movie-web/MovieQuery";
import { GameResponse } from "../Data/Response/GameResponse";
import { Injectable } from "@angular/core";
import { GameRequest } from "../Data/Request/GameRequest";
import { GameQuery } from "../ClientPages/MainPages/game-web/GameQuery";
import { GenreResponse } from "../Data/Response/GenreResponse";
@Injectable({
    providedIn: 'root'
})
export class GameService {
    private apiUrl = 'http://localhost:5009/api/Game';
    constructor(private http: HttpClient) {}
    getGames(): Observable<GameResponse[]> {
    return this.http.get<GameResponse[]>(`${this.apiUrl}`);
    }
    GetPlatforms(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/Platforms`);
    }
    GetGenres(): Observable<GenreResponse[]> {
    return this.http.get<GenreResponse[]>(`${this.apiUrl}/Genres`);
    }
    getGameById(id: string): Observable<GameResponse> {
    return this.http.get<GameResponse>(`${this.apiUrl}/${id}`);
    }
    getGamesByFilter(query: GameQuery): Observable<GameResponse[]> {
    let params = new HttpParams();
    Object.keys(query).forEach(key => {
        const value = query[key as keyof GameQuery];
        if (value !== null && value !== undefined && value !== '') {
            params = params.set(key, value.toString());
        }
        });
    return this.http.get<GameResponse[]>(`${this.apiUrl}`, { params: params });
    }
    addGame(game: GameRequest): Observable<GameResponse>
    {
        return this.http.post<GameResponse>(`${this.apiUrl}`, game);
    }
    updateGame(gameId: string, updateCommand: GameRequest): Observable<GameResponse> {
      return this.http.put<GameResponse>(`${this.apiUrl}/${gameId}`, updateCommand);
    }
    deleteGame(gameId: string): Observable<void> {
      return this.http.delete<void>(`${this.apiUrl}/${gameId}`);
    }
}