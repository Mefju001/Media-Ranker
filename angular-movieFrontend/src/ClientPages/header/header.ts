import { Component, inject, OnInit, signal } from '@angular/core';
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
export class Header implements OnInit {
  private authService = inject(AuthService);
  private userService = inject(UserService);
  private dialog = inject(MatDialog);
  private router = inject(Router);

  isLoggedIn = signal<boolean>(this.authService.isLoggedIn());
  username = signal<string>(this.authService.currentUsername() || '');
  userRole = signal<boolean>(false);


  ngOnInit() {
    const token = this.authService.getAccessToken();
    if (token) {
      this.isLoggedIn.set(true);
      this.username.set(this.authService.currentUsername() || 'Użytkownik');
      this.userRole.set(this.authService.userRoles().includes('Admin'));
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
            this.isLoggedIn.set(true);
            this.username.set(this.authService.currentUsername() || 'Użytkownik');
            this.userRole.set(this.authService.userRoles().includes('Admin'));
            this.router.navigate(['/movies']);
          },
          error: (error) => console.error('Login failed:', error)
        });
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.isLoggedIn.set(false);
    this.username.set('');
    this.userRole.set(false);
    this.router.navigate(['/']);
  }

  openChangePassword() { this.dialog.open(ChangePassword, { width: '400px' }); }
  openChangeDetails() { this.dialog.open(ChangeDetails, { width: '400px' }); }
  openRegisterDialog() { this.dialog.open(RegisterDialog, { width: '400px', data: { title: 'Proszę uzupełnić dane.' } }); }
  
  deleteAccount() {
    this.userService.deleteAccount().subscribe({
      next: () => {
        this.isLoggedIn.set(false);
        this.username.set('');
        this.router.navigate(['/']);
      },
      error: (err:any) => alert('Nie udało się usunąć konta.')
    });
  }
}