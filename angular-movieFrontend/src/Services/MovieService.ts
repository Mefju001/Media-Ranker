import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import {MovieResponse} from "../Data/Response/MovieResponse";
import { Injectable } from "@angular/core";
import { MovieQuery } from "../Data/Request/MovieQuery";
@Injectable({
  providedIn: 'root' 
})
export class MovieService {
private apiUrl = 'http://localhost:5009/api/Movie';
constructor(private http: HttpClient) {}
getMovies(): Observable<MovieResponse[]> {
  return this.http.get<MovieResponse[]>(`${this.apiUrl}`);
}
getMovieById(id: number): Observable<MovieResponse> {
  return this.http.get<MovieResponse>(`${this.apiUrl}/${id}`);
}
getMoviesByFilter(query: MovieQuery): Observable<MovieResponse[]> {
  let params = new HttpParams();
  Object.keys(query).forEach(key => {
      const value = query[key as keyof MovieQuery];
      if (value !== null && value !== undefined && value !== '') {
        params = params.set(key, value.toString());
      }
    });
  return this.http.get<MovieResponse[]>(`${this.apiUrl}`, { params: params });
}

}