import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ListedSolution } from '../Models/listedSolutions';
import { ListingParams } from '../Models/listingParams';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class SolutionsService {
  baseUrl = environment.apiUrl;
  solutions: ListedSolution[] = [];

  constructor(private http: HttpClient) { }

  addSolution(model: any) {
    return this.http.post(this.baseUrl + 'solutions', model);
  }

  getSolutionsForCurrentUser(solutionParams: ListingParams) {
    let params = getPaginationHeaders(solutionParams.pageNumber, solutionParams.pageSize)

    return getPaginatedResult<ListedSolution[]>(this.baseUrl + 'solutions/forCurrentUser', params, this.http).pipe(
      map(response => {
        return response;
      })
    );
  }

  getSolutionsForCurrentUserAndTask(solutionParams: ListingParams, taskTag: string) {
    let params = getPaginationHeaders(solutionParams.pageNumber, solutionParams.pageSize)

    return getPaginatedResult<ListedSolution[]>(this.baseUrl + 'solutions/forCurrentUserandTask/' + taskTag, params, this.http).pipe(
      map(response => {
        return response;
      })
    );
  }
}
