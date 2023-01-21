import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ListedTask } from '../models/listedTask';
import { ListingParams } from '../models/listingParams';
import { AlgTask } from '../models/task';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class ModeratorService {
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

  getTasksToVerify(taskParams: ListingParams) {
    let params = getPaginationHeaders(taskParams.pageNumber, taskParams.pageSize)

    return getPaginatedResult<ListedTask[]>(this.baseUrl + 'moderator/verifytasks', params, this.http).pipe(
      map(response => {
        return response;
      })
    );
  }

  getTaskToVerify(nameTag: string) {
    return this.http.get<AlgTask>(this.baseUrl + 'moderator/verifytasks/' + nameTag);
  }

  verifyTask(nameTag: string) {
    return this.http.put(this.baseUrl + 'moderator/verifytasks/' + nameTag, {});
  }
}
