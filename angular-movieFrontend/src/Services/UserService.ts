import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ChangePassword } from '../Data/Request/ChangePassword';
import { UserDetailsRequest } from '../Data/Request/UserDetailsRequest';

@Injectable({
    providedIn: 'root'
})
export class UserService {
    private apiUrl = 'http://localhost:5009/api/User';

    constructor(private http: HttpClient) { }

    getUsers(): Observable<any[]> {
        return this.http.get<any[]>(this.apiUrl);
    }
    getMyDetails(): Observable<any> {
        return this.http.get<any>(`${this.apiUrl}`);
    }
    getUserById(id: string): Observable<any> {
        return this.http.get<any>(`${this.apiUrl}/${id}`);
    }
    changePassword(Data: ChangePassword): Observable<any>
    {
        return this.http.patch<any>(`${this.apiUrl}/Change/Password`,Data);
    }
    changeDetails(Data:UserDetailsRequest):Observable<any>
    {
        return this.http.patch<any>(`${this.apiUrl}/Change/Details`,Data);
    }
    deleteAccount(){
        return this.http.delete<any>(`${this.apiUrl}`);
    }
}