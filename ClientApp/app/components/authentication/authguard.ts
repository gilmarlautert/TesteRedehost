
  import { Injectable } from '@angular/core';
  import { CanActivate } from "@angular/router";
  import { AuthenticationService } from './authentication.service'
  
  @Injectable()
  export class AuthGuard implements CanActivate {
      constructor(private authService: AuthenticationService) { }
    
      canActivate() {
        if (this.authService.isLoggedIn()) {
          return true;
        }
        return false;
      }
    }