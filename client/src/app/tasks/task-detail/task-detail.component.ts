import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Task } from 'src/app/Models/task';
import { SolutionsService } from 'src/app/services/solutions.service';
import { TasksService } from 'src/app/services/tasks.service';

@Component({
  selector: 'app-task-detail',
  templateUrl: './task-detail.component.html',
  styleUrls: ['./task-detail.component.scss']
})
export class TaskDetailComponent implements OnInit {
  task: Task;
  markdown = '';
  tab='problemStatement';
  addSolutionForm: FormGroup;
  submitSolutionForm: FormGroup;
  validationErrors: string[] = [];
  fileContent: string = "";

  constructor(private route: ActivatedRoute, private tasksService: TasksService, 
    private solutionsService: SolutionsService, private router: Router) { }

  ngOnInit(): void {
    this.getTask();
    this.initializeForm();
  }

  getTask() {
    this.tasksService.getTask(this.route.snapshot.paramMap.get('nameTag')).subscribe(response => {
      this.task = response;
      this.markdown = this.replaceInlineCode(response.content);
      console.log(response.exampleTestGroup.tests)
    })
  }

  submitSolution() {
    var model = this.submitSolutionForm.value;
    model.code = this.fileContent == "" ? this.submitSolutionForm.get("code").value : this.fileContent;
    model.language = this.submitSolutionForm.get("language").value;
    model.algTaskTag = this.task.nameTag;
    delete model.file
    console.log(model);
    this.solutionsService.addSolution(model).subscribe(response => {
      this.router.navigateByUrl(`/solutions`);
    });
  }

  replaceInlineCode(md: string) : string {
    let matchString = '.';
    while(matchString != null) {
      let match = md.match(/(?=`)`(?!`)[^`]*(?=`)`(?!`)/);
      if(match == null) {
        matchString = null;
        continue;
      }
      md = `${md.substring(0, match.index)}<span class="bg-gray-300 dark:bg-gray-700 p-1 rounded-md">${match[0].substring(1, match[0].length-1)}</span>${md.substring(match.index+match[0].length)}`;
    }
    return md;
  }

  changeTab(name: string) {
    this.tab = name;
  }

  initializeForm() {
    this.submitSolutionForm = new FormGroup({
      file: new FormControl(''),
      code: new FormControl('', Validators.required),
      language: new FormControl(''),
    });
    this.submitSolutionForm.get("language").setValue("C++");
    this.submitSolutionForm.get("file").valueChanges.subscribe(filename => {
      if(filename != "") {
        console.log(filename);
        this.submitSolutionForm.get("code").disable();
        let ext = filename.split('.').pop();
        if(ext == "py")
          this.submitSolutionForm.get("language").setValue("PYTHON");
        else if(ext == "cpp" || ext == "cxx" || ext == "cc")
          this.submitSolutionForm.get("language").setValue("C++");

        this.submitSolutionForm.get("language").disable();
      } else {
        this.submitSolutionForm.get("code").enable();
        this.submitSolutionForm.get("language").enable();
      }
    });
  }

  async onFileChange(event) {
    const file:File = event.target.files[0];

    if(file) {
      const text = await file.text();
      this.fileContent = text;
    } else {
      this.fileContent = "";
    }
  }
}
