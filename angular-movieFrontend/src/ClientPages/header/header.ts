import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { Router, RouterLink } from "@angular/router";
import { LoginDialog } from '../auth/login-dialog/login-dialog';
import { MatDialog, MatDialogModule} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../Services/AuthService';
import { ChangePassword } from '../change-password/change-password';
import { ChangeDetails } from '../change-details/change-details';
import { RegisterDialog } from '../auth/register-dialog/register-dialog';
import { UserService } from '../../Services/UserService';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink,
    MatDialogModule,
    MatButtonModule],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header{
  private authService = inject(AuthService);
  private userService = inject(UserService);
  private dialog = inject(MatDialog);
  private router = inject(Router);

  readonly isLoggedIn = this.authService.isLoggedIn;
  readonly username = this.authService.currentUsername;
  readonly isAdmin = computed(() => this.authService.userRoles().includes('Admin'));


  openLoginDialog(): void {
    this.dialog.open(LoginDialog, {
      width: '400px',
      data: { title: 'Witaj, Zaloguj się' } 
    }).afterClosed().subscribe(credentials => {
      if (credentials) {
        this.authService.login(credentials).subscribe({
          next: (response) => {
            this.router.navigate(['/movies']);
          },
          error: (error) => console.error('Login failed:', error)
        });
      }
    });
  }

  logout(): void {
    this.authService.logout().subscribe();
  }

  openChangePassword() { this.dialog.open(ChangePassword, { width: '400px' }); }
  openChangeDetails() { this.dialog.open(ChangeDetails, { width: '400px' }); }
  openRegisterDialog() { this.dialog.open(RegisterDialog, { width: '400px', data: { title: 'Proszę uzupełnić dane.' } }); }
  
  deleteAccount() {
    this.userService.deleteAccount().subscribe({
      next: () => {
        this.authService.setAccessToken(null);
        alert('Konto zostało usunięte.');
        this.router.navigate(['/']);
      },
      error: (err: unknown) => alert('Nie udało się usunąć konta.')
    });
  }
}