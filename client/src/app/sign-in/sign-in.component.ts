import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
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
  signInForm: FormGroup;
  validationErrors: string[] = [];

  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.signInForm = new FormGroup({
      username: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(32)]),
      password: new FormControl('', [Validators.required, Validators.minLength(7), Validators.maxLength(32), this.containsAllTypes()])
    });
  }

  containsAllTypes(): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value.match(/^(?=.*\d)(?=.*[@$!%*#?&])(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{7,32}$/) ? null : {containsAllTypes: true};
    }
  }

  login() {
    this.accountService.login(this.signInForm.value).subscribe(response => {
      this.router.navigateByUrl('/');
    }, error => {
      this.validationErrors = error;
    });
  }
}
