import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { HomeComponent } from './home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { LoadingInterceptor } from './interceptors/loading.interceptor';
import { TasksComponent } from './tasks/tasks/tasks.component';
import { TaskDetailComponent } from './tasks/task-detail/task-detail.component';
import { MarkdownModule } from 'ngx-markdown';
import { FooterComponent } from './footer/footer.component';
import { SolutionsComponent } from './solutions/solutions/solutions.component';
import { SolutionDetailComponent } from './solutions/solution-detail/solution-detail.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { DropdownComponent } from './modules/dropdown/dropdown.component';
import { CodeViewerComponent } from './modules/code-viewer/code-viewer.component';
import { SharedModule } from './modules/shared.module';
import { CodeEditorComponent } from './modules/code-editor/code-editor.component';
import { TimeagoModule } from 'ngx-timeago';
import { HasRoleDirective } from './directives/has-role.directive';
import { VerifyTasksComponent } from './moderator/verify-tasks/verify-tasks.component';
import { VerifyTaskDetailComponent } from './moderator/verify-task-detail/verify-task-detail.component';
import { TaskAddingComponent } from './tasks/task-adding/task-adding.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    SignInComponent,
    SignUpComponent,
    HomeComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MemberDetailComponent,
    TasksComponent,
    TaskDetailComponent,
    FooterComponent,
    SolutionsComponent,
    SolutionDetailComponent,
    DropdownComponent,
    CodeViewerComponent,
    CodeEditorComponent,
    HasRoleDirective,
    VerifyTasksComponent,
    VerifyTaskDetailComponent,
    TaskAddingComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgxSpinnerModule,
    NgxPaginationModule,
    BrowserAnimationsModule,
    SharedModule,
    NgxChartsModule,
    MarkdownModule.forRoot(),
    TimeagoModule.forRoot(),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      },
      defaultLanguage: 'pl'
    }),
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}

export function HttpLoaderFactory(http: HttpClient): TranslateHttpLoader {
  return new TranslateHttpLoader(http);
}
