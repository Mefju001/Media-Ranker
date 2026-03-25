import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ReviewResponse } from "../Data/Response/ReviewResponse";
import { Injectable } from "@angular/core";
@Injectable({
  providedIn: 'root' 
})
export class ReviewService {
    private apiUrl = 'http://localhost:5009/Review';
constructor(private http: HttpClient) {}
getTheLastestReviews(): Observable<String[]> {
    return this.http.get<String[]>(`${this.apiUrl}/TheLatest`);
    }
}