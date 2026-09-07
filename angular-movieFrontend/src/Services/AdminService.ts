import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UserResponse } from "../Data/Response/UserResponse";
import { MediaNumbersResponse } from "../Data/Response/MediaNumbersResponse";
@Injectable({
    providedIn: 'root'
})
export class AdminService {
  private apiUrl = 'http://localhost:5009/api/Admin';
  constructor(private http: HttpClient) {}
  getAllNumbers(): Observable<MediaNumbersResponse> {
    return this.http.get<MediaNumbersResponse>(`${this.apiUrl}`);
  }
  getAllUsers(): Observable<UserResponse[]> {
    return this.http.get<UserResponse[]>(`${this.apiUrl}/Users`);
  }
  deleteUser(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/User/${id}`);
  }
  editUser(id: string, userData: any): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/users/${id}`, userData);
  }
  generatePassword(id: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/users/${id}/generate-password`, {});
  }
}
