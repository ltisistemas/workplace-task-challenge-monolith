import { Injectable } from '@angular/core';
import { LoginRequestDto, LoginResponseDto } from '../../infra/dto/login-dto';
import { Observable } from 'rxjs';
import { AuthRepositoryService } from '../../infra/repositories/auth-repository.service';

export interface LoginUsecase {
    execute(payload: LoginRequestDto): Observable<LoginResponseDto>;
}

@Injectable({
    providedIn: 'root'
})
export class LoginUsecaseService implements LoginUsecase {
    constructor(private repository: AuthRepositoryService) { }

    execute(payload: LoginRequestDto): Observable<LoginResponseDto> {
        return this.repository.login(payload);
    }
}