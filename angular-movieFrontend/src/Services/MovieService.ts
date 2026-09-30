import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import {MovieResponse} from "../Data/Response/MovieResponse";
import { Injectable } from "@angular/core";
import { MovieQuery } from "../ClientPages/MainPages/movie-web/MovieQuery";
import { MovieRequest } from "../Data/Request/MovieRequest";
import { GenreResponse } from "../Data/Response/GenreResponse";
@Injectable({
  providedIn: 'root' 
})
export class MovieService {

private apiUrl = 'http://localhost:5009/api/Movie';
constructor(private http: HttpClient) {}
getMovies(): Observable<MovieResponse[]> {
  return this.http.get<MovieResponse[]>(`${this.apiUrl}`);
}
getMovieById(id: string): Observable<MovieResponse> {
  console.log(`Fetching movie with ID: ${id}`);
  return this.http.get<MovieResponse>(`${this.apiUrl}/${id}`);
}
GetGenres(): Observable<GenreResponse[]> {
  return this.http.get<GenreResponse[]>(`${this.apiUrl}/Genres`);
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
addMovie(movie: MovieRequest): Observable<MovieResponse>
{
    return this.http.post<MovieResponse>(`${this.apiUrl}`, movie);
}
deleteMovie(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
}
updateMovie(movieId: string, updateCommand: MovieRequest): Observable<MovieResponse> {
  return this.http.put<MovieResponse>(`${this.apiUrl}/${movieId}`, updateCommand);
}
}