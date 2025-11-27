import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import {MovieResponse} from "../Data/Response/MovieResponse";
import { Injectable } from "@angular/core";
@Injectable({
  providedIn: 'root' 
})
export class MovieService {
private apiUrl = 'http://localhost:5009/Movie';
constructor(private http: HttpClient) {}
getMovies(): Observable<MovieResponse[]> {
    return this.http.get<MovieResponse[]>(`${this.apiUrl}`);
  }
}