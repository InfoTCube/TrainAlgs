import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  assignModerator(username: string) {
    return this.http.put(this.baseUrl + 'admin/assignModerator/' + username, {});
  }

  deleteUser(username: string) {
    return this.http.delete(this.baseUrl + 'admin/deleteUser/' + username);
  }
}
