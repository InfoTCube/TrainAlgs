import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PaginatedResult } from '../Models/pagination';
import { ListedTask } from '../Models/listedTask';
import { Task } from '../Models/task';
import { TaskParams } from '../Models/taskParams';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  baseUrl = environment.apiUrl;
  tasks: ListedTask[] = [];
  

  constructor(private http: HttpClient) { }

  getTasks(taskParams: TaskParams) {
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
