import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { TvSeriesResponse } from "../Data/Response/TvSeriesResponse";
import { MovieQuery } from "../Data/Request/MovieQuery";
import { TvSeriesRequest } from "../Data/Request/TvSeriesRequest";

@Injectable({
  providedIn: 'root' 
})
export class TvSeriesService {
    private apiUrl = 'http://localhost:5009/api/TvSeries';
    constructor(private http: HttpClient) {}
    getTvSeries(): Observable<TvSeriesResponse[]> {
        return this.http.get<TvSeriesResponse[]>(`${this.apiUrl}`);
    }
    GetGenres(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Genres`);
    }
    getTvSeriesById(id: string): Observable<TvSeriesResponse> {
        return this.http.get<TvSeriesResponse>(`${this.apiUrl}/${id}`);
    }
    getTvSeriesByFilter(query: MovieQuery): Observable<TvSeriesResponse[]> {
        let params = new HttpParams();
        Object.keys(query).forEach(key => {
        const value = query[key as keyof MovieQuery];
        if (value !== null && value !== undefined && value !== '') {
            params = params.set(key, value.toString());
        }
        });
        return this.http.get<TvSeriesResponse[]>(`${this.apiUrl}`, { params: params });
    }
    addTvSeries(tvSeries: TvSeriesRequest): Observable<any>
    {
        return this.http.post<any>(`${this.apiUrl}`, tvSeries);
    }
    updateSeries(seriesId: string, updateCommand: any) {
        return this.http.put(`${this.apiUrl}/${seriesId}`, updateCommand);
    }
    deleteSeries(seriesId: string): Observable<any> {
        return this.http.delete<any>(`${this.apiUrl}/${seriesId}`);
    }
}