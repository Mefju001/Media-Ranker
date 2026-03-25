import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { TvSeriesResponse } from "../Data/Response/TvSeriesResponse";
import { MovieQuery } from "../Data/Request/MovieQuery";

@Injectable({
  providedIn: 'root' 
})
export class TvSeriesService {
    private apiUrl = 'http://localhost:5009/TvSeries';
    constructor(private http: HttpClient) {}
    getTvSeries(): Observable<TvSeriesResponse[]> {
        return this.http.get<TvSeriesResponse[]>(`${this.apiUrl}`);
    }
    getMovieById(id: number): Observable<TvSeriesResponse> {
        return this.http.get<TvSeriesResponse>(`${this.apiUrl}/id/${id}`);
    }
    getMoviesByFilter(query: MovieQuery): Observable<TvSeriesResponse[]> {
        let params = new HttpParams();
        Object.keys(query).forEach(key => {
        const value = query[key as keyof MovieQuery];
        if (value !== null && value !== undefined && value !== '') {
            params = params.set(key, value.toString());
        }
        });
        return this.http.get<TvSeriesResponse[]>(`${this.apiUrl}/FilterBy`, { params: params });
    }
}