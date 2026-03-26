import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MatDialogContent, MatDialogActions, MatDialogModule } from '@angular/material/dialog';
import { UserService } from '../../Services/UserService';
import { MatFormField, MatError, MatLabel, MatFormFieldModule } from "@angular/material/form-field";
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
@Component({
  selector: 'app-change-password',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDialogModule
  ],
  templateUrl: './change-password.html',
  styleUrl: './change-password.css',
})
export class ChangePassword {
passwordForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    public dialogRef: MatDialogRef<ChangePassword>
  ) {
    this.passwordForm = this.fb.group({
      oldPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator });
  }


  passwordMatchValidator(g: FormGroup) {
    return g.get('newPassword')?.value === g.get('confirmPassword')?.value
      ? null : { 'mismatch': true };
  }

  onSubmit() {
    if (this.passwordForm.valid) {
      this.userService.changePassword(this.passwordForm.value).subscribe({
        next: () => {
          alert('Hasło zostało zmienione!');
          this.dialogRef.close();
        },
        error: (err) => alert('Błąd: ' + err.error.message)
      });
    }
  }
}
