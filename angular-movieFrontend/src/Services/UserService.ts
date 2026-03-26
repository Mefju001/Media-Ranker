import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UUID } from 'crypto';
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

    getUserById(id: UUID): Observable<any> {
        return this.http.get<any>(`${this.apiUrl}/${id}`);
    }
    changePassword(Data: ChangePassword): Observable<any>
    {
        return this.http.patch<any>(`${this.apiUrl}/Change/Password`,Data);
    }
    changeDetails(Data:UserDetailsRequest):Observable<any>
    {
        return this.http.patch<any>(`${this.apiUrl}/Change/Details`,Data)
    }
}