import { Component, OnInit } from '@angular/core';
import { AbstractControl, UntypedFormControl, UntypedFormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {
  signUpForm: UntypedFormGroup;
  validationErrors: string[] = [];

  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.signUpForm = new UntypedFormGroup({
      firstName: new UntypedFormControl('', Validators.required),
      lastName: new UntypedFormControl('', Validators.required),
      email: new UntypedFormControl('', [Validators.required, Validators.email]),
      username: new UntypedFormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(32)]),
      password: new UntypedFormControl('', [Validators.required, Validators.minLength(7), Validators.maxLength(32), this.containsAllTypes()]),
      confirmPassword: new UntypedFormControl('', [Validators.required, this.matchValues('password')])
    });
    this.signUpForm.controls.password.valueChanges.subscribe(() => {
      this.signUpForm.controls.confirmPassword.updateValueAndValidity();
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value == control?.parent?.controls[matchTo].value 
        ? null : {isMatching: true};
    };
  }

  containsAllTypes(): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value.match(/^(?=.*\d)(?=.*[@$!%*#?&])(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{7,32}$/) ? null : {containsAllTypes: true};
    }
  }

  register() {
    this.accountService.register(this.signUpForm.value).subscribe(response => {
      this.router.navigateByUrl('/');
    }, error => {
      this.validationErrors = error;
    });
  }
}
