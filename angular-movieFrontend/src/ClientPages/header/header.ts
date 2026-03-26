import { Component } from '@angular/core';
import { RouterLink } from "@angular/router";
import { LoginDialog } from '../auth/login-dialog/login-dialog';
import { MatDialog, MatDialogModule} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../Services/AuthService';
import { ChangePassword } from '../change-password/change-password';
import { ChangeDetails } from '../change-details/change-details';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink,
    MatDialogModule,
    MatButtonModule],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header {
  isLoggedIn: boolean = false;
  username: string = '';
  constructor(
    private dialog: MatDialog,
    private authService: AuthService
  ) {}
openChangePassword() {
  this.dialog.open(ChangePassword, {
    width: '400px',
  });
}
openChangeDetails() {
  this.dialog.open(ChangeDetails, {
    width: '400px',
  });
}
openLoginDialog(): void {
      this.dialog.open(LoginDialog, {
          width: '400px',
          data: { title: 'Witaj, Zaloguj się' } 
      }).afterClosed().subscribe(credentials => {
          if (credentials) {
              this.authService.login(credentials).subscribe({
                  next: (response) => {
                      this.isLoggedIn = true;
                      this.storeLoginState(true);
                      this.username = credentials.username;
                      sessionStorage.setItem('username', this.username);
                      console.log('Login successful:', response);
                  }
              ,error: (error) => {
                      console.error('Login failed:', error);
                  }
              });
  }
      });
  }
  logout(): void {
    this.authService.logout();
    this.isLoggedIn = false;
    this.storeLoginState(false);
    this.username = '';
    sessionStorage.removeItem('username');
    console.log('User logged out');
  }
  private storeLoginState(isLoggedIn: boolean): void {
    sessionStorage.setItem('isLoggedIn', isLoggedIn ? 'true' : 'false');
  }
}
