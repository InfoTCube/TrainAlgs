import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TasksService } from 'src/app/services/tasks.service';

@Component({
  selector: 'app-task-adding',
  templateUrl: './task-adding.component.html',
  styleUrls: ['./task-adding.component.scss']
})
export class TaskAddingComponent implements OnInit {
  content: string = "";
  markdown: string = "";
  addTaskForm: FormGroup;
  validationErrors: string[] = [];
  tab='editor';

  constructor(private router: Router, private tasksService: TasksService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  getCode(content: string) {
    this.content = content;
    this.markdown = this.replaceInlineCode(content);
  }

  replaceInlineCode(md: string) : string {
    let matchString = '.';
    while(matchString != null) {
      let match = md.match(/(?<!``)(?=`)`(?!`)[^`]*(?=`)`(?!`)/);
      if(match == null) {
        matchString = null;
        continue;
      }
      md = `${md.substring(0, match.index)}<span class="bg-gray-400 dark:bg-gray-700 p-1 rounded-md">${match[0].substring(1, match[0].length-1)}</span>${md.substring(match.index+match[0].length)}`;
    }
    return md;
  }

  addTask() {
    var model = this.addTaskForm.value;
    model.content = this.addTaskForm.get("content").value;
    this.tasksService.addTask(model).subscribe(response => {
      this.router.navigateByUrl(`/tasks`);
    });
  }

  changeTab(name: string) {
    this.tab = name;
  }

  initializeForm() {
    this.addTaskForm = new FormGroup({
      taskName: new FormControl('', Validators.required),
      content: new FormControl('', Validators.required),
    });
  }
}
