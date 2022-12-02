import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
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
  fileExtension = "cpp";
  fileUrl;


  constructor(private solutionsService: SolutionsService, private route: ActivatedRoute, private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.getSolution();
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
      this.getLanguage();
    });
  }

  getLanguage() {
    switch(this.solution?.language) {
      case "CPP":
        this.language = "cpp";
        this.fileExtension = "cpp";
        break;
      case "PYTHON":
        this.language = "python";
        this.fileExtension = "py";
        break;
      default:
        this.language = "cpp";
        this.fileExtension = "cpp";
        break;
    }
    const blob = new Blob([this.solution?.code], { type: 'application/octet-stream' });
    this.fileUrl = this.sanitizer.bypassSecurityTrustResourceUrl(window.URL.createObjectURL(blob));
  }

  toggleCode() {
    this.showCode = !this.showCode;
  }
}
