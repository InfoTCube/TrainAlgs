import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AccountService } from '../services/account.service';

@Injectable({
  providedIn: 'root'
})
export class UnauthGuard implements CanActivate {
  constructor(private accountService: AccountService) {}

  canActivate(): boolean {
    var user;
    this.accountService.currentUser$.subscribe(u => {
        user = u;
      }
    );

    if(user) return false;
    return true;
  }
}
