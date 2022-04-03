import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { createPopper, placements } from '@popperjs/core';
import { Observable } from 'rxjs';
import { User } from '../Models/user';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit, AfterViewInit {
  dropdownPopoverShow: boolean = false;
  @ViewChild("btnDropdownRef", { static: false }) btnDropdownRef: ElementRef;
  @ViewChild("popoverDropdownRef", { static: false }) popoverDropdownRef: ElementRef;

  constructor(public accountService: AccountService, private router: Router) { }

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
}
