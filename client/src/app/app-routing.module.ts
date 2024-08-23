import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersComponent } from './admin/users/users.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { AdminGuard } from './guards/admin.guard';
import { AuthGuard } from './guards/auth.guard';
import { ModeratorGuard } from './guards/moderator.guard';
import { UnauthGuard } from './guards/unauth.guard';
import { HomeComponent } from './home/home.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { VerifyTaskDetailComponent } from './moderator/verify-task-detail/verify-task-detail.component';
import { VerifyTasksComponent } from './moderator/verify-tasks/verify-tasks.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { SolutionDetailComponent } from './solutions/solution-detail/solution-detail.component';
import { SolutionsComponent } from './solutions/solutions/solutions.component';
import { TaskAddingComponent } from './tasks/task-adding/task-adding.component';
import { TaskDetailComponent } from './tasks/task-detail/task-detail.component';
import { TasksComponent } from './tasks/tasks/tasks.component';
import { CompetitionsComponent } from './competitions/competitions/competitions.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'tasks', component: TasksComponent},
  {path: 'competitions', component: CompetitionsComponent},
  {path: 'tasks/:nameTag/statement', component: TaskDetailComponent},
  {path: 'tasks/add-task', component: TaskAddingComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: 'members/:username', component: MemberDetailComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'solutions', component: SolutionsComponent},
      {path: 'solutions/:id', component: SolutionDetailComponent},
      {path: 'tasks/:nameTag/solutions', component: TaskDetailComponent},
      {path: 'tasks/:nameTag/submit', component: TaskDetailComponent},
      {path: 'moderator/verify-tasks', component: VerifyTasksComponent, canActivate: [ModeratorGuard]},
      {path: 'moderator/verify-tasks/:nameTag', component: VerifyTaskDetailComponent, canActivate: [ModeratorGuard]},
      {path: 'admin/users', component: UsersComponent, canActivate: [AdminGuard]}
    ]
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [UnauthGuard],
    children: [
      {path: 'sign_up', component: SignUpComponent},
      {path: 'sign_in', component: SignInComponent},
    ]
  },
  {path: '**', component: NotFoundComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
