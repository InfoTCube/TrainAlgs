import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TestGroup } from 'src/app/models/testGroup';
import { CodeEditorComponent } from 'src/app/modules/code-editor/code-editor.component';
import { TasksService } from 'src/app/services/tasks.service';

@Component({
  selector: 'app-task-adding',
  templateUrl: './task-adding.component.html',
  styleUrls: ['./task-adding.component.scss']
})
export class TaskAddingComponent implements OnInit {
  @ViewChild('codeEditor') codeEditor: CodeEditorComponent;
  content: string = "";
  markdown: string = "";
  addTaskForm: FormGroup;
  validationErrors: string[] = [];
  tab='editor';
  testGroups: TestGroup[] = [];

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
    model.content = this.content;
    this.saveInputsAndOutputs();
    model.testGroups = this.testGroups;
    console.log(model);
    this.tasksService.addTask(model).subscribe(response => {
      this.router.navigateByUrl(`/tasks`);
    });
  }

  changeTab(name: string) {
    this.tab = name;
    if(name == "editor") {
      setTimeout(() => {
        this.codeEditor.setCode(this.content);
        this.loadInputsAndOutputs();
      }, 0);
    } else if(name == "preview") {
      this.saveInputsAndOutputs();
    }
  }

  initializeForm() {
    this.addTaskForm = new FormGroup({
      name: new FormControl('', Validators.required),
      timeLimit: new FormControl('', Validators.required),
      memoryLimit: new FormControl('', Validators.required),
    });

    this.testGroups.push({number: 0, points: 0, tests: [{number: 1, input: "", output: ""}]});
    this.testGroups.push({number: 1, points: 0, tests: [{number: 1, input: "", output: ""}]});
  }

  addTestGroup() {
    this.testGroups.push({number: this.testGroups.length, points: 0, tests: [{number: 1, input: "", output: ""}]});
  }

  addTest(groupIndex: number) {
    this.testGroups[groupIndex].tests.push({number: this.testGroups[groupIndex].tests.length+1, input: "", output: ""});
  }

  deleteTestGroup(groupIndex: number) {
    this.testGroups.splice(groupIndex, 1);
    this.testGroups.forEach((element, index) => { element.number = index});
  }

  deleteTest(groupIndex: number, testIndex: number) {
    this.testGroups[groupIndex].tests.splice(testIndex, 1);
    this.testGroups[groupIndex].tests.forEach((element, index) => { element.number = index+1});
    this.testGroups.forEach((element, index) => { if(element.tests.length == 0) this.testGroups.splice(index, 1)});
    this.testGroups.forEach((element, index) => { element.number = index});
  }

  saveInputsAndOutputs() {
    this.testGroups.forEach((group) => {
      group.tests.forEach((test) => {
        test.input = (document.getElementById(`input${group.number}.${test.number}`) as HTMLInputElement).value;
        test.output = (document.getElementById(`output${group.number}.${test.number}`) as HTMLInputElement).value;
      });
      group.points = Number((document.getElementById(`points${group.number}`) as HTMLInputElement).value);
    });
  }

  loadInputsAndOutputs() {
    this.testGroups.forEach((group) => {
      group.tests.forEach((test) => {
        (document.getElementById(`input${group.number}.${test.number}`) as HTMLInputElement).value = test.input;
        (document.getElementById(`output${group.number}.${test.number}`) as HTMLInputElement).value = test.output;
      });
      (document.getElementById(`points${group.number}`) as HTMLInputElement).value = String(group.points);
    });
  }
}
