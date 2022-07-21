import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
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
  theme = localStorage.theme;

  constructor(public accountService: AccountService, private router: Router) {
    router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.page = event.url.split("/").slice(0, 2).join('/');
    });
  }

  ngOnInit(): void {
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
    if($event == "Profile") this.goToProfile();
    else if($event == "Sign out") this.logout();
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

