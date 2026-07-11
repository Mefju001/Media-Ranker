import { HttpClient } from "@angular/common/http";
import { LoginRequest } from "../Data/Request/LoginRequest";
import { finalize, Observable, tap } from "rxjs";
import { LoginResponse } from "../Data/Response/LoginResponse";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { UserRegisterRequest } from "../Data/Request/UserRegisterRequest";
import { jwtDecode } from 'jwt-decode';
@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = 'http://localhost:5009/api/Account';
    private refreshTokenUrl = 'http://localhost:5009/api/Auth/RefreshToken';
    public accessToken: string | null = null;
    constructor(private http: HttpClient, private router: Router) {}
    getAccessToken(): string | null {
        return this.accessToken;
    }
    setAccessToken(token: string | null): void {
        this.accessToken = token;
       // this.storeAccessToken(token!);
    }
    storeAccessToken(token: string): void {
        sessionStorage.setItem('accessToken', token);
    }
    clearAccessToken(): void {
        this.accessToken = null;
        sessionStorage.removeItem('accessToken');

    }
    login(request:LoginRequest):Observable<LoginResponse> {
        return this.http.post<LoginResponse>(`${this.apiUrl}/Login`, request,{withCredentials: true}).pipe(tap(response => {
            this.setAccessToken(response.token);
        }));
    }
    logout(): Observable<String> {
        return this.http.post<String>(`${this.apiUrl}/Logout`,{}).pipe(finalize(() => {
            this.clearAccessToken();
            sessionStorage.removeItem('isLoggedIn');
            sessionStorage.removeItem('username');
        }));
    }
    register(data:UserRegisterRequest):Observable<any>{
        return this.http.post<{message: string, token: string}>(`${this.apiUrl}/Register`, data,{withCredentials:true}).pipe(tap(response=>{
            this.setAccessToken(response.token)
        }));
    }
    refreshTokens():Observable<{accessToken: string}>{
        console.log('1. Odpalam refreshTokens()... czekam na odpowiedź z .NET');
        return this.http.post<{accessToken: string}>(this.refreshTokenUrl, {}, {withCredentials:true}).pipe(tap(response=>{
            console.log('2. .NET odpowiedział! Otrzymany token:', response?.accessToken);
            this.setAccessToken(response.accessToken);
            this.getDetailsFromToken();
           
        }));
    }
    getDetailsFromToken():void{
        const token = this.getAccessToken();
        if (!token) {
            console.log('Brak tokenu w pamięci RAM – użytkownik jest gościem.');
            return;
        }
        const decodedToken: any = jwtDecode(token);

        const username = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] 
                     || decodedToken.name 
                     || decodedToken.unique_name;
                     
        const userId = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] 
                   || decodedToken.sub;

        if (username) {
            sessionStorage.setItem('username', username);
            sessionStorage.setItem('isLoggedIn', 'true');
        } else {
            console.warn('Nie można znaleźć nazwy użytkownika w tokenie.');
        }
    }
    getRolesFromToken(): string[] {
        const token = this.getAccessToken();
        if (!token) {
            return [];
        }
        const decodedToken: any = jwtDecode(token);
        const roles = decodedToken.role || decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || [];
        return Array.isArray(roles) ? roles : [roles];
    }
}