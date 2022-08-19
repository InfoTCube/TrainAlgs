import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { colorSets } from '@swimlane/ngx-charts';
import * as moment from 'moment';
import { Member } from 'src/app/models/member';
import { AccountService } from 'src/app/services/account.service';
import { MembersService } from 'src/app/services/members.service';
import { theme } from './theme';

const monthName = new Intl.DateTimeFormat("en-us", { month: "short" });
const weekdayName = new Intl.DateTimeFormat("en-us", { weekday: "short" });

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.scss']
})
export class MemberDetailComponent implements OnInit {
  member: Member;
  currentUserUsername: string;
  calendarData: any[] = [];
  colorScheme: any;
  heatmapMin: number = 0;
  heatmapMax: number = 10;

  constructor(private membersService: MembersService, private route: ActivatedRoute, private accoutService: AccountService) { }

  ngOnInit() {
    this.loadMember();
    this.colorScheme = theme;
  }

  loadMember() {
    this.accoutService.currentUser$.subscribe(user => {
      this.currentUserUsername = user.username;
    });

    this.membersService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member => {
      this.member = member;
      this.calendarData = this.getCalendarData();
    })
  }

  calendarAxisTickFormatting(mondayString: string) {
    const monday = new Date(mondayString);
    const month = monday.getMonth();
    const day = monday.getDate();
    const year = monday.getFullYear();
    const lastSunday = new Date(year, month, day - 1);
    const nextSunday = new Date(year, month, day + 6);
    return lastSunday.getMonth() !== nextSunday.getMonth() ? monthName.format(nextSunday) : '';
  }

  calendarTooltipText(c): string {
    return `
      <span class="tooltip-label">${c.label} • ${c.cell.date.toLocaleDateString()}</span>
      <span class="tooltip-val">${c.data.toLocaleString()}</span>
    `;
  }

  getCalendarData(): any[] {
    var maxValue = 0;
    // today
    const now = new Date();
    const todaysDay = now.getDate();
    const thisDay = new Date(now.getFullYear(), now.getMonth(), todaysDay);

    // Monday
    const thisMonday = new Date(thisDay.getFullYear(), thisDay.getMonth(), todaysDay - thisDay.getDay() + 1);
    const thisMondayDay = thisMonday.getDate();
    const thisMondayYear = thisMonday.getFullYear();
    const thisMondayMonth = thisMonday.getMonth();

    // 52 weeks before monday
    const calendarData = [];
    const getDate = d => new Date(thisMondayYear, thisMondayMonth, d);
    for (let week = -52; week <= 0; week++) {
      const mondayDay = thisMondayDay + week * 7;
      const monday = getDate(mondayDay);

      // one week
      const series = [];
      for (let dayOfWeek = 7; dayOfWeek > 0; dayOfWeek--) {
        const date = getDate(mondayDay - 1 + dayOfWeek);

        // skip future dates
        if (date > now) {
          continue;
        }

        // value
        const solutions = this.member.solutions.find(s => Object.values(s)[0] === moment(date).format('DD/MM/yyyy'));

        const value = solutions != undefined ? Object.values(solutions)[1] : 0;

        maxValue = value > maxValue ? value : maxValue;

        series.push({
          date,
          name: weekdayName.format(date),
          value: value
        });
      }

      calendarData.push({
        name: monday.toString(),
        series
      });
    }

    this.heatmapMax = maxValue;
    return calendarData;
  }

}
