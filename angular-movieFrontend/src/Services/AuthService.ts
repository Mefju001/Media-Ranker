import { HttpClient } from "@angular/common/http";
import { LoginRequest } from "../Data/Request/LoginRequest";
import { finalize, Observable, tap } from "rxjs";
import { LoginResponse } from "../Data/Response/LoginResponse";
import { computed, Injectable, signal } from "@angular/core";
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

    readonly currentUsername = signal<string | null>(null);
    readonly userRoles = signal<string[]>([]);
    readonly isLoggedIn = computed(() => !!this.accessToken && !!this.currentUsername());

    constructor(private http: HttpClient, private router: Router) {}
    getAccessToken(): string | null {
        return this.accessToken;
    }
    setAccessToken(token: string | null): void {
        this.accessToken = token;

        if (token) {
        this.decodeAndStoreTokenDetails(token);
        } else {
        this.currentUsername.set(null);
        this.userRoles.set([]);
        }
    }
    login(request:LoginRequest):Observable<LoginResponse> {
        return this.http.post<LoginResponse>(`${this.apiUrl}/Login`, request,{withCredentials: true}).pipe(tap(response => {
            this.setAccessToken(response.token);

        }));
    }
    logout(): Observable<void> {
        return this.http.post<void>(`${this.apiUrl}/Logout`,{ withCredentials: true }).pipe(finalize(() => {
        this.setAccessToken(null);
        this.router.navigate(['/movies']);
        })
    );
    }
    register(data:UserRegisterRequest):Observable<any>{
        return this.http.post<{message: string, token: string}>(`${this.apiUrl}/Register`, data,{withCredentials:true}).pipe(tap(response=>{
            this.setAccessToken(response.token)
        }));
    }
    refreshTokens():Observable<{accessToken: string}>{
        return this.http.post<{accessToken: string}>(this.refreshTokenUrl, {}, {withCredentials:true}).pipe(tap(response=>{
            this.setAccessToken(response.accessToken);           
        }));
    }
    private decodeAndStoreTokenDetails(token: string): void {
    try {
      const decoded: any = jwtDecode(token);

      const username = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] 
                    || decoded.name 
                    || decoded.unique_name;

      const roles = decoded.role 
                 || decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] 
                 || [];

      this.currentUsername.set(username ?? null);
      this.userRoles.set(Array.isArray(roles) ? roles : [roles]);
    } catch {
      this.setAccessToken(null);
    }
  }
}