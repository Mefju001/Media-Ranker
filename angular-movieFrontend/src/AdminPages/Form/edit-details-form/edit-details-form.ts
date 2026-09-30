import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ChangeDetails } from '../../../ClientPages/change-details/change-details';
import { UserDetailsRequest } from '../../../Data/Request/UserDetailsRequest';
import { UserService } from '../../../Services/UserService';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { AdminService } from '../../../Services/AdminService';

@Component({
  selector: 'app-edit-details-form',
  imports: [MatButtonModule, MatCardModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule],
  templateUrl: './edit-details-form.html',
  styleUrl: './edit-details-form.css',
})
export class EditDetailsForm {
  detailsForm!: FormGroup;
  userId!: string;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private userService: UserService,
    private adminService: AdminService,
  ) { }

  ngOnInit(): void {
    this.detailsForm = this.fb.group({
      name: ['', Validators.required],
      surname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
    const idFromRoute = this.route.snapshot.paramMap.get('id');

    if (idFromRoute) {
      this.userId = idFromRoute;
    this.userService.getUserById(this.userId).subscribe({
      next: (userData) => {
        if (userData) {
          this.detailsForm.patchValue(userData);
        }
      },
      error: (err) => console.error('Błąd pobierania danych:', err)
    });
  }
  }

  onSubmit(): void {
    if (this.detailsForm.valid) {
      console.log('Wysyłanie danych:', this.detailsForm.value);
      this.adminService.editUser(this.userId, this.detailsForm.value).subscribe({
        next: () => {
          alert('Dane zaktualizowane!');
          this.router.navigate(['/adminDashboard']);
        },
        error: (err) => alert('Błąd: ' + (err?.error?.message || 'Nieznany błąd'))
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/adminDashboard']);
  }
}
