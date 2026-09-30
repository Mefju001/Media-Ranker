import { inject } from "@angular/core";
import { AuthService } from "../../../Services/AuthService";
import { CanActivateFn, Router } from "@angular/router";

export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.userRoles().includes('Admin')) {
    return true;
  }
  router.navigate(['/movies']);
  return false; 
};