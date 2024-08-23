import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ListedCompetitions } from '../models/listedCompetitions';
import { ListingParams } from '../models/listingParams';
import { HttpClient } from '@angular/common/http';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { map } from 'rxjs';
import { Competition } from '../models/competition';

@Injectable({
  providedIn: 'root'
})
export class CompetitionsService {
  baseUrl = environment.apiUrl;
  comps: ListedCompetitions[] = [];
  compParams: ListingParams;

  constructor(private http: HttpClient) {
      this.compParams = new ListingParams();
  }

  getCompParams() {
    return this.compParams;
  }

  setCompParams(params: ListingParams) {
    this.compParams = params;
  }

  resetCompParams() {
    this.compParams = new ListingParams();
    return this.compParams;
  }

  addCompetition(model: any) {
    return this.http.post(this.baseUrl + 'competitions', model);
  }

  getCompetitions(compParams: ListingParams) {
    let params = getPaginationHeaders(compParams.pageNumber, compParams.pageSize)

    return getPaginatedResult<ListedCompetitions[]>(this.baseUrl + 'competitions', params, this.http).pipe(
      map(response => {
        console.log(response)
        return response;
      })
    );
  }

  getCompetition(id: number) {
    return this.http.get<Competition>(this.baseUrl + 'competitions/' + id);
  }
}
