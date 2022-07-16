import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Solution } from 'src/app/Models/solution';
import { SolutionsService } from 'src/app/services/solutions.service';

@Component({
  selector: 'app-solution-detail',
  templateUrl: './solution-detail.component.html',
  styleUrls: ['./solution-detail.component.scss']
})
export class SolutionDetailComponent implements OnInit {
  solution: Solution;

  constructor(private solutionsService: SolutionsService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.getSolution();
  }

  getSolution() {
    this.solutionsService.getSolution(parseInt(this.route.snapshot.paramMap.get('id'))).subscribe(response => {
      this.solution = response;
      console.log(this.solution);
    })
  }

}
