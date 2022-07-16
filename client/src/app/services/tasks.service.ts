import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PaginatedResult } from '../Models/pagination';
import { ListedTask } from '../Models/listedTask';
import { Task } from '../Models/task';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { ListingParams } from '../Models/listingParams';
import { AccountService } from './account.service';
import { take } from 'rxjs/operators';
import { User } from '../Models/user';

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  baseUrl = environment.apiUrl;
  tasks: ListedTask[] = [];
  taskParams: ListingParams;

  constructor(private http: HttpClient) { 
      this.taskParams = new ListingParams();
  }

  getTaskParams() {
    return this.taskParams;
  }

  setTaskParams(params: ListingParams) {
    this.taskParams = params;
  }

  resetTaskParams() {
    this.taskParams = new ListingParams();
    return this.taskParams;
  }

  getTasks(taskParams: ListingParams) {
    let params = getPaginationHeaders(taskParams.pageNumber, taskParams.pageSize)

    return getPaginatedResult<ListedTask[]>(this.baseUrl + 'tasks', params, this.http).pipe(
      map(response => {
        return response;
      })
    );
  }

  getTask(nameTag: string) {
    return this.http.get<Task>(this.baseUrl + 'tasks/' + nameTag);
  }
}
