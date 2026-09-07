import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from "@angular/router";
import { LoginDialog } from '../auth/login-dialog/login-dialog';
import { MatDialog, MatDialogModule} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../Services/AuthService';
import { ChangePassword } from '../change-password/change-password';
import { ChangeDetails } from '../change-details/change-details';
import { RegisterDialog } from '../auth/register-dialog/register-dialog';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink,
    MatDialogModule,
    MatButtonModule],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header implements OnInit {
  isLoggedIn = false;
  username = '';
  userRole = false;
  userService: any;

  constructor(
    private authService: AuthService, 
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    const token = this.authService.getAccessToken();
    if (token) {
      this.isLoggedIn = true;
      this.username = this.authService.currentUsername() || 'Użytkownik';
      this.userRole = this.authService.userRoles().includes('Admin');
    }
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
            this.username = this.authService.currentUsername() || 'Użytkownik';
            this.userRole = this.authService.userRoles().includes('Admin');
            this.router.navigate(['/movies']);
          },
          error: (error) => console.error('Login failed:', error)
        });
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.isLoggedIn = false;
    this.username = '';
    this.userRole = false;
    this.router.navigate(['/']);
  }

  openChangePassword() { this.dialog.open(ChangePassword, { width: '400px' }); }
  openChangeDetails() { this.dialog.open(ChangeDetails, { width: '400px' }); }
  openRegisterDialog() { this.dialog.open(RegisterDialog, { width: '400px', data: { title: 'Proszę uzupełnić dane.' } }); }
  
  deleteAccount() {
    this.userService.deleteAccount().subscribe({
      next: () => {
        this.isLoggedIn = false;
        this.username = '';
        this.router.navigate(['/']);
      },
      error: (err:any) => alert('Nie udało się usunąć konta.')
    });
  }
}