import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PaginatedResult } from '../models/pagination';
import { ListedTask } from '../models/listedTask';
import { AlgTask } from '../models/task';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { ListingParams } from '../models/listingParams';
import { AccountService } from './account.service';
import { take } from 'rxjs/operators';
import { User } from '../models/user';
import { TaskParams } from '../models/taskParams';

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  baseUrl = environment.apiUrl;
  tasks: ListedTask[] = [];
  taskParams: TaskParams;

  constructor(private http: HttpClient) {
      this.taskParams = new TaskParams();
  }

  getTaskParams() {
    return this.taskParams;
  }

  setTaskParams(params: TaskParams) {
    this.taskParams = params;
  }

  resetTaskParams() {
    this.taskParams = new TaskParams();
    return this.taskParams;
  }

  getTasks(taskParams: TaskParams) {
    let params = getPaginationHeaders(taskParams.pageNumber, taskParams.pageSize)

    return getPaginatedResult<ListedTask[]>(this.baseUrl + 'tasks', params, this.http).pipe(
      map(response => {
        return response;
      })
    );
  }

  getTask(nameTag: string) {
    return this.http.get<AlgTask>(this.baseUrl + 'tasks/' + nameTag);
  }

  getTasksForCurrentUser(taskParams: TaskParams) {
    let params = getPaginationHeaders(taskParams.pageNumber, taskParams.pageSize)
    params = params.append('taskStatus', taskParams.taskStatus.toString());

    return getPaginatedResult<ListedTask[]>(this.baseUrl + 'tasks/forCurrentUser', params, this.http).pipe(
      map(response => {
        return response;
      })
    );
  }

  addTask(model: any) {
    return this.http.post(this.baseUrl + 'tasks', model);
  }
}
