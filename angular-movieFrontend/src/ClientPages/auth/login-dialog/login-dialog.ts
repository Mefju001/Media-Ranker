import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  //standalone: true,
  selector: 'app-login-dialog',
  imports: [
    MatDialogModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    CommonModule,
    MatInputModule,
    MatButtonModule ],
  templateUrl: './login-dialog.html',
  styleUrl: './login-dialog.css',
})
export class LoginDialog implements OnInit {
  loginForm!: FormGroup;
  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<LoginDialog>, 
    @Inject(MAT_DIALOG_DATA) public data: { title: string }
  ) {}
  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: [''],
      password: [''] 
    });
  }
  onLoginSubmit(): void {
    if (this.loginForm.valid) {
      const credentials = this.loginForm.value;
      console.log('Submitting login with credentials:', credentials);
      this.dialogRef.close(credentials);
    }
  }

  onCancel(): void {
    this.dialogRef.close(null); 
  }
}
