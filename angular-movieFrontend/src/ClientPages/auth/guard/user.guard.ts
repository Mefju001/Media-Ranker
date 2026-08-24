import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../../../Services/AuthService";

export const userGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.getRolesFromToken().includes('User')) {
    return true;
  }
  router.navigate(['/movies']);
  return false; 
};