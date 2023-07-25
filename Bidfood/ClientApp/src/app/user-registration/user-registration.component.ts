import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CONSTANTS } from '../../shared/constants';

@Component({
  selector: 'app-user-registration',
  templateUrl: './user-registration.component.html',
  styleUrls: ['./user-registration.component.scss']
})
export class UserRegistrationComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  firstName = '';
  lastName = '';
  userRegistrationStatusMessage = '';
  submitted = false;
  isUserRegistrationInitiated = false;
  isUserRegistrationSuccess = false;

  formUserRegistration: FormGroup = new FormGroup({
    firstname: new FormControl(''),
    lastname: new FormControl(''),
  });

  ngOnInit(): void {
    this.formUserRegistration = this.formBuilder.group(
      {
        firstname: ['', Validators.required],
        lastname: ['', Validators.required]
      }
    );
  }

  onSubmit() {
    this.submitted = true;
    this.isUserRegistrationInitiated = false;
    this.isUserRegistrationSuccess = false;

    if (this.formUserRegistration.invalid) {
      return;
    }

    this.isUserRegistrationInitiated = true;
    this.http.post(this.baseUrl + 'userregistration', this.getUserDetailsRequest()).subscribe(() => {
      this.isUserRegistrationSuccess = true;
      this.userRegistrationStatusMessage = CONSTANTS.userRegistrationSuccessMessage
    }, () => {
      this.isUserRegistrationSuccess = false,
        this.userRegistrationStatusMessage = CONSTANTS.userRegistrationFailureMessage
    });
  }

  getUserDetailsRequest() {
    return {
      firstName: this.firstName,
      lastName: this.lastName
    }
  }

  hasRequiredError(control: AbstractControl) {
    return this.submitted && control.hasError('required');
  }

  showUserRegistrationStatus() {
    return this.submitted && this.isUserRegistrationInitiated;
  }

  reset() {
    this.submitted = false;
    this.isUserRegistrationInitiated = false;
    this.isUserRegistrationSuccess = false;
  }

}
