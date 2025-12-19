import { Injectable } from '@angular/core';
import { LoginUsecase, LoginUsecaseService } from '../usecases/login-usecase.service';
import { LoginRequestDto, LoginResponseDto } from '../../infra/dto/login-dto';
import { Observable } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { LoginComponent } from '../components/login.component';

@Injectable({
    providedIn: 'root'
})
export class LoginControllerService {
    constructor(private usecase: LoginUsecaseService, private snackBar: MatSnackBar, private router: Router) { }

    execute(payload: LoginRequestDto, component: LoginComponent) {
        try {
            if (payload.email === '' || payload.password === '') {
                throw new Error('Email e senha são obrigatórios');
            }

            this.usecase.execute(payload).subscribe({
                next: (response) => {
                    component.isLoading = false;
                    this.snackBar.open('Login realizado com sucesso', 'Fechar', {
                        duration: 3000,
                        panelClass: 'success-snackbar'
                    });

                    localStorage.setItem('token', response.token);
                    localStorage.setItem('user', JSON.stringify(response));
                    this.router.navigate(['']);
                },
                error: (error) => {
                    component.isLoading = false;
                    const errorMessage = error?.error?.message || error?.message || 'Erro ao realizar login. Verifique suas credenciais.';
                    this.snackBar.open(errorMessage, 'Fechar', {
                        duration: 3000,
                        panelClass: 'error-snackbar'
                    });
                }
            });
        } catch (error: any) {
            component.isLoading = false;
            this.snackBar.open(error.message, 'Fechar', {
                duration: 3000,
                panelClass: 'error-snackbar'
            });
        }
    }
}