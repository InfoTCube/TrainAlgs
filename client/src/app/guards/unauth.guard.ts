import { Injectable } from '@angular/core';

import { AccountService } from '../services/account.service';

@Injectable({
  providedIn: 'root'
})
export class UnauthGuard  {
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
