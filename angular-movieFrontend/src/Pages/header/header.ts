import { Component } from '@angular/core';
import { RouterLink } from "@angular/router";
import { LoginDialog } from '../auth/login-dialog/login-dialog';
import { MatDialog, MatDialogModule} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

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
    private dialog: MatDialog
  ) {}
openLoginDialog(): void {
      this.dialog.open(LoginDialog, {
          width: '380px',
          data: { title: 'Witaj, Zaloguj się' } 
      }).afterClosed().subscribe(credentials => {
          if (credentials) {
              console.log(credentials.username);
              console.log('Dane logowania otrzymane:', credentials);
              // Tutaj wywołanie usługi autoryzacji
          }
      });
  }
}
