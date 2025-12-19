import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginRequestDto, LoginResponseDto } from '../dto/login-dto';

@Injectable({
    providedIn: 'root'
})
export class AuthRepositoryService {
    private readonly apiUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }

    login(payload: LoginRequestDto): Observable<LoginResponseDto> {
        return this.http.post<LoginResponseDto>(`${this.apiUrl}/auth/login`, payload);
    }
}