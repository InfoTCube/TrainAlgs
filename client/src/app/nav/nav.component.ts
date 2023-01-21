import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  @Output() themeChangedEvent = new EventEmitter<string>();
  page = "/";
  theme = 'light';

  constructor(public accountService: AccountService, private router: Router, private translate: TranslateService) {
    router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.page = event.url.split("/").slice(0, 2).join('/');
    });
  }

  ngOnInit(): void {
    localStorage.theme = localStorage.theme == null ? 'light' : localStorage.theme;
    this.theme = localStorage.theme;
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/sign_in');
  }

  goToProfile() {
    this.accountService.currentUser$.subscribe(user => {
      this.router.navigateByUrl('/', {skipLocationChange: true}).then(()=>
      this.router.navigateByUrl(`/members/${user.username}`));
    });
  }

  takeAction($event) {

    if($event == this.translate.instant("Menu.Profile")) this.goToProfile();
    else if($event == this.translate.instant("Menu.SignOut")) this.logout();
  }

  themeChanged() {
    if(localStorage.theme == 'dark') {
      localStorage.theme = 'light';
    } else {
      localStorage.theme = 'dark';
    }
    this.theme = localStorage.theme;
    this.themeChangedEvent.emit(this.theme);
  }
}

