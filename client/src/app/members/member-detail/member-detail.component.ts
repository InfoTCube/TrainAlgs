import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Member } from 'src/app/models/member';
import { AccountService } from 'src/app/services/account.service';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.scss']
})
export class MemberDetailComponent implements OnInit {
  member: Member;
  currentUserUsername: string;

  constructor(private membersService: MembersService, private route: ActivatedRoute, private accoutService: AccountService) { }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    this.accoutService.currentUser$.subscribe(user => {
      this.currentUserUsername = user.username;
    });

    this.membersService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member => {
      this.member = member;
    })
  }

}
