import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Task } from 'src/app/Models/task';
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

  constructor(private route: ActivatedRoute, private tasksService: TasksService) { }

  ngOnInit(): void {
    this.getTask();
  }

  getTask() {
    this.tasksService.getTask(this.route.snapshot.paramMap.get('nameTag')).subscribe(response => {
      this.task = response;
      this.markdown = this.replaceInlineCode(response.content);
      console.log(response.exampleTestGroup.tests)
    })
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
}
