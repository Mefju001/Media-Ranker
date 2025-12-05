import { HttpClient } from "@angular/common/http";
import { LoginRequest } from "../Data/Request/LoginRequest";
import { Observable } from "rxjs";
import { LoginResponse } from "../Data/Response/LoginResponse";
import { Injectable } from "@angular/core";
@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = 'http://localhost:5009/api';
    constructor(private http: HttpClient) {}
    login(request:LoginRequest):Observable<LoginResponse> {
        return this.http.post<LoginResponse>(`${this.apiUrl}/login`, request);
    }
}