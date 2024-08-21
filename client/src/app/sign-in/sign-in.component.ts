import { Component, OnInit } from '@angular/core';
import { AbstractControl, UntypedFormControl, UntypedFormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {
  signInForm: UntypedFormGroup;
  validationErrors: string[] = [];

  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.signInForm = new UntypedFormGroup({
      username: new UntypedFormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(32)]),
      password: new UntypedFormControl('', [Validators.required, Validators.minLength(7), Validators.maxLength(32), this.containsAllTypes()]),
      rememberMe: new UntypedFormControl(false)
    });
  }

  containsAllTypes(): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value?.match(/^(?=.*\d)(?=.*[@$!%*#?&])(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{7,32}$/) ? null : {containsAllTypes: true};
    }
  }

  login() {
    this.accountService.login(this.signInForm.value).subscribe({
      next: (response) => {
        this.router.navigate(['/']);
      }, error: (error: string) => {
        this.router.navigateByUrl('/sign_in');
        this.signInForm.reset();
        if(error == '401') error = 'Username or password incorrect';
        this.validationErrors = [error];
      }
    });
  }
}
