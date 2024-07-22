import { Injectable } from '@angular/core';

import { map, Observable } from 'rxjs';
import { AccountService } from '../services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard  {
  constructor(private accountService: AccountService) {}

  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(user => {
        if(user) return true;
        return false;
      })
    );
  }

}
