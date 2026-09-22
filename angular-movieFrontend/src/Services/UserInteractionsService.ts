import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ETypeInteractions } from "../ClientPages/DetailsPages/Data/ETypeInteractions";
import { ERatingVote } from "../ClientPages/DetailsPages/Data/ERatingVote";
@Injectable({
  providedIn: 'root'
})
export class UserInteractionService {
  private apiUrl = 'http://localhost:5009/api/UserInteractions';
  constructor(private http: HttpClient) {}
  Get(typeInteractions?: string| null, ratingVote?: string| null): Observable<any[]> {
    let params = new HttpParams();
    if (typeInteractions) {
      params = params.set('typeInteractions', typeInteractions);
    }
    if (ratingVote) {
      params = params.set('ratingVote', ratingVote);
    }
    return this.http.get<any[]>(`${this.apiUrl}`, { params });
  }
  addUserInteraction(mediaId: string, typeInteractions?: ETypeInteractions|null, ratingVote?: ERatingVote|null): Observable<any> {
    const body: any = {
      mediaId: mediaId,
      typeInteractions: typeInteractions,
      ratingVote: ratingVote
    };
    return this.http.post<any>(`${this.apiUrl}`, body);
  }
}