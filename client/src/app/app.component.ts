import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { User } from './models/user';
import { AccountService } from './services/account.service';
import { PresenceService } from './services/presence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  theme = localStorage.theme;
  lang = localStorage.lang;

  constructor(private accountService: AccountService, private presenceService: PresenceService, private translate: TranslateService) {}

  ngOnInit(): void {
    this.setCurrentUser();
    this.setLang();
  }

  setCurrentUser() {
    var user: User = JSON.parse(sessionStorage.getItem('user'));
    if(!user) user = JSON.parse(localStorage.getItem('user'));
    if(user) {
      this.accountService.setCurrentUser(user, false);
      this.presenceService.createHubConnection(user);
    }
  }

  changeTheme(theme: string) {
    this.theme = theme;
  }

  changeLang(lang: string) {
    this.lang = lang;
    this.setLang();
  }

  setLang() {
    switch(this.lang) {
      case 'gb':
        this.translate.use('en');
        break;
      case 'pl':
        this.translate.use('pl');
        break;
    }
  }
}
