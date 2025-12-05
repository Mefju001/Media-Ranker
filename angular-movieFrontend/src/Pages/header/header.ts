import { Component } from '@angular/core';
import { RouterLink } from "@angular/router";
import { LoginDialog } from '../auth/login-dialog/login-dialog';
import { MatDialog, MatDialogModule} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../Services/AuthService';

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
openLoginDialog(): void {
      this.dialog.open(LoginDialog, {
          width: '400px',
          data: { title: 'Witaj, Zaloguj się' } 
      }).afterClosed().subscribe(credentials => {
          if (credentials) {
              this.authService.login(credentials).subscribe({
                  next: (response) => {
                      this.isLoggedIn = true;
                      this.username = credentials.username;
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
      this.isLoggedIn = false;
      this.username = '';
      console.log('User logged out');
  }
}
