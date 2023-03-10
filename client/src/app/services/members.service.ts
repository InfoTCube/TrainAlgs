import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ListingParams } from '../models/listingParams';
import { Member } from '../models/member';
import { SearchedMember } from '../models/searchedMember';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  listingParams: ListingParams;

  constructor(private http: HttpClient) {
      this.listingParams = new ListingParams();
  }

  getListingParams() {
    return this.listingParams;
  }

  setListingParams(params: ListingParams) {
    this.listingParams = params;
  }

  resetListingParams() {
    this.listingParams = new ListingParams();
    return this.listingParams;
  }
  getMember(username: string) {
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  searchMembers(listingParams: ListingParams, searchText: string) {
    let params = getPaginationHeaders(listingParams.pageNumber, listingParams.pageSize)


    return getPaginatedResult<SearchedMember[]>(this.baseUrl + 'users/search/' + searchText, params, this.http).pipe(
      map(response => {
        return response;
      })
    );
  }
}
