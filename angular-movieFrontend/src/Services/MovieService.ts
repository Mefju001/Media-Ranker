import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import {MovieResponse} from "../Data/Response/MovieResponse";
import { Injectable } from "@angular/core";
import { MovieQuery } from "../Data/Request/MovieQuery";
@Injectable({
  providedIn: 'root' 
})
export class MovieService {
private apiUrl = 'http://localhost:5009/Movie';
constructor(private http: HttpClient) {}
getMovies(): Observable<MovieResponse[]> {
  return this.http.get<MovieResponse[]>(`${this.apiUrl}`);
}
getMovieById(id: number): Observable<MovieResponse> {
  return this.http.get<MovieResponse>(`${this.apiUrl}/id/${id}`);
}
getMoviesByFilter(query: MovieQuery): Observable<MovieResponse[]> {
  let params = new HttpParams();
    for (const key in query) {
        const value = query[key as keyof MovieQuery];
        
        if (value !== null && value !== undefined && value !== '') {
            params = params.append(key, String(value));
        }
    }
  return this.http.get<MovieResponse[]>(`${this.apiUrl}/FilterBy`, { params: params });
}

}