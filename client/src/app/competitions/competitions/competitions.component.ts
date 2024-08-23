import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ListedCompetitions } from 'src/app/models/listedCompetitions';
import { ListingParams } from 'src/app/models/listingParams';
import { Pagination } from 'src/app/models/pagination';
import { AccountService } from 'src/app/services/account.service';
import { CompetitionsService } from 'src/app/services/competitions.service';

@Component({
  selector: 'app-competitions',
  templateUrl: './competitions.component.html',
  styleUrl: './competitions.component.scss'
})
export class CompetitionsComponent implements OnInit {
  comps: ListedCompetitions[];
  pagination: Pagination;
  compParams: ListingParams;
  now: number = (new Date()).valueOf();

  constructor(private competitionsService: CompetitionsService, private router: Router, private route: ActivatedRoute, public accountService: AccountService) {
    this.compParams = this.competitionsService.getCompParams();
    setInterval(() => {
      this.now = (new Date()).valueOf();
    }, 1000);
  }

  ngOnInit(): void {
    this.loadCompetitions();
  }

  async loadCompetitions() {
    this.competitionsService.getCompetitions(this.compParams).subscribe(response => {
      this.comps = response.result;
      this.pagination = response.pagination;
    })
  }

  compPage(id: number) {
    this.router.navigateByUrl('/competitions/' + id.toString());
  }

  pageChanged(event: any) {
    this.compParams.pageNumber = event;
    this.competitionsService.setCompParams(this.compParams);
    this.loadCompetitions();
  }

  statusCheck(startDate: Date, endDate: Date): number {
    const start = Date.parse(startDate.toString())
    const end = Date.parse(endDate.toString())
    if(start <= this.now && end >= this.now)
      return 1; //active
    else if(end < this.now)
      return 2; //past
    else if(start > this.now)
      return 3; //future
  }
}
