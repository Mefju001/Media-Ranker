import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { MediaResponse } from "../Data/Response/MediaResponse";
@Injectable({
    providedIn: 'root'
})
export class RecommendationService {
    private apiUrl = 'http://localhost:5009/api/Recommendation';
    constructor(private http: HttpClient) {}
    getRecommendations(selectedType: string): Observable<MediaResponse[]> {
        let params = new HttpParams();
        params = params.set("recommendationType", selectedType);
        return this.http.get<MediaResponse[]>(`${this.apiUrl}`, { params });
    }
}