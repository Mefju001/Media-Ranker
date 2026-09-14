import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
@Injectable({
    providedIn: 'root'
})
export class RecommendationService {
    private apiUrl = 'http://localhost:5009/api/Recommendation';
    constructor(private http: HttpClient) {}
    getRecommendations(selectedType: string): Observable<any[]> {
        const params = new HttpParams();
        params.set("recommendationType", selectedType);
        return this.http.get<any[]>(`${this.apiUrl}`, { params });
    }
}