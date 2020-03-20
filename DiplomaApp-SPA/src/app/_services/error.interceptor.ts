import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(
    req: import('@angular/common/http').HttpRequest<any>,
    next: import('@angular/common/http').HttpHandler
  ): import('rxjs').Observable<import('@angular/common/http').HttpEvent<any>> {
    // what error is happening next
    return next.handle(req).pipe(
      catchError(error => {
        if (error.status === 401) {
          return throwError(error.statusText);
        }
        if (error instanceof HttpErrorResponse) {
          // wee need to get applicationError from our server (500 internal error type of errors)
          // we need to get it from header and it's name needs to be the same as we define it in diplomaapp.api ->helpers -> extensions.cs
          const applicationError = error.headers.get('Application-Error');
          if (applicationError) {
            return throwError(applicationError);
          }
          // model state errors
          // error.error is how specified in Angular in our browser
          // HttpErrorResponse.error, so error.error is next level down error
          const serverError = error.error;
          // next one is foe passwords errors etc
          // we are looking into object that contains an array (error.errors) and loop throughtover
          // the keys of an object and do smth with the values of that object
          let modalStateError = '';
          if (serverError.errors && typeof serverError.errors === 'object') {
            for (const key in serverError.errors) {
              // exsistence of a key
              // if it's a password error, the key is 'Password' in browser console
              if (serverError.errors[key]) {
                // list of strings separated by a new line
                modalStateError += serverError.errors[key] + '\n';
              }
            }
          }
          // if we have smth in modalStateError and it's not an empty string
          // we will throw modalStateError and serverError
          // or if we don't have any modalStateErrors we will throw an serverError or 'Server Error'
          return throwError(modalStateError || serverError || 'Server Error');
        }
      })
    );
  }
}

// we are registering a new ErrorInterceptorProvider to the AngularHttpInterceptor
// array of providers that is already exists
// then this ErrorInterceptorProvider must be added to app.module.providers
export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
};
