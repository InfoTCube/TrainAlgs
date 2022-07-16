import { AfterViewInit, Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { createPopper, placements } from '@popperjs/core';
import { filter, Observable } from 'rxjs';
import { User } from '../Models/user';
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
    this.toggleDropdown(event);
    this.router.navigateByUrl('/sign_in');
  }

  toggleDropdown(event) {
    event.preventDefault();
    if(this.dropdownPopoverShow) {
      this.dropdownPopoverShow = false;
    } else {
      this.dropdownPopoverShow = true;
    }
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

