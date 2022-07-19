import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Solution } from 'src/app/models/solution';
import { TestSolutionResults } from 'src/app/models/testSolutionResults';
import { SolutionsService } from 'src/app/services/solutions.service';

@Component({
  selector: 'app-solution-detail',
  templateUrl: './solution-detail.component.html',
  styleUrls: ['./solution-detail.component.scss']
})
export class SolutionDetailComponent implements OnInit {
  solution: Solution;
  tests: TestSolutionResults[] = [];
  testsErrors: string[] = [];
  exampleTests: TestSolutionResults[] = [];
  exampleTestsErrors: string[] = [];
  showCode = false;
  language = "cpp";

  constructor(private solutionsService: SolutionsService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.getSolution();
    this.getLanguage();
  }

  getSolution() {
    this.solutionsService.getSolution(parseInt(this.route.snapshot.paramMap.get('id'))).subscribe(response => {
      this.solution = response;
      this.solution.testGroups.forEach(tg => {
        tg.tests.forEach(t => {
          var test: TestSolutionResults = {number: `${tg.number}.${t.number}`, time: t.time, memory: t.memory, timeLimit: t.timeLimit,
            memoryLimit: t.memoryLimit, status: t.status, error: t.error, points: null, maxPoints: null, showPoints: false, pointsLength: null};

          if(t.number == 1) {
            test.points = tg.points;
            test.maxPoints = tg.maxPoints;
            test.showPoints = true;
            test.pointsLength = tg.tests.length;
          }

          if(tg.number == 0)
            this.exampleTests.push(test);
          else
            this.tests.push(test);

          if(t.error && tg.number == 0)
            this.exampleTestsErrors.push(t.error);
          else if(t.error)
            this.testsErrors.push(t.error);
        });
      });
      console.log(response)
    });
  }

  getLanguage() {
    switch(this.solution.language) {
      case "C++":
        this.language = "cpp";
        break;
      case "Python":
        this.language = "python";
        break;
      default:
        this.language = "cpp";
        break;
    }
  }

  toggleCode() {
    this.showCode = !this.showCode;
  }

}
