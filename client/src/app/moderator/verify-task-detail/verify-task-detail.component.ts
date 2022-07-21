import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlgTask } from 'src/app/models/task';
import { ModeratorService } from 'src/app/services/moderator.service';

@Component({
  selector: 'app-verify-task-detail',
  templateUrl: './verify-task-detail.component.html',
  styleUrls: ['./verify-task-detail.component.scss']
})
export class VerifyTaskDetailComponent implements OnInit {
  task: AlgTask;
  markdown = '';

  constructor(private moderatorService: ModeratorService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.getTaskToVerify();
  }

  getTaskToVerify() {
    this.moderatorService.getTaskToVerify(this.route.snapshot.paramMap.get('nameTag')).subscribe(response => {
      this.task = response;
      this.markdown = this.replaceInlineCode(response.content);
    })
  }

  replaceInlineCode(md: string) : string {
    let matchString = '.';
    while(matchString != null) {
      let match = md.match(/(?=`)`(?!`)[^`]*(?=`)`(?!`)/);
      if(match == null) {
        matchString = null;
        continue;
      }
      md = `${md.substring(0, match.index)}<span class="bg-gray-300 dark:bg-gray-700 p-1 rounded-md">${match[0].substring(1, match[0].length-1)}</span>${md.substring(match.index+match[0].length)}`;
    }
    return md;
  }
}
