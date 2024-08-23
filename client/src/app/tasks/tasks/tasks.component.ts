import { ChangeDetectorRef, Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginatedResult, Pagination } from 'src/app/models/pagination';
import { ListedTask } from 'src/app/models/listedTask';
import { TasksService } from 'src/app/services/tasks.service';
import { AccountService } from 'src/app/services/account.service';
import { TaskParams } from 'src/app/models/taskParams';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.scss']
})
export class TasksComponent implements OnInit {
  tasks: ListedTask[];
  pagination: Pagination;
  taskParams: TaskParams;

  constructor(private tasksService: TasksService, private router: Router, private route: ActivatedRoute, public accountService: AccountService) {
    this.taskParams = this.tasksService.getTaskParams();
    this.taskParams.taskStatus = "Default";
  }

  ngOnInit(): void {
    this.loadTasks();
  }

  async loadTasks() {
    var userLoggedIn: boolean = false;
    this.accountService.currentUser$.subscribe(user => {
      userLoggedIn = user.username !== null ? true : false;
    })

    if(userLoggedIn) {
      this.tasksService.getTasksForCurrentUser(this.taskParams).subscribe(response => {
        this.tasks = response.result;
        this.pagination = response.pagination;
      })
    } else {
      this.tasksService.getTasks(this.taskParams).subscribe(response => {
        this.tasks = response.result;
        this.pagination = response.pagination;
      })
    }
  }

  taskPage(nameTag: string) {
    this.router.navigateByUrl('/tasks/' + nameTag + '/statement');
  }

  pageChanged(event: any) {
    this.taskParams.pageNumber = event;
    this.tasksService.setTaskParams(this.taskParams);
    this.loadTasks();
  }

  filterByProgress(status: string) {
    this.taskParams.taskStatus = status;
    this.loadTasks();
  }
}
