import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ListedSolution } from 'src/app/Models/listedSolutions';
import { ListingParams } from 'src/app/Models/listingParams';
import { Pagination } from 'src/app/Models/pagination';
import { SolutionsService } from 'src/app/services/solutions.service';

@Component({
  selector: 'app-solutions',
  templateUrl: './solutions.component.html',
  styleUrls: ['./solutions.component.scss']
})
export class SolutionsComponent implements OnInit {
  solutions: ListedSolution[];
  pagination: Pagination;
  solutionParams: ListingParams;
  @Input() taskTag: string = "";

  constructor(private solutionsService: SolutionsService, private router: Router) { 
    this.solutionParams = this.solutionsService.getSolutionParams();
  }

  ngOnInit(): void {
    if(this.taskTag == "")
      this.loadAllSolutions();
    else
      this.loadSolutionsForTask();
  }

  async loadAllSolutions() {
    this.solutionsService.getSolutionsForCurrentUser(this.solutionParams).subscribe(response => {
      this.pagination = response.pagination;
      this.solutions = response.result;
      console.log(this.pagination)
    });
  }

  async loadSolutionsForTask() {
    this.solutionsService.getSolutionsForCurrentUserAndTask(this.solutionParams, this.taskTag).subscribe(response => {
      this.solutions = response.result;
      this.pagination = response.pagination;
      console.log(this.pagination)
    })
  }

  solutionPage(id: number) {
    this.router.navigateByUrl('/solutions/' + id);
  }

  pageChanged(event: any) {
    this.solutionParams.pageNumber = event;
    console.log(event)
    this.solutionsService.setSolutionParams(this.solutionParams);
    if(this.taskTag == "")
      this.loadAllSolutions();
    else
      this.loadSolutionsForTask();
  }

}
