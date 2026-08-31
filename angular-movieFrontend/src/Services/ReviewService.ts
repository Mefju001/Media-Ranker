import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ReviewResponse } from "../Data/Response/ReviewResponse";
import { Injectable } from "@angular/core";
import { ReviewRequest } from "../Data/Request/ReviewRequest";
@Injectable({
  providedIn: 'root' 
})
export class ReviewService {
    private apiUrl = 'http://localhost:5009/api/Review';
    constructor(private http: HttpClient) {}
    getTheLastestReviews(): Observable<string[]> {
        return this.http.get<string[]>(`${this.apiUrl}/TheLatest`);
        }
    addReview(mediaId:string, result:ReviewRequest):Observable<any>{
        const payload = {
            movieId:mediaId,
            rating:result.Rating,
            comment:result.Comment
        }
        return this.http.post<any>(`${this.apiUrl}`,payload)
    }
}