import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../../Services/AuthService';

@Component({
  selector: 'app-register-dialog',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register-dialog.html',
  styleUrl: './register-dialog.css',
})
export class RegisterDialog {
  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      name: ['', Validators.required],
      surname: ['', Validators.required]
    });
  }

  onRegister() {
    if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value).subscribe({
        next: (res) => {
          alert('Zarejestrowano pomyślnie!');
          localStorage.setItem('token', res.token);
        },
        error: (err) => alert('Błąd rejestracji: ' + err.error.message)
      });
    }
  }
}
