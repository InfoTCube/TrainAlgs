import { AfterViewInit, Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { createPopper } from '@popperjs/core';
import { filter } from 'rxjs';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit, AfterViewInit {
  @Output() themeChangedEvent = new EventEmitter<string>();
  dropdownPopoverShow: boolean = false;
  @ViewChild("btnDropdownRef", { static: false }) btnDropdownRef: ElementRef;
  @ViewChild("popoverDropdownRef", { static: false }) popoverDropdownRef: ElementRef;
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

  ngAfterViewInit(): void {
    createPopper(
      this.btnDropdownRef.nativeElement,
      this.popoverDropdownRef.nativeElement,
      {
        placement: "bottom-end",
      }
    );
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/sign_in');
  }

  goToProfile() {
    let username;
    this.accountService.currentUser$.subscribe(
      user => {
        username = user.username;
      }
    )
    this.router.navigateByUrl(`/members/${username}`);
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

