import { ChangeDetectorRef, Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { delay, Observable } from 'rxjs';
import { PaginatedResult, Pagination } from 'src/app/models/pagination';
import { ListedTask } from 'src/app/models/listedTask';
import { ListingParams } from 'src/app/models/listingParams';
import { TasksService } from 'src/app/services/tasks.service';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.scss']
})
export class TasksComponent implements OnInit {
  tasks: ListedTask[];
  pagination: Pagination;
  taskParams: ListingParams;

  constructor(private tasksService: TasksService, private router: Router, private route: ActivatedRoute) {
    this.taskParams = this.tasksService.getTaskParams();
  }

  ngOnInit(): void {
    this.loadTasks();
  }

  async loadTasks() {
    this.tasksService.getTasks(this.taskParams).subscribe(response => {
      this.tasks = response.result;
      this.pagination = response.pagination;
      console.log(this.pagination)
    })
  }

  taskPage(nameTag: string) {
    this.router.navigateByUrl('/tasks/' + nameTag + '/statement');
  }

  pageChanged(event: any) {
    this.taskParams.pageNumber = event;
    this.tasksService.setTaskParams(this.taskParams);
    this.loadTasks();
  }
}
