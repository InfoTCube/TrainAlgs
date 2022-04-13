import { ChangeDetectorRef, Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { delay, Observable } from 'rxjs';
import { PaginatedResult, Pagination } from 'src/app/Models/pagination';
import { Task } from 'src/app/Models/task';
import { TaskParams } from 'src/app/Models/taskParams';
import { TasksService } from 'src/app/services/tasks.service';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.scss']
})
export class TasksComponent implements OnInit {
  tasks: Task[];
  pagination: Pagination;
  taskParams: TaskParams = new TaskParams();

  constructor(private tasksService: TasksService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks() {
    this.tasksService.getTasks(this.taskParams).subscribe(response => {
      this.tasks = response.result;
      this.pagination = response.pagination;
      console.log(this.pagination)
    })
  }
}
