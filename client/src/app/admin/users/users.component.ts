import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { debounceTime, distinctUntilChanged, filter, fromEvent, map } from 'rxjs';
import { ListingParams } from 'src/app/models/listingParams';
import { Pagination } from 'src/app/models/pagination';
import { SearchedMember } from 'src/app/models/searchedMember';
import { AccountService } from 'src/app/services/account.service';
import { AdminService } from 'src/app/services/admin.service';
import { MembersService } from 'src/app/services/members.service';
import { PresenceService } from 'src/app/services/presence.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  @ViewChild('usersSearchInput', { static: true }) usersSearchInput: ElementRef;
  isSearching: boolean;
  listingParams: ListingParams;
  users: SearchedMember[];
  pagination: Pagination;
  currentUserUsername: string;
  searchText: string;
  confirm: boolean;
  currentUsername: string;

  constructor(private membersService: MembersService, private adminService: AdminService,
    private accountService: AccountService, public presenceService: PresenceService) { 
    this.listingParams = this.membersService.getListingParams();
    this.users = [];
    
  }

  ngOnInit(): void {

    this.accountService.currentUser$.subscribe(user => {
      this.currentUserUsername = user.username;
    });

    fromEvent(this.usersSearchInput.nativeElement, 'keyup').pipe(

      // get value
      map((event: any) => {
        return event.target.value;
      })
      // if character length greater then 2
      , filter(res => res.length > 2)

      // Time in milliseconds between key events
      , debounceTime(1000)

      // If previous query is diffent from current   
      , distinctUntilChanged()

      // subscription for response
    ).subscribe((text: string) => {
      this.searchText = text;
      this.searchMembers(text);
    });
  }

  pageChanged(event: any) {
    this.listingParams.pageNumber = event;
    this.membersService.setListingParams(this.listingParams);
    this.searchMembers(this.searchText);
  }

  grantModerator($event) {
    $event.stopPropagation();
    this.adminService.assignModerator(this.currentUsername).subscribe(out => {
      this.searchMembers(this.searchText);
      this.toggleConfirm($event, "");
    });
  }

  toggleConfirm($event, username: string) {
    $event.stopPropagation();
    this.currentUsername = username;
    this.confirm = !this.confirm;
  }

  searchMembers(textToSearch: string) {
    this.membersService.searchMembers(this.listingParams, textToSearch).subscribe((response) => {
      this.users = response.result;
      this.pagination = response.pagination;
    }, (err) => {});
  }
}