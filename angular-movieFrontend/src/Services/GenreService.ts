import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import {GenreResponse} from "../Data/Response/GenreResponse";
import { Injectable } from "@angular/core";
@Injectable({
  providedIn: 'root' 
})
export class GenreService {
    private apiUrl = 'http://localhost:5009/Genre';
    constructor(private http: HttpClient) {}
    getGenres(): Observable<GenreResponse[]> {
        return this.http.get<GenreResponse[]>(`${this.apiUrl}`);
    }
}