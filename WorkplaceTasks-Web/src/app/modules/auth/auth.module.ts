import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './domain/components/login.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { LoginUsecaseService } from './domain/usecases/login-usecase.service';
import { AuthRepositoryService } from './infra/repositories/auth-repository.service';

const routes: Routes = [
    {
        path: '',
        component: LoginComponent
    }
];

@NgModule({
    declarations: [
        LoginComponent
    ],
    imports: [
        CommonModule,
        SharedModule,
        RouterModule.forChild(routes)
    ],
    providers: [
        LoginUsecaseService,
        AuthRepositoryService
    ]
})
export class AuthModule { }