import { HttpClient } from "@angular/common/http";
import { LoginRequest } from "../Data/Request/LoginRequest";
import { finalize, Observable, tap } from "rxjs";
import { LoginResponse } from "../Data/Response/LoginResponse";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = 'http://localhost:5009/api';
    public accessToken: string | null = null;
    constructor(private http: HttpClient, private router: Router) {}
    getAccessToken(): string | null {
        return this.accessToken;
    }
    setAccessToken(token: string | null): void {
        this.accessToken = token;
        this.storeAccessToken(token!);
    }
    storeAccessToken(token: string): void {
        sessionStorage.setItem('accessToken', token);
    }
    clearAccessToken(): void {
        this.accessToken = null;
        sessionStorage.removeItem('accessToken');

    }
    login(request:LoginRequest):Observable<LoginResponse> {
        return this.http.post<LoginResponse>(`${this.apiUrl}/login`, request,{withCredentials: true}).pipe(tap(response => {
            this.setAccessToken(response.token);
        }));
    }
    logout(): Observable<String> {
        return this.http.post<String>(`${this.apiUrl}/logout`,{}).pipe(finalize(() => {
            this.clearAccessToken();
            sessionStorage.removeItem('isLoggedIn');
            sessionStorage.removeItem('username');
        }));
    }
}