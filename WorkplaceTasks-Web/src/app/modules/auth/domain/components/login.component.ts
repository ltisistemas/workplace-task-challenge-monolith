import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginControllerService } from '../controllers/login-controller.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent {
    form: FormGroup;
    isLoading = false;

    constructor(private fb: FormBuilder, private controller: LoginControllerService) {
        this.form = this.fb.group({
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required]]
        });
    }

    onSubmit = () => {
        if (this.form.valid && !this.isLoading) {
            this.isLoading = true;
            this.controller.execute(this.form.value, this);
        }
    }

}