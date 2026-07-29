import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
@Injectable({
    providedIn: 'root'
})
export class AdminService {
  private apiUrl = 'http://localhost:5009/api/Admin';
  constructor(private http: HttpClient) {}
  getAllNumbers(): Observable<{ numberOfMovies: number; numberOfTvSeries: number; numberOfGames: number }> {
    return this.http.get<{numberOfGames: number ; numberOfMovies: number; numberOfTvSeries: number}>(`${this.apiUrl}`);
  }
  getAllUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Users`);
  }
  deleteUser(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/User/${id}`);
  }
  editUser(id: string, userData: any): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/users/${id}`, userData);
  }
  editPassword(id: string, passwordData: any): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/users/${id}/Password`, passwordData);
  }
}
