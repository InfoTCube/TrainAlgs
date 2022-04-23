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

  constructor(private route: ActivatedRoute, private tasksService: TasksService) { }

  ngOnInit(): void {
    this.getTask();
  }

  getTask() {
    this.tasksService.getTask(this.route.snapshot.paramMap.get('nameTag')).subscribe(response => {
      this.task = response;
      this.markdown = response.contentUrl;
    })
  }

}
