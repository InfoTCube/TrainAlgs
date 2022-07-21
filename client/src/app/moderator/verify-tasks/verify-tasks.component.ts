import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ListedTask } from 'src/app/models/listedTask';
import { ListingParams } from 'src/app/models/listingParams';
import { Pagination } from 'src/app/models/pagination';
import { ModeratorService } from 'src/app/services/moderator.service';

@Component({
  selector: 'app-verify-tasks',
  templateUrl: './verify-tasks.component.html',
  styleUrls: ['./verify-tasks.component.scss']
})
export class VerifyTasksComponent implements OnInit {
  tasks: ListedTask[];
  pagination: Pagination;
  taskParams: ListingParams;
  confirm: boolean;
  currentNameTag: string = "";

  constructor(private moderatorService: ModeratorService, private router: Router, private route: ActivatedRoute) {
    this.taskParams = this.moderatorService.getTaskParams();
  }

  ngOnInit(): void {
    this.loadTasksToVerify();
  }

  async loadTasksToVerify() {
    this.moderatorService.getTasksToVerify(this.taskParams).subscribe(response => {
      this.tasks = response.result;
      this.pagination = response.pagination;
    })
  }

  taskPage(nameTag: string) {
    this.router.navigateByUrl('/moderator/verify-tasks/' + nameTag);
  }

  pageChanged(event: any) {
    this.taskParams.pageNumber = event;
    this.moderatorService.setTaskParams(this.taskParams);
    this.loadTasksToVerify();
  }

  verifyTask($event) {
    $event.stopPropagation();
    this.moderatorService.verifyTask(this.currentNameTag).subscribe(out => {
      this.loadTasksToVerify();
      this.toggleConfirm($event, "");
    });
  }

  toggleConfirm($event, nameTag: string) {
    $event.stopPropagation();
    this.currentNameTag = nameTag;
    this.confirm = !this.confirm;
  }
}
