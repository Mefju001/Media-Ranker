import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { UserService } from '../../Services/UserService';
import { UserDetailsRequest } from '../../Data/Request/UserDetailsRequest';

@Component({
  selector: 'app-change-details',
imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatDialogModule],  templateUrl: './change-details.html',
  styleUrl: './change-details.css',
})
export class ChangeDetails implements OnInit {
detailsForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    public dialogRef: MatDialogRef<ChangeDetails>,
    @Inject(MAT_DIALOG_DATA) public data: UserDetailsRequest
  ) {
    this.detailsForm = this.fb.group({
      name: [data?.name || '', Validators.required],
      surname: [data?.surname || '', Validators.required],
      email: [data?.email || '', [Validators.required, Validators.email]]
    });
  }
  ngOnInit(): void {
    this.userService.getMyDetails().subscribe({
      next: (userDetails) => {
        this.detailsForm.patchValue({
          name: userDetails.name,
          surname: userDetails.surname,
          email: userDetails.email
        });
      },
      error: (err) => alert('Błąd podczas pobierania danych użytkownika: ' + (err?.error?.message || 'Nieznany błąd'))
    });
  }

  onSubmit() {
    if (this.detailsForm.valid) {
      this.userService.changeDetails(this.detailsForm.value).subscribe({
        next: () => {
          alert('Dane zaktualizowane!');
          this.dialogRef.close(true);
        },
        error: (err) => alert('Błąd: ' + (err?.error?.message || 'Nieznany błąd'))
      });
    }
  }
}
