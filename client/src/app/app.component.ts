import { Component, OnInit } from '@angular/core';
import { delay } from 'rxjs';
import { User } from './models/user';
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'client';
  theme = localStorage.theme;

  ngOnInit(): void {
    this.setCurrentUser();
  }

  constructor(private accountService: AccountService) {}

  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user'));
    this.accountService.setCurrentUser(user);
  }

  changeTheme(theme: string) {
    this.theme = theme;
  }
}
