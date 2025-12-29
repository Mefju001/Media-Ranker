import { CanActivate, Router } from "@angular/router";
import { AuthService } from "../../../Services/AuthService";
import { Injectable } from "@angular/core";
@Injectable({
    providedIn: 'root'
})
export class Guard implements CanActivate {
constructor(private authService: AuthService, private router: Router) {}
canActivate(): boolean {
    const isLoggedIn = sessionStorage.getItem('isLoggedIn') === 'true';
if (isLoggedIn) {
      return true;
    } else {
      this.router.navigate(['/']); 
      return false;
    }
}
}
