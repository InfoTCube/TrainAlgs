import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if(error) {
          switch(error.status) {
            case 400:
              if(error.error.errors) {
                const modalStateError = [];
                for(const key in error.error.errors) {
                  if(error.error.errors[key]) {
                    modalStateError.push(error.error.errors[key])
                  }
                }
                throw modalStateError.flat();
              } else {
                throw(error.statusText, error.status);
              }
              break;
            case 401:
              this.router.navigateByUrl('/');
              throw(error.statusText, error.status);
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {state: {error: error.error}};
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;
            default:
              console.log(error);
              throw(error.statusText, error.status);
              break;
          }
        }
        return throwError(error);
      })
    );
  }
}
